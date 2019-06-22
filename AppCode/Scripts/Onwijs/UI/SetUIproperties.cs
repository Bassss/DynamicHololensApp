using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onwijs.EventSystem;
namespace Onwijs.UI
{
    public class SetUIproperties : MonoBehaviour
    {
        public bool hasPreviewArea = false;
        public GameObject previewArea;
        public bool hasInfoScreen = false;
        public GameObject infoScreen;
        public bool hasPrefabPanel = true;
        public bool canBeOperatedByhand = true;
        public bool canBeOperatedByGaze = true;
        public bool rotatesWithUser = true;
        public string serverLink = "http://172.20.10.2:8001/api/v1/";
        public GameEvent newActiveEvent;
        public  GameEvent noActiveEvent;

        private void Start()
        {
            UIProperties.hasPreviewArea = hasPreviewArea;
            UIProperties.previewArea = previewArea;
            UIProperties.hasInfoScreen = hasInfoScreen;
            UIProperties.infoScreen = infoScreen;
            UIProperties.newActiveEvent = newActiveEvent;
            UIProperties.noActiveEvent = noActiveEvent;
            UIProperties.hasPrefabPanel = hasPrefabPanel;
            UIProperties.canBeOperatedByhand = canBeOperatedByhand;
            UIProperties.canBeOperatedByGaze = canBeOperatedByGaze;
            UIProperties.rotatesWithUser = rotatesWithUser;
            UIProperties.UiPositionHelper = GetComponent<UiPositionHelper>();
            UIProperties.serverLink = serverLink;
        }
        public void setPreviewMode(bool Bool)
        {
            UIProperties.previewmode = Bool;
        }
    }
}
