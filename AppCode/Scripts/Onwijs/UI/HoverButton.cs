using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using Onwijs.Interaction;
using UnityEngine.Events;
namespace Onwijs.UI
{
    public class HoverButton : MonoBehaviour, IFocusable, IInputClickHandler, IInputHandler
    {
        private Transform visual;
        private Transform buttonObject;
        private float buttonZ = 0;
        public bool isToggle = false;
        protected bool toggledOn = false;
        public UnityEvent FunctionToDo;

        private void Start()
        {
            buttonObject = transform.GetChild(0);
            visual = transform.GetChild(1);
        }
        public virtual void buttonPress()
        {
            FunctionToDo.Invoke();
        }
        public void OnInputDown(InputEventData eventData)
        {
        }
        public void OnInputUp(InputEventData eventData)
        {
        }
           public void OnInputClicked(InputClickedEventData eventData)
        {
            Click();
        }
        private void Click()
        {
           
            buttonPress();
            if (!toggledOn)
            {
                StartCoroutine(buttonOnAnimation());
            }
            else
            {
                StartCoroutine(buttonOffAnimation());
            }
        }
        public void toggleHandler()
        {
            if (toggledOn)
            {
                StartCoroutine(buttonOffAnimation());
            }
        }
        public void OnFocusEnter()
        {
            visual.localPosition = new Vector3(visual.localPosition.x, visual.localPosition.y, 5.51f);
            if (!toggledOn)
            {
                visual.GetComponent<SpriteRenderer>().color = Color.gray;
            }
        }
        public void OnFocusExit()
        {
            visual.localPosition = new Vector3(visual.localPosition.x, visual.localPosition.y, 0.51f);
            if (!toggledOn)
            {
                visual.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
        IEnumerator buttonOnAnimation()
        {
            while (buttonZ > -2)
            {
                buttonZ = buttonZ - 0.2f;
                buttonObject.localPosition = new Vector3(buttonObject.localPosition.x, buttonObject.localPosition.y, buttonZ);
                yield return new WaitForFixedUpdate();
            }
            visual.GetComponent<SpriteRenderer>().color = Color.black;
            toggledOn = true;
            if (!isToggle)
            {
                StartCoroutine(buttonOffAnimation());
            }
        }
    
    IEnumerator buttonOffAnimation()
        {
            while (buttonZ < 0)
            {
                buttonZ = buttonZ + 0.1f;
                buttonObject.localPosition = new Vector3(buttonObject.localPosition.x, buttonObject.localPosition.y, buttonZ);
                yield return new WaitForFixedUpdate();
            }
            toggledOn = false;
            visual.GetComponent<SpriteRenderer>().color = Color.gray;
           
        }
    }
}
