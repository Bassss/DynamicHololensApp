using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
namespace Onwijs.StandardData
{
    public class standardDataObjectAudio : standardDataObject
    {
        private AudioSource audioComponent;

        public override void Start()
        {
            base.Start();
            type = "audio";
            audioComponent = GetComponent<AudioSource>();
        }
        public override void loadAsset(bool downloadOverride = false)
        {
            base.loadAsset(downloadOverride);
            string Path = Application.persistentDataPath + data.localPath;
            
            if (checkLocalPath(Path) && !downloadOverride)
            {
                if (audioComponent == null)
                {
                   audioComponent = GetComponent<AudioSource>();
                }
                StartCoroutine(LoadAudioFile(Path));
              
            }
            else if (data.dataUrl != null)
            {
                StartCoroutine(DownloadAudio(data.dataUrl));
            }
            else
            {
                Debug.Log("no data found for " + type + " with localIdentifier " + localIdentifier);
            }
        }
        IEnumerator LoadAudioFile(string Path)
        {
            string[] words = Path.Split('/');
            string[] fileName = words[words.Length - 1].Split('.');
            UnityWebRequest webRequest = UnityWebRequestMultimedia.GetAudioClip(Path, (AudioType)AudioType.Parse(typeof(AudioType), "UNKNOWN"));
            yield return webRequest.SendWebRequest();
            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.Log("help: " + webRequest.error);
            }
            else
            {
                AudioClip newAudio = DownloadHandlerAudioClip.GetContent(webRequest);
                audioComponent.clip = newAudio;
                //audioComponent.Play();
            }
        }
        IEnumerator DownloadAudio(string MediaUrl)
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(MediaUrl);
            DownloadHandler receiveBundle = new DownloadHandlerBuffer();
            webRequest.downloadHandler = receiveBundle;
            webRequest.SendWebRequest();
            if (!Directory.Exists(Application.persistentDataPath + "/audio/"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/audio");
            }
            string[] words = MediaUrl.Split('/');
            string[] fileName = words[words.Length - 1].Split('.');
            string newLocalPath = "/audio/" + words[words.Length - 1];
            Debug.Log(Application.persistentDataPath + newLocalPath);
            while (!webRequest.isDone)
            {
                yield return new WaitForFixedUpdate();
            }
            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.Log("help: " + webRequest.error);
            }
            else
            {
                Debug.Log("done downloading " + words[words.Length - 1]);
                File.WriteAllBytes(Application.persistentDataPath + newLocalPath, webRequest.downloadHandler.data);
                data.localPath = newLocalPath;
                loadAsset();
                Save();
            }
        }
    }
}