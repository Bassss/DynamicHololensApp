using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onwijs.Saving;
namespace Onwijs.StandardData
{
    public class standardDataHelper : MonoBehaviour
    {
        public standardDataObject[] childeren;
        private bool editmode = false;
        public void Start()
        {
            childeren = GetComponentsInChildren<standardDataObject>();
        }
        public standardDataObject[] getChilderen()
        {
            childeren = GetComponentsInChildren<standardDataObject>();
            return childeren;
        }

        public void activateChilderen(List<standardObjectData> data)
        {
            childeren = GetComponentsInChildren<standardDataObject>();
            for (int i = 0; i < childeren.Length; i++)
			{
                if( data.Count > childeren[i].localIdentifier)
                {
                    childeren[i].data = data[childeren[i].localIdentifier];
                    childeren[i].loadAsset();
                }
                else
                {
                    Debug.Log("No data found for "+childeren[i].type+" with local identifier "+ childeren[i].localIdentifier);
                }
			}
        }
        public void editMode()
        {
            editmode = !editmode;
            for (int i = 0; i < childeren.Length; i++)
            {
                childeren[i].edit(editmode);
            }
        }
        public void setStandardData(int number, standardObjectData data)
        {
            childeren[number].data = data;
            childeren[number].loadAsset();
        }
        public List<standardObjectData> GetStandardData()
        {
            List<standardObjectData> data = new List<standardObjectData>();
            childeren = GetComponentsInChildren<standardDataObject>();
            for (int i = 0; i < childeren.Length; i++)
            {
                childeren[i].data.localIdentifier = childeren[i].localIdentifier;
                childeren[i].data.type = childeren[i].type;
                data.Add(childeren[i].data);
            }
            return data;
        }
    
    }
}

