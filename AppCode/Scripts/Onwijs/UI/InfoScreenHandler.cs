using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
namespace Onwijs.UI
{
    public class InfoScreenHandler : MonoBehaviour, IFocusable
    {
        public void openScreen(float number)
        {
            StartCoroutine(OnAnimation(number));
        }
        public void CloseScreen(float number)
        {
            StartCoroutine(OffAnimation(number));
        }
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

        IEnumerator OnAnimation(float newY)
        {
            while (transform.localScale.y < newY)
            {
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + 0.05f, transform.localScale.z);
                yield return new WaitForFixedUpdate();
            }
        }
        IEnumerator OffAnimation(float newY)
        {
            while (transform.localScale.y > newY)
            {
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y - 0.05f, transform.localScale.z);
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
