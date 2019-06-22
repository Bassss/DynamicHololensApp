using UnityEngine;
using UnityEngine.Events;

namespace Onwijs.EventSystem
{
    public class GameEventListener : MonoBehaviour
    {
        public GameEvent gameEvent;
        public UnityEvent basicResponse;

        private void OnEnable()
        {
            gameEvent.RegisterListener(this);
        }
        private void OnDisable()
        {
            gameEvent.UnregisterListener(this);
        }

        public void OnEventRaised()
        {
            basicResponse.Invoke();
        }
    }
}