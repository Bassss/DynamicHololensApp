using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
namespace Onwijs.Interaction
{
     [RequireComponent(typeof(Collider))]
    public class Gaze : MonoBehaviour, IFocusable
    {
        //Ifocusable interface
        public virtual void OnFocusEnter()
        {   
            //object is looked at
           //Debug.Log("I'm looking at a " + gameObject.name);
        }
        public virtual void OnFocusExit()
        {
            //object is no longer looked at
          //  Debug.Log("I'm no longer looking at a " + gameObject.name);
        }
    }
}
