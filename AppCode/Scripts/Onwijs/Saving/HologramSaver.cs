using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onwijs.Enums;
using Onwijs.StandardData;
namespace Onwijs.Saving
{
    [RequireComponent(typeof(standardDataHelper))]
    public class HologramSaver : MonoBehaviour
    {
        // object identifiers 
        public SaveData originalSaveData;
        public ObjectType type;
        public GameObject anchor;
        private List<GameObject> collidingAnchors = new List<GameObject>();
        public GameObject AnchorPrefab;
        public void Save()
        {
            setClosestAnchor();
            if (!originalSaveData.newSave)
            {
                SaveData data = SaveHandler.saveDataWrapper.saveData[originalSaveData.id - 1];
                data.unityLocation = transform.position;
                data.localLocation = transform.localPosition;
                data.localRotation = transform.localRotation;
                data.localScale = transform.localScale;
                data.standardObjectData = GetComponent<standardDataHelper>().GetStandardData();
                SaveHandler.Save();
            }
            else
            {
                int data = SaveHandler.getFirstEmptyHologramSpot();
                SaveHandler.saveDataWrapper.saveData[data].name = gameObject.name;
                SaveHandler.saveDataWrapper.saveData[data].unityLocation = transform.position;
                SaveHandler.saveDataWrapper.saveData[data].localLocation = transform.localPosition;
                SaveHandler.saveDataWrapper.saveData[data].localRotation = transform.localRotation;
                SaveHandler.saveDataWrapper.saveData[data].localScale = transform.localScale;
                SaveHandler.saveDataWrapper.saveData[data].anchorId = anchor.GetComponent<AnchorHelper>().anchorData.id;
                SaveHandler.saveDataWrapper.saveData[data].standardObjectData = GetComponent<standardDataHelper>().GetStandardData();
                SaveHandler.saveDataWrapper.saveData[data].type = type.ToString();
                SaveHandler.saveDataWrapper.saveData[data].newSave = false;
                SaveHandler.saveDataWrapper.saveData[data].deleted = false;
                originalSaveData = SaveHandler.saveDataWrapper.saveData[data];
                SaveHandler.Save();
            }
        }
        public void ChangeAnchor(GameObject newAnchor, bool exit)
        {
            if (exit)
            {
                if (collidingAnchors.Contains(newAnchor)){
                    collidingAnchors.Remove(newAnchor);
                }
            }
            else
            {
                if (!collidingAnchors.Contains(newAnchor))
                {
                    collidingAnchors.Add(newAnchor);
                }
            }
            if(collidingAnchors.Count < 1)
            {
                anchor = null;
            }
            else
            {
                setClosestAnchor();
            }
        }
        private void setClosestAnchor()
        {
            if (collidingAnchors.Count == 0)
            {
                if (anchor == null)
                {
                    Debug.Log("Cant save an object without an anchor as parent, creating anchor");
                    // make an anchor code
                    GameObject cube = Instantiate(AnchorPrefab);
                    cube.transform.position = transform.position;
                    cube.GetComponent<AnchorHelper>().checkForChilderen();
                    cube.GetComponent<AnchorHelper>().placeAnchor();
                    anchor = cube;
                    transform.parent = anchor.transform;
                }
            }
            GameObject closestAnchor = anchor;
            float closestDistance = 10f;
            for (var i = 0; i < collidingAnchors.Count; i++)
            {

                float distance = Vector3.Distance(collidingAnchors[i].transform.position, transform.position);
              
                if (distance < closestDistance)
                {
                    closestAnchor = collidingAnchors[i];
                    closestDistance = distance;
                }
            }
            originalSaveData.anchorId = int.Parse(closestAnchor.name);
            anchor = closestAnchor;
        }
        public void Edit()
        {
            GetComponent<standardDataHelper>().editMode();
        }
        public void Remove()
        {
            // cant delete an entry out of the list without messing op id system 
            SaveHandler.saveDataWrapper.saveData[originalSaveData.id - 1].deleted = true;
            SaveHandler.Save();
            Destroy(gameObject);
        }
        public void setData(SaveData data)
        {
            originalSaveData = data;
            GetComponent<standardDataHelper>().activateChilderen(data.standardObjectData);
        }
    }
}
