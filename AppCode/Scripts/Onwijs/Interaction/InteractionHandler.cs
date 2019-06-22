using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onwijs.Enums;
using Onwijs.UI;

namespace Onwijs.Interaction
{
    [RequireComponent(typeof(Collider))]
    //[RequireComponent(typeof(MoveWithPull))]
    //[RequireComponent(typeof(ScaleInteraction))]
   // [RequireComponent(typeof(RotateInteraction))]
    public class InteractionHandler : Gaze
    {
        public GameObject optionsObject;
        public GameObject precisionButtons;
        public ObjectState objectState = ObjectState.move;

        private MoveWithPull moveWithPull;
        private ScaleInteraction scaleInteraction;
        private RotateInteraction rotateInteraction;
        private HoverButton[] buttons;
        private Vector3 optionsScale;
        private Vector3 buttonScale;

        private void Start()
        {
            optionsScale = optionsObject.transform.localScale;
            buttonScale = precisionButtons.transform.localScale;
            moveWithPull = GetComponent<MoveWithPull>();
            scaleInteraction = GetComponent<ScaleInteraction>();
            scaleInteraction.enabled = false;
            rotateInteraction = GetComponent<RotateInteraction>();
            rotateInteraction.enabled = false;
            optionsObject.transform.localPosition = new Vector3(0, 0, 0);
        }
        public override void OnFocusEnter()
        {
            if (UIProperties.ActiveObject != gameObject && !UIProperties.playmode && !UIProperties.previewmode)
            {
                openOptions();
                showPrecisionButtons();
            }
        }
       
        public void showPrecisionButtons()
        {
            precisionButtons.transform.parent = transform;
            if (precisionButtons.transform.parent == transform)
            {
                precisionButtons.transform.localScale = buttonScale;
            }
            precisionButtons.transform.localPosition = new Vector3(0, 0, 0);
            precisionButtons.SetActive(true);
            precisionButtons.transform.parent = transform.parent;
        }

            public void openOptions()
        {
            optionsObject.transform.parent = transform;
            if (optionsObject.transform.parent == transform)
            {
                optionsObject.transform.localScale = optionsScale;
            }
           

            optionsObject.transform.localPosition = new Vector3(0, 0, 0);
            optionsObject.SetActive(true);
            optionsObject.transform.parent = transform.parent;
        }
        public void closeMenus()
        {
            optionsObject.SetActive(false);
            precisionButtons.SetActive(false);
        }
        // code to change state
        public void ChangeState(string newState) // must be a string in order for UnityEvent to understand 
        {
            ObjectState parsedState = (ObjectState)System.Enum.Parse(typeof(ObjectState), newState);
            if (parsedState == objectState)
            {
                parsedState = ObjectState.move;
            }
            objectState = parsedState;
            setVisual();
            HandleState();
        }
        // code to set visual
        public void setVisual()
        {
               
        }

        // code to handle state 
        public void HandleState()
        {
            // code to handle interaction
            moveWithPull.enabled = false;
            scaleInteraction.enabled = false;
            rotateInteraction.enabled = false;

            switch (objectState)
            {
                case (ObjectState.rotate):
                    rotateInteraction.enabled = true;
                    break;
                case (ObjectState.scale):
                    scaleInteraction.enabled = true;
                    break;
                default:
                    moveWithPull.enabled = true;
                    break;
            }
        }

    }
}
