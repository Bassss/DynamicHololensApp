using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using Onwijs.Saving;

namespace Onwijs.Interaction
{
    public class RotateInteraction : MonoBehaviour, INavigationHandler
    {
        public float RotationSensitivity = 10.0f;
        private Vector3 manipulationPreviousPosition;
        private float rotationFactorX;
        private float rotationFactorY;
        private float rotationFactorZ;
        private Collider SphereCollider;
        public void OnNavigationUpdated(NavigationEventData eventData)
        {
            if (!UIProperties.playmode && !UIProperties.previewmode)
            {
                rotationFactorX = eventData.NormalizedOffset.x * RotationSensitivity;
                rotationFactorY = eventData.NormalizedOffset.y * RotationSensitivity;
                rotationFactorZ = eventData.NormalizedOffset.z * RotationSensitivity;

                if (Mathf.Abs(rotationFactorX) > Mathf.Abs(rotationFactorY) && Mathf.Abs(rotationFactorX) > Mathf.Abs(rotationFactorZ))
                {
                    transform.RotateAround(transform.parent.position, transform.parent.up, (Mathf.Round(rotationFactorX*10)) * Time.deltaTime);
                }
                else if (Mathf.Abs(rotationFactorY) > Mathf.Abs(rotationFactorX) && Mathf.Abs(rotationFactorY) > Mathf.Abs(rotationFactorZ))
                {
                    transform.RotateAround(transform.parent.position, transform.parent.right, -(Mathf.Round(rotationFactorY*10)) * Time.deltaTime);
                }
                else if (Mathf.Abs(rotationFactorZ) > Mathf.Abs(rotationFactorX) && Mathf.Abs(rotationFactorZ) > Mathf.Abs(rotationFactorY))
                {
                    transform.RotateAround(transform.parent.position, transform.parent.forward, (Mathf.Round(rotationFactorZ*10)) * Time.deltaTime);
                }
            }
        }

        public void OnNavigationStarted(NavigationEventData eventData)
        {
            SphereCollider = gameObject.AddComponent<SphereCollider>();

        }

        public void OnNavigationCompleted(NavigationEventData eventData)
        {
            Destroy(SphereCollider);
            GetComponent<HologramSaver>().Save();
        }

        public void OnNavigationCanceled(NavigationEventData eventData)
        {
            Destroy(SphereCollider);
            GetComponent<HologramSaver>().Save();
        }
    }
}
