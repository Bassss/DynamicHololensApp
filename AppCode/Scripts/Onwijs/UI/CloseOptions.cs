using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onwijs.Interaction;
namespace Onwijs.UI
{
    public class CloseOptions : Gaze
    {
        public Transform Parent;
       // public GameObject precisionButtons;
        public override void OnFocusEnter()
        {
            if (UIProperties.ActiveObject != Parent.gameObject)
            {
                gameObject.SetActive(false);
                //precisionButtons.SetActive(false);
                transform.parent = Parent;
               // precisionButtons.transform.parent = Parent;
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
