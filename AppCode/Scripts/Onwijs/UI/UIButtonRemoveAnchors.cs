using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onwijs.Saving;
namespace Onwijs.UI
{
    public class UIButtonRemoveAnchors : UIButton
    {
        public override void Start()
        {
            disabled = false;
            base.Start();
        }
        public override void buttonPress()
        {
            SaveHandler.EmptySave();
        }
    }
}
