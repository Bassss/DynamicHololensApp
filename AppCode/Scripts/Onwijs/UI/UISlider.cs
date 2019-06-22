using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.Events;
namespace Onwijs.UI
{
    public class UISlider : MonoBehaviour, INavigationHandler, IFocusable
    {
        public float min = -0.4f;
        public float max = 0.4f;
        public float CurrentValue = 0;
        private float lastValue = 0;
        public UnityEvent FunctionToDo;

        private Transform SliderPointer;

        private void Start()
        {
            SliderPointer = transform.GetChild(0);
        }

        public void OnFocusEnter()
        {
            SliderPointer.localScale = new Vector3(0.133f, 0.8f, 2f);
        }
        public void OnFocusExit()
        {
            SliderPointer.localScale = new Vector3(0.1f, 0.6f, 2f);
        }

        public void OnNavigationUpdated(NavigationEventData eventData)
        {
            // Check rotation to see on what axis the gesture needs to be measured
            float Rotation = UIProperties.UiPositionHelper.transform.eulerAngles.y;
  

            float uncalculatedValue;

            if ( Rotation > 45 && Rotation < 135)
            {
                uncalculatedValue = (eventData.NormalizedOffset.x* 1f);
                CurrentValue = lastValue + ((uncalculatedValue * -1) / 0.008f);
            }
            else if (Rotation > 255 && Rotation <315)
            {
                uncalculatedValue = (eventData.NormalizedOffset.x * 1f);
                CurrentValue = lastValue + (uncalculatedValue / 0.008f);
            }
            else if (Rotation > 135 && Rotation < 225)
            {
                uncalculatedValue = (eventData.NormalizedOffset.x * 1f);
                CurrentValue = lastValue + ((uncalculatedValue *-1) / 0.008f);
            }
            else
            {
                uncalculatedValue = (eventData.NormalizedOffset.x * 1f);
                CurrentValue = lastValue + ((uncalculatedValue) / 0.008f);
            }
   

            SliderPointer.localPosition = new Vector3(percentageToPosition(), SliderPointer.localPosition.y, SliderPointer.localPosition.z);
            FunctionToDo.Invoke();
        }
        public void OnNavigationStarted(NavigationEventData eventData)
        {
            lastValue = CurrentValue;
        }

        public void OnNavigationCompleted(NavigationEventData eventData)
        {
        }

        public void OnNavigationCanceled(NavigationEventData eventData)
        {
        }
        private float percentageToPosition()
        {
            float value = 0;
            value = -0.4f + (0.008f * CurrentValue);
            if (CurrentValue >= 100)
            {
                value = 0.4f;
                CurrentValue = 100;
            }

            if (CurrentValue <= 0)
            {
                value = -0.4f;
                CurrentValue = 0;
            }
           
            return value;
        }
    }
}
