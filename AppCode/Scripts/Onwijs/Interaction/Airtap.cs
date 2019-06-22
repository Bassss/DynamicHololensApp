using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
namespace Onwijs.Interaction
{
    [RequireComponent(typeof(Collider))]
    public class Airtap : MonoBehaviour, IInputClickHandler, IInputHandler
    {
        public void OnInputClicked(InputClickedEventData eventData)
        {
            //AirTap detected
            click();
        }
        public virtual void click()
        {
            Debug.Log("I'm clicking on " + gameObject.name);
        }
        public void OnInputDown(InputEventData eventData)
        {
        }
        public void OnInputUp(InputEventData eventData)
        {
        }
    }
}
