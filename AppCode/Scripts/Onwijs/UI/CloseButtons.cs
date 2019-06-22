using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onwijs.Interaction;
namespace Onwijs.UI
{
    public class CloseButtons : Gaze
    {
        public Transform Parent;
        // public GameObject precisionButtons;
        public override void OnFocusEnter()
        {
            if (UIProperties.ActiveObject != Parent.gameObject)
            {
                transform.parent.gameObject.SetActive(false);
                transform.parent.transform.parent = Parent;
                transform.parent.transform.localRotation =new Quaternion(0, 0, 0, 0);
    
            }
        }
        public override void OnFocusExit()
        {

        }
        public void setParentAsActiveObject()
        {
            UIProperties.ActiveObject = Parent.gameObject;
        }
    }
}
