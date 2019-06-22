using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.Video;
using System;

namespace Onwijs.StandardData
{
    public class standardDataObjectVideo : standardDataObject
    {
        private VideoPlayer videoComponent;

        public override void Start()
        {
            base.Start();
            type = "video";
            videoComponent = GetComponent<VideoPlayer>();
        }
        public override void loadAsset(bool downloadOverride = false)
        {
            base.loadAsset(downloadOverride);
            string Path = Application.persistentDataPath + data.localPath;
            if (checkLocalPath(Path) && !downloadOverride)
            {
                if(videoComponent == null)
                {
                    videoComponent = GetComponent<VideoPlayer>();
                }
                videoComponent.url = Path;
                StartCoroutine(PlayVideo());
            }
            else if (data.dataUrl != null && data.dataUrl != "" && tries < 2)
            {
                StartCoroutine(DownloadVideo(data.dataUrl));
            }
            else
            {
                Debug.Log("no data found for " + type + " with localIdentifier " + localIdentifier);
            }
        }
        IEnumerator DownloadVideo(string MediaUrl)
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(MediaUrl);
            DownloadHandler receiveBundle = new DownloadHandlerBuffer();
            webRequest.downloadHandler = receiveBundle;
            webRequest.SendWebRequest();
            if (!Directory.Exists(Application.persistentDataPath + "/videos/"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/videos");
            }
            string[] words = MediaUrl.Split('/');
            string[] fileName = words[words.Length - 1].Split('.');
            string newLocalPath = "/videos/" + words[words.Length - 1];
            Debug.Log(Application.persistentDataPath + newLocalPath);
            while(!webRequest.isDone)
            {
                yield return new WaitForFixedUpdate();
            }
            Debug.Log("done");
            File.WriteAllBytes(Application.persistentDataPath + newLocalPath,webRequest.downloadHandler.data);
            data.localPath = newLocalPath;
            loadAsset();
            Save();
        }
        IEnumerator PlayVideo()
        {
            videoComponent.Prepare();
            while (!videoComponent.isPrepared)
            {
                yield return new WaitForFixedUpdate();
            }
            videoComponent.Play();
        }
    }
}

