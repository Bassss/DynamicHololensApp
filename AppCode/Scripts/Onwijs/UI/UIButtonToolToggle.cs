using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onwijs.EventSystem;
namespace Onwijs.UI
{
public class UIButtonToolToggle : UIButton
{
        public GameEvent TurnOnEvent;
        public GameEvent TurnOffEvent;
        public override void Start()
        {
            disabled = !enableOverride;
            base.Start();
        }
        public override void buttonPress()
        {
            FunctionToDo.Invoke();
            isToggle = true;
            if (!toggeledOn)
            {
                if(TurnOffEvent != null)
                {
                    TurnOnEvent.Raise();
                }
            }
            else
            {
                if (TurnOnEvent != null)
                {
                    TurnOffEvent.Raise();
                }
            }
        }
    }
}
