using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.Events;
namespace Onwijs.UI
{
    public class UIButton : MonoBehaviour, IFocusable, IInputClickHandler, IInputHandler
    {
        protected UiPositionHelper baseUI;
        protected bool isToggle = false;
        protected bool toggeledOn = false;
        private float Yscale = 1f;
        protected bool disabled = true;
        public bool enableOverride = false;
        private Material enabledMat;
        public Material defaultMat;
        public UnityEvent FunctionToDo;

        public virtual void Start()
        {
            enabledMat = GetComponent<MeshRenderer>().materials[0];
            if (disabled)
            {
                Material[] data = new Material[1];
                data[0] = defaultMat;
                GetComponent<MeshRenderer>().materials = data;
            }
            baseUI = GameObject.Find("UI").GetComponent<UiPositionHelper>();
            if(PlayerPrefs.GetString("UIButton"+gameObject.name) == "True")
            {
                   Click();
            }
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
        public void Click()
        {
             buttonPress();
            Yscale = 0.5f;
            transform.localScale =  new Vector3( transform.localScale.x, Yscale, transform.localScale.z);
            if(!isToggle)
            {
                StartCoroutine(buttonPressAnimation());
            }
            else
            {
                if(toggeledOn)
                {
                     StartCoroutine(buttonPressAnimation());
                }
                toggeledOn = !toggeledOn;
                PlayerPrefs.SetString("UIButton"+gameObject.name, toggeledOn+"");
                Debug.Log("UIButton" + gameObject.name);
            }
        }
        public void OnFocusEnter()
        {
            if (!baseUI.lookingAtChilderen.Contains(this))
            {
                baseUI.lookingAtChilderen.Add(this);
            }

            transform.localScale = new Vector3( transform.localScale.x, Yscale + 0.2f, transform.localScale.z);
        }
        public void OnFocusExit()
        {
            baseUI.lookingAtChilderen.Remove(this);
            transform.localScale =  new Vector3( transform.localScale.x, Yscale, transform.localScale.z);
        }
        IEnumerator buttonPressAnimation()
        {
            while (Yscale < 1f)
            {
                Yscale = Yscale +0.05f;
                transform.localScale = new Vector3( transform.localScale.x, Yscale, transform.localScale.z);
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
