using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onwijs.Saving;
namespace Onwijs.UI
{
    public class UIButtonDebugAnchorsToggel : UIButton
    {
        public override void Start()
        {
            disabled = false;
            base.Start();
        }
        public override void buttonPress()
        {
            isToggle = true;
            if (!toggeledOn)
            {
                for (int i = 0; i < SaveHandler.allAnchors.Count; i++)
                {
                    SaveHandler.allAnchors[i].GetComponent<AnchorHelper>().DebugObject.SetActive(true);
                }
            }
            else
            {
                for (int i = 0; i < SaveHandler.allAnchors.Count; i++)
                {
                    SaveHandler.allAnchors[i].GetComponent<AnchorHelper>().DebugObject.SetActive(false);
                }
            }
        }
        public void overridePress(bool on)
        {
            toggeledOn = !on;
            buttonPress();
        }
    }
}
