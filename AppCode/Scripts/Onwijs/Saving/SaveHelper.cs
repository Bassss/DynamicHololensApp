using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using UnityEngine.XR.WSA;
using UnityEngine.XR.WSA.Persistence;
using UnityEngine.Networking;
using Onwijs.Enums;
using System.Linq;
namespace Onwijs.Saving
{
    public class SaveHelper : MonoBehaviour
    {
        private List<SaveData> objectsToLoad = new List<SaveData>();
        private List<AnchorData> anchorObjectsToLoad = new List<AnchorData>();
        public List<GameObject> prefabs = new List<GameObject>();
        private WorldAnchorManager worldAnchorManager;
        private List<GameObject> anchorObjects = new List<GameObject>();
        private List<GameObject> hologramObjects = new List<GameObject>();

        public GameObject anchorPoint;
        private string currentMode;

        // Start is called before the first frame update
        void Start()
        {
            worldAnchorManager = GameObject.Find("AnchorManager").GetComponent<WorldAnchorManager>();
            currentMode = UIProperties.saveDataMode;
            StartCoroutine(CheckIfSaved());
        }
        // we use this to check if the application is closed onApplicationQuit() does not work because windows.
        void OnApplicationFocus(bool focus)
        {
            if (!focus)
            {
                Debug.Log("Quit!");
                SaveHandler.anchorCleanUp();
                SaveHandler.saveToServer();
            }
        }
       
        public void LoadOtherSaveFile(string mode)
        {
            SaveHandler.saveToServer();
            SaveHandler.ClearObjects();
            UIProperties.saveDataMode = mode;
            string link = UIProperties.serverLink + "SaveData/" + mode;
            currentMode = mode;
            StartCoroutine(DownloadSaveFile(link));
        }
        IEnumerator CheckIfSaved()
        {
            bool saving = false;
            while (PlayerPrefs.GetString("isSaved") == "False" && !UIProperties.ServerError)
            {
               
                if (!saving)
                {
                    saving = true;
                    Debug.Log("Saving");
                    SaveHandler.Load();
                    SaveHandler.saveToServer();
                }
                yield return new WaitForFixedUpdate();
            }
            string link = UIProperties.serverLink + "SaveData" + currentMode;
            StartCoroutine(DownloadSaveFile(link));
        }
        IEnumerator DownloadSaveFile(string Url)
        {
            Debug.Log(Url);
            UnityWebRequest webRequest = UnityWebRequest.Get(Url);
            webRequest.SetRequestHeader("x-access-id", SystemInfo.deviceUniqueIdentifier);
            DownloadHandler receiveBundle = new DownloadHandlerBuffer();
            webRequest.downloadHandler = receiveBundle;
            webRequest.SendWebRequest();

            while (!webRequest.isDone)
            {
                yield return new WaitForFixedUpdate();
            }
            if (webRequest.responseCode == 200)
            {
                DownloadWrapper download = new DownloadWrapper();
                download = JsonUtility.FromJson<DownloadWrapper>(webRequest.downloadHandler.text);
                SaveHandler.saveDataWrapper = download.SaveDataWrapper;

               
            }
            else
            {
                UIProperties.ServerError = true;
                Debug.LogError("SaveFile could not be downloaded, please check ur internet");
                Debug.LogError("falling back to local file if found");
                SaveHandler.Load();
            }
         
      
            StartCoroutine(WaitForStoreToLoad());
            // wait for store to get ready
        }
        IEnumerator WaitForStoreToLoad()
        {
            #if !UNITY_EDITOR
            while(!worldAnchorManager.ready)
            {
                 yield return new WaitForFixedUpdate();
            }
            Debug.Log("AnchorStore done loading");
            #else
            while(false)
            {
                 yield return new WaitForFixedUpdate();
            }
            Debug.Log("In editor, no anchorStore to load");
            #endif
            StartCoroutine(LoadAnchorObjects());
        }
        IEnumerator LoadAnchorObjects()
        {
            anchorObjectsToLoad = new List<AnchorData>(SaveHandler.saveDataWrapper.anchorData);
            // Get anchor per anchoreSave
            Debug.Log(anchorObjectsToLoad.Count+" anchors found in save file");
            while (anchorObjectsToLoad.Count > 0 )
            {
                //Debug.Log(anchorObjectsToLoad.Count+" anchors to go");
                if(!anchorObjectsToLoad[0].deleted)
                {
                    GameObject cube = Instantiate(anchorPoint);
                    cube.name = anchorObjectsToLoad[0].SpatialAnchorID;
                    cube.GetComponent<AnchorHelper>().anchorData = anchorObjectsToLoad[0];
                    #if UNITY_EDITOR
                        cube.transform.position =  anchorObjectsToLoad[0].unityLocation;
                    #else
                        WorldAnchor worldAnchor = worldAnchorManager.AnchorStore.Load(cube.name, cube); 
                    #endif
                    anchorObjects.Add(cube);
                    cube.GetComponent<AnchorHelper>().checkForChilderen();
                    cube.GetComponent<AnchorHelper>().DebugObject.SetActive((PlayerPrefs.GetString("UIButtonDebug Anchors button") == "True"));
                }
                else
                {
                    SaveHandler.deletedAnchors.Add(anchorObjectsToLoad[0].id);
                }

                anchorObjectsToLoad.Remove(anchorObjectsToLoad[0]);
                yield return new WaitForFixedUpdate();
            }
            SaveHandler.allAnchors = anchorObjects;
             StartCoroutine(ObjectLoader());
        }
        IEnumerator ObjectLoader()
        {
            objectsToLoad = new List<SaveData>(SaveHandler.saveDataWrapper.saveData);
            Debug.Log(objectsToLoad.Count+" objects found in save file");
            while (objectsToLoad.Count > 0)
            {
               // Debug.Log(objectsToLoad.Count+" objects to go");
                if(!objectsToLoad[0].deleted)
                {
                    GameObject cube = Instantiate(prefabs.Where(obj => obj.name == objectsToLoad[0].type).SingleOrDefault());
                    cube.GetComponent<Rigidbody>().isKinematic = true;
                    cube.name = objectsToLoad[0].name;
                    if( objectsToLoad[0].anchorId != null && objectsToLoad[0].anchorId > 0 )
                    {
                        
                      //  Debug.Log("object "+objectsToLoad[0].id+" is connected to anchor "+objectsToLoad[0].anchorId);
                        cube.transform.parent = anchorObjects.Where(obj => obj.name == objectsToLoad[0].anchorId + "").SingleOrDefault().transform;
                        cube.transform.localPosition = objectsToLoad[0].localLocation;
                        cube.transform.localRotation = objectsToLoad[0].localRotation;
                        cube.transform.localScale = objectsToLoad[0].localScale;
                    }
                    else
                    {
                        cube.transform.position = objectsToLoad[0].unityLocation;
                    }
                    cube.GetComponent<HologramSaver>().setData(objectsToLoad[0]);
                    cube.GetComponent<HologramSaver>().type = (ObjectType)System.Enum.Parse(typeof(ObjectType), objectsToLoad[0].type);
                    hologramObjects.Add(cube);
                }
                else
                {
                    SaveHandler.deletedHolograms.Add(objectsToLoad[0].id);
                }
                objectsToLoad.Remove(objectsToLoad[0]);
                yield return new WaitForFixedUpdate();
            }
            SaveHandler.allObjects = hologramObjects;
        }

    }
}
