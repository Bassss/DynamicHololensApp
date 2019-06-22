using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onwijs.UI;
using Onwijs.EventSystem;

    public class UIProperties
    {
        public static bool hasPreviewArea = false;
        public static GameObject previewArea;
        public static bool hasInfoScreen = false;
        public static GameObject infoScreen;
        public static bool hasPrefabPanel = false;
        public static bool canBeOperatedByhand = false;
        public static bool canBeOperatedByGaze = true;
        public static bool rotatesWithUser = true;
        public static GameObject ActiveObject;
        public static GameEvent newActiveEvent;
        public static GameEvent noActiveEvent;
        public static Vector3 activeObjectScale;
        public static UiPositionHelper UiPositionHelper;
        public static string serverLink = "http://172.20.10.2:8001/api/v1/";
        public static string saveDataMode = "/";
        public static bool ServerError = false;
        public static bool playmode = false;
        public static bool previewmode = false;

    }

