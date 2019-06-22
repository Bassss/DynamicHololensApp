using UnityEngine;
namespace Onwijs.EventSystem
{
    [CreateAssetMenu(menuName = "Event/Basic GameEvent", order = 0)]
    public class BasicGameEvent : GameEvent
    {
        public override void Raise()
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised();
            }
        }

    }
}