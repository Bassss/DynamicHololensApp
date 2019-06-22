using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

namespace Onwijs.UI
{
    public class UiPositionHelper : MonoBehaviour, IFocusable
    {
        private Quaternion iniRot;
        private bool lockRotation = false;
        private bool lookingAtPanel = false;
        private bool moving = false;
        public List<IFocusable> lookingAtChilderen = new List<IFocusable>();
        public GameObject User;
        public float lockTime = 2f;
        private Vector3 oldPos;
        public float movingTreshold = 0.01f;
       
        void Start()
        {
            iniRot = transform.rotation;
            oldPos = User.transform.position;
        }
        public void OnFocusEnter()
        {
            lookingAtPanel = true;
            lockRotation = true;
        }
        public void OnFocusExit()
        {
            lookingAtPanel = false;
            StartCoroutine(UnlockUI(Time.realtimeSinceStartup));
        }
        private void Update()
        {
            if((oldPos.x - movingTreshold < User.transform.position.x && oldPos.x + movingTreshold > User.transform.position.x) && 
               (oldPos.y - movingTreshold < User.transform.position.y && oldPos.y + movingTreshold > User.transform.position.y) &&
               (oldPos.z - movingTreshold < User.transform.position.z && oldPos.z + movingTreshold > User.transform.position.z))
            {
                moving = false;
            }
            else
            {
                moving = true;
                StartCoroutine(UnlockUI(Time.realtimeSinceStartup));
            }
            oldPos = User.transform.position;
        }

        void LateUpdate()
        {

            transform.position = Camera.main.transform.position;
            if(!lockRotation)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, Mathf.DeltaAngle(0, User.transform.localEulerAngles.y), transform.rotation.z);
            }
        }
        IEnumerator UnlockUI(float startTime)
        {
            while(startTime + lockTime > Time.realtimeSinceStartup && !moving)
            {
                 yield return new WaitForFixedUpdate();
            }
            if (lookingAtChilderen.Count == 0 && !lookingAtPanel && moving)
            {
                lockRotation = false;
            }
        }
    }
}
