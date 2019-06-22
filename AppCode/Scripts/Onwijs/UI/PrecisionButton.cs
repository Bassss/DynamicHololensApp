using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onwijs.Enums;
using Onwijs.Interaction;
using HoloToolkit.Unity.InputModule;

namespace Onwijs.UI
{
    public class PrecisionButton : MonoBehaviour, IFocusable, IInputClickHandler, IInputHandler
    {
        private MeshFilter meshFilter;
        public GameObject visual;
        public GameObject Hologram;
        public float moveFactor = 0.01f;
        public float scaleFactor = 0.01f;
        public float rotateFactor = 5f;
        private ObjectState currentState = ObjectState.move;
        private bool hold = false;
        public bool right = false;
        public bool up = false;

        public void OnInputDown(InputEventData eventData)
        {
            hold = true;
        }
        public void OnInputUp(InputEventData eventData)
        {
            hold = false;
        }
        public void OnInputClicked(InputClickedEventData eventData)
        {
            Click();
        }
        public void OnFocusEnter()
        {
            visual.GetComponent<Animation>().Play();
        }
        public void OnFocusExit()
        {
            visual.GetComponent<Animation>().Stop();
            visual.transform.localPosition = new Vector3(0, 0, 0);
        }
        public void Click()
        {
            currentState = Hologram.GetComponent<InteractionHandler>().objectState;
            if(currentState == ObjectState.move)
            {
                Hologram.transform.position += transform.forward * moveFactor ;

            }
            else if (currentState == ObjectState.rotate)
            {
                if (right)
                {
                    Hologram.transform.RotateAround(Hologram.transform.position, Hologram.transform.parent.right, (Mathf.Round(rotateFactor * 10)) * Time.deltaTime);
                }
                else if (up)
                {
                    Hologram.transform.RotateAround(Hologram.transform.position, Hologram.transform.parent.up, (Mathf.Round(rotateFactor * 10)) * Time.deltaTime);
                }
                else
                {
                    Hologram.transform.RotateAround(Hologram.transform.position, Hologram.transform.parent.forward, (Mathf.Round(rotateFactor * 10)) * Time.deltaTime);
                }
            }
            else if (currentState == ObjectState.scale)
            {
               Hologram.transform.localScale = Hologram.transform.localScale + new Vector3(scaleFactor, scaleFactor, scaleFactor);
            }
        }
    }
}
 