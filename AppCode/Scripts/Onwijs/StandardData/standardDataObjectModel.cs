using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

namespace Onwijs.StandardData
{
    public class standardDataObjectModel : standardDataObject
    {
        private MeshFilter meshComponent;
        private MeshRenderer rendererComponent;
        public Mesh defaultMesh;
        public Material defaultMaterial;

        public override void Start()
        {
            base.Start();
            type = "model";
            meshComponent = GetComponent<MeshFilter>();
            rendererComponent = GetComponent<MeshRenderer>();

        }
        public override void loadAsset(bool downloadOverride = false)
        {
            base.loadAsset(downloadOverride);
            string Path = Application.persistentDataPath + data.localPath;
            if (checkLocalPath(Path) && !downloadOverride)
            {
                //StartCoroutine(LoadModel(Path))
                ObjImporter importer = new ObjImporter();
                Mesh mesh = importer.ImportFile(Path);
                setMesh(mesh);
            }
            else if (data.dataUrl != null && data.dataUrl != "" && tries < 2)
            {
                StartCoroutine(DownloadModel(data.dataUrl));
            }
            else
            {
                setMesh(defaultMesh);  
                Debug.Log("no data found for " + type + " with localIdentifier " + localIdentifier);
            }
        }
        private void setMesh(Mesh mesh)
        {
            if (meshComponent == null)
            { 
                meshComponent = GetComponent<MeshFilter>();
            }
            meshComponent.mesh = mesh;
            transform.localScale = new Vector3(0.000001f, 0.000001f, 0.000001f);
            if (rendererComponent == null)
            {
                rendererComponent = GetComponent<MeshRenderer>();
            }
            Material[] newMaterials;
            if (mesh.subMeshCount > 1)
            { 
                newMaterials = new Material[mesh.subMeshCount];
                for (int i = 0; i == mesh.subMeshCount; i++)
                {
                    Debug.Log(i);
                    newMaterials[i] = defaultMaterial;
                }
            }
            else
            {
                newMaterials = new Material[1];
                newMaterials[0] = defaultMaterial;
            }
            Debug.Log(mesh.bounds);
            rendererComponent.materials = newMaterials; // set length of array yay c# 
            newMaterials.CopyTo(rendererComponent.materials, newMaterials.Length -1); // copy the content of the array to materials array
            // any code required to handle materials ect...
        }
        //IEnumerator LoadModel(string path)
        //{
           
        //}
        IEnumerator DownloadModel(string MediaUrl)
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(MediaUrl);
            DownloadHandler receiveBundle = new DownloadHandlerBuffer();
            webRequest.downloadHandler = receiveBundle;
            webRequest.SendWebRequest();
            if (!Directory.Exists(Application.persistentDataPath + "/models/"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/models");
            }
            string[] words = MediaUrl.Split('/');
            string[] fileName = words[words.Length - 1].Split('.');
            string newLocalPath = "/models/" + words[words.Length - 1];
            Debug.Log(Application.persistentDataPath + newLocalPath);
            while (!webRequest.isDone)
            {
                yield return new WaitForFixedUpdate();
            }
            Debug.Log("done");
            File.WriteAllBytes(Application.persistentDataPath + newLocalPath, webRequest.downloadHandler.data);
            data.localPath = newLocalPath;
            loadAsset();
            Save();
        }
    }
}
