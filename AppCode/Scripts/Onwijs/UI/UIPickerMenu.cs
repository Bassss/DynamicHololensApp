using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onwijs.Saving;
using Onwijs.StandardData;
using Onwijs.Enums;
using UnityEngine.Networking;
using UnityEngine.Events;
namespace Onwijs.UI
{
    public class UIPickerMenu : MonoBehaviour
    {
        public int itemsPerPage = 5;
        private int currentPageNumber = 0;
        private string selectedFile;
        public standardDataHelper ObjectToFill;
        private standardDataObject[] standardObjectDatas;
        public int currentStandardDataObject;
        public GameObject menuItemPrefab;
        public List<AssetDownloadWrapper> ServerItems = new List<AssetDownloadWrapper>(); // no local items cuz the online link always needs to be saved
        public List<GameObject> menuItems = new List<GameObject>();
        private string currentType;

        public void LoadObject()
        {
            ObjectToFill = UIProperties.ActiveObject.GetComponent<standardDataHelper>();
            standardObjectDatas = ObjectToFill.getChilderen();
            currentStandardDataObject = 0;
            StartCoroutine(DownloadMenuItems(standardObjectDatas[currentStandardDataObject].type));
        }
        public void addItemToMenu(string name, int number)
        {
            GameObject menuItem = Instantiate(menuItemPrefab);
            menuItem.transform.parent = gameObject.transform;
            menuItem.name = number + "";
        
            menuItem.transform.localPosition = new Vector3(0, 0.4f - (0.15f * menuItems.Count), -1);
            menuItem.transform.localScale = new Vector3(0.9f, 0.1f, 0.1f);
            menuItem.transform.localRotation = Quaternion.Euler(0,0,0);
            menuItem.GetComponentInChildren<TextMesh>().text = name;
            menuItem.GetComponent<UIPickerButton>().Menu = this;
            menuItems.Add(menuItem);
        }
        public void NextPage()
        {
            if ((currentPageNumber * itemsPerPage) < ServerItems.Count) { 
                for (var j = menuItems.Count; j > 0; j--)
                {
                    Destroy(menuItems[j - 1]);
                    menuItems.Remove(menuItems[j - 1]);
                }
            for (var i = currentPageNumber * itemsPerPage; i < ServerItems.Count && i < ((currentPageNumber * itemsPerPage) + itemsPerPage); i++)
            {
                //Debug.Log(ServerItems[i].name);
                addItemToMenu(ServerItems[i].name, i);
            }

            currentPageNumber++;
            }
        }
        public void PrevPage()
        {
            if (currentPageNumber - 1 != 0)
            {
                for (var j = menuItems.Count; j > 0; j--)
                {
                    Destroy(menuItems[j - 1]);
                    menuItems.Remove(menuItems[j - 1]);
                }

                for (var i = (currentPageNumber - 2) * itemsPerPage; i < ServerItems.Count && i < (((currentPageNumber - 2) * itemsPerPage) + itemsPerPage); i++)
                {
                    //Debug.Log(ServerItems[i].name);
                    addItemToMenu(ServerItems[i].name, i);
                }
                currentPageNumber--;
            }
        }

        public void skipItem()
        {
            if (currentStandardDataObject < standardObjectDatas.Length - 1)
            {
                currentStandardDataObject++;
                NewItem();
            }
            else
            {
                EmptyMenu();
            }
        }
        public void PickItem(string number)
        {
            standardObjectDatas[currentStandardDataObject].data.dataUrl = ServerItems[int.Parse(number)].url;
            standardObjectDatas[currentStandardDataObject].loadAsset(true);
            if (currentStandardDataObject < standardObjectDatas.Length -1) {
                currentStandardDataObject++;
                NewItem();
            }
            else
            {
                EmptyMenu();
            }
        }
        public void NewItem()
        {
            currentPageNumber = 0;
            if (standardObjectDatas[currentStandardDataObject].type == currentType)
            {
                NextPage();
            }
            else
            {
                StartCoroutine(DownloadMenuItems(standardObjectDatas[currentStandardDataObject].type));
            }
        }
        public void EmptyMenu() {
           
            currentPageNumber = 0;
            for (var j = menuItems.Count; j > 0; j--)
            {
                Destroy(menuItems[j - 1]);
                menuItems.Remove(menuItems[j - 1]);
            }
            ObjectToFill = null;
            UIProperties.ActiveObject = null;
            gameObject.SetActive(false);

        }

        IEnumerator DownloadMenuItems(string type)
        {
            Debug.Log("startdownload");
            UnityWebRequest webRequest = UnityWebRequest.Get(UIProperties.serverLink + "/assets/" + type);
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
                currentType = type;
                AssetDownload download = new AssetDownload();
                download = JsonUtility.FromJson<AssetDownload>(webRequest.downloadHandler.text);
                ServerItems = download.assets;
                NextPage();
            }   
        }
    }
    [System.Serializable]
    public class AssetDownload
    {
        public bool success;
        public int amount;
        public List<AssetDownloadWrapper> assets;
    }
    [System.Serializable]
    public class AssetDownloadWrapper
    {
        public string name;
        public string url;  
    }
}

