using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Onwijs.UI
{
    public class UIObjectMutation : MonoBehaviour
    {
        public UISlider rotationSlider;
        public UISlider scaleSlider;

        // func for rotate slider
        public void Rotate()
        {
            float rotation = rotationSlider.CurrentValue * 3.6f;
            UIProperties.ActiveObject.transform.rotation = Quaternion.Euler(transform.rotation.x, Mathf.DeltaAngle(0,rotation ), transform.rotation.z);

        }
        // func for scale slider 
        public void Scale()
        {
            float scale = 1;
            if (scaleSlider.CurrentValue == 50)
            {
                scale = 1;
            }
           else if (scaleSlider.CurrentValue == 0)
            {
                scale = 0.1f;
            }
            else if (scaleSlider.CurrentValue > 50)
           {
                scale = ((scaleSlider.CurrentValue - 50) *2)/ 10;
           }
           else if(scaleSlider.CurrentValue < 50)
           {
                scale = 0.01f * ((scaleSlider.CurrentValue * 2));
           }

            UIProperties.ActiveObject.transform.localScale = new Vector3(UIProperties.activeObjectScale.x *  scale, UIProperties.activeObjectScale.y * scale, UIProperties.activeObjectScale.z * scale);
        }
    }
}
