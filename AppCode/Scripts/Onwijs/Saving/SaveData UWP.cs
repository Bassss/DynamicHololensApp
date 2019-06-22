using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using HoloToolkit.Unity;
using Onwijs.Enums;
using UnityEngine.Networking;

namespace Onwijs.Saving
{
    [System.Serializable]
    public class DownloadWrapper
    {
        public bool success;
        public SaveDataWrapper SaveDataWrapper;
    }
    [System.Serializable]
    public class SaveDataWrapper
    {
        public List<AnchorData> anchorData = new List<AnchorData>();
        public List<SaveData> saveData = new List<SaveData>();
    }
    [System.Serializable]
    public class SaveData
    {
        public int id;
        public string name;
        public string type;
        public Vector3 unityLocation;
        public Vector3 localLocation;
        public Quaternion localRotation;
        public Vector3 localScale;
        public int anchorId;
        public bool newSave = true;
        public bool deleted = false;
        public List<standardObjectData> standardObjectData = new List<standardObjectData>();
    }
    [System.Serializable]
    public class standardObjectData
    {
        public string type;
        public int localIdentifier;
        public string localPath;
        public string dataUrl;
    }
    [System.Serializable]
    public class AnchorData
    {
        public int id;
        public string SpatialAnchorID;
        public Vector3 unityLocation;
        public bool deleted = false;
    }
    public class SaveHandler
    {
        public static SaveDataWrapper saveDataWrapper;
        public static List<GameObject> allAnchors = new List<GameObject>();
        public static List<GameObject> allObjects = new List<GameObject>();
        public static List<int> deletedAnchors = new List<int>();
        public static List<int> deletedHolograms = new List<int>();

        private static string gameDataProjectFilePath = "/data.json";


        public static void Load()
        {
            string filePath = Application.persistentDataPath + gameDataProjectFilePath;
            Debug.Log(filePath);
            if (File.Exists(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath);
                SaveHandler.saveDataWrapper = JsonUtility.FromJson<SaveDataWrapper>(dataAsJson);
            }
            else
            {
                SaveHandler.saveDataWrapper = new SaveDataWrapper();
            }
        }
     

        public static void Save()
        {
            Debug.Log("Saving file");
            string dataAsJson = JsonUtility.ToJson(SaveHandler.saveDataWrapper, true);
            string filePath = Application.persistentDataPath + gameDataProjectFilePath;
            File.WriteAllText(filePath, dataAsJson);
            //saveToServer(); we limit this to keep performance high
        }
        public static void anchorCleanUp()
        {
            Debug.Log("CleaningUp anchors");
            List<GameObject> anchorsToCheck = new List<GameObject>(allAnchors);
            foreach (GameObject anchor in anchorsToCheck)
            {
                anchor.GetComponent<AnchorHelper>().deleteIfEmpty();
            }
        }
        public static void saveToServer()
        {
            anchorCleanUp();
            Debug.Log("Saving to server");
            // set player pref is saved to false
            PlayerPrefs.SetString("isSaved", "False");
            string dataAsJson = JsonUtility.ToJson(SaveHandler.saveDataWrapper, true);
            UnityWebRequest webRequest = UnityWebRequest.Put(UIProperties.serverLink+"SaveData" + UIProperties.saveDataMode, dataAsJson);
            webRequest.SetRequestHeader("x-access-id", SystemInfo.deviceUniqueIdentifier);
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SendWebRequest();
            // would love to start a Coroutine but i cant in a static class like this :(
           
            StaticCoroutine.Start("SaveChecker", webRequest );
            // wait for request if true set to true
        }
        public static void EmptySave()
        {
            SaveHandler.saveDataWrapper = new SaveDataWrapper();
            SaveHandler.Save();
            GameObject.Find("AnchorManager").GetComponent<WorldAnchorManager>().RemoveAllAnchors();
            ClearObjects();
        }
        public static void ClearObjects()
        {
            for (int i = 0; i < allAnchors.Count; i++)
            {
                GameObject.Destroy(allAnchors[i]);
            }
            allAnchors.Clear();
            for (int i = 0; i < allObjects.Count; i++)
            {
                GameObject.Destroy(allObjects[i]);
            }
            allObjects.Clear();
        }
        public static int getFirstEmptyAnchorSpot()
        {
            if(SaveHandler.deletedAnchors.Count == 0)
            {
                AnchorData data = new AnchorData();
                data.id = SaveHandler.saveDataWrapper.anchorData.Count + 1;
                SaveHandler.saveDataWrapper.anchorData.Add(data);
               return data.id -1;
            }
            else
            {
                int value = SaveHandler.deletedAnchors[0];
                SaveHandler.deletedAnchors.Remove(deletedAnchors[0]);
                return value -1;
            }
        }
        public static int getFirstEmptyHologramSpot()
        {
            if(SaveHandler.deletedHolograms.Count == 0)
            {
                SaveData data = new SaveData();
                data.id = SaveHandler.saveDataWrapper.saveData.Count + 1;
                SaveHandler.saveDataWrapper.saveData.Add(data);
                return data.id -1;
            }
            else
            {
                int value = SaveHandler.deletedHolograms[0];
                SaveHandler.deletedHolograms.Remove(deletedHolograms[0]);
                return value -1;
            }
        }

    }
}