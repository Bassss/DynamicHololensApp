using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Onwijs.EventSystem
{
    public class GameEvent : ScriptableObject
    {
        public List<GameObject> objectsWhoRaised =  new List<GameObject>();
        public List<GameObject> AllRaisers =  new List<GameObject>();
        protected List<GameEventListener> listeners = new List<GameEventListener>();
        public void RegisterListener(GameEventListener listener)
        {
            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);
            }
        }
        public void UnregisterListener(GameEventListener listener)
        {
            if (listeners.Contains(listener))
            {
                listeners.Remove(listener);
            }
        }
        protected void OnDisable()
        {
            listeners.Clear();
        }
        public virtual void Raise(){}
    
        public void AddRaiser(GameObject GameObject){
            if (!objectsWhoRaised.Contains(GameObject))
            {
                objectsWhoRaised.Add(GameObject);
                AllRaisers.Add(GameObject);
            }
        }
        public void RemoveRaiser(GameObject GameObject){
            if (objectsWhoRaised.Contains(GameObject)){
                objectsWhoRaised.Remove(GameObject);
            }
        }
    }
}