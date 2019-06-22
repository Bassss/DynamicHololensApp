using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Onwijs.EventSystem
{
    public class GameEventRaiser : MonoBehaviour
    {
        public GameEvent eventToRaise;
       
        public void RaiseEvent()
        {
            if (eventToRaise == null)
            {
                Debug.Log("Event was not set for Event Raiser on GameObject named:" + gameObject.name);
                return;
            }
            eventToRaise.AddRaiser(gameObject);
            eventToRaise.Raise();
        }
    }
}

