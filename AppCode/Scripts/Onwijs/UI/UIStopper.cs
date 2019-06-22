using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
namespace Onwijs.UI
{
    public class UIStopper : MonoBehaviour, IFocusable
    {
        public void OnFocusEnter()
        {
            if (!UIProperties.UiPositionHelper.lookingAtChilderen.Contains(this))
            {
                UIProperties.UiPositionHelper.lookingAtChilderen.Add(this);
            }
        }
        public void OnFocusExit()
        {
            UIProperties.UiPositionHelper.lookingAtChilderen.Remove(this);
        }
    }
}
