using System;
using UnityEngine;
using Onwijs.Saving;
using Onwijs.EventSystem;

namespace Onwijs.Utility
{
    public class QRScanComponent : MonoBehaviour
    {
        public SaveHelper saveHelper;
        public GameEvent playModeOn;
        public GameEvent playModeOff;

        void OnApplicationFocus(bool focus)
        {
            if (focus)
            {
                Debug.Log("onFocus");
                activate();
            }
        }
        public void activate()
        {
            StartCoroutine(QRScanner.QRReaderCoroutine(s => ParseMessage(s), false));
        }
        private void ParseMessage(string QRData)
        {
            Debug.Log(QRData);
            string[] words = QRData.Split(' ');
            bool playmode = false;
            string mode = "";
            if (words.Length > 2)
            {
                Debug.Log("QR-code data is to long ");

            }
            else if (words.Length == 2)
            {
                playmode = bool.Parse(words[0]);
                if (words[1] != "default")
                {
                    mode = words[1];
                }
                saveHelper.LoadOtherSaveFile(mode);

            }
            else if (words.Length == 1)
            {
                mode = words[0];
                saveHelper.LoadOtherSaveFile(mode);
            }
            else
            {
                Debug.Log("QR-code data is to short ");
            }
            if (playmode)
            {
                playModeOn.Raise();
                UIProperties.playmode = true;
            }
            else
            {
                playModeOff.Raise();
                UIProperties.playmode = false;
            }
        }
    }
}
