using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using Onwijs.Saving;

namespace Onwijs.Interaction
{
    public class ScaleInteraction : MonoBehaviour, INavigationHandler
    {
        public float ScaleSensitivity = 0.1f;
        private Vector3 manipulationPreviousScale;
        private float scaleFactor;
        private Collider SphereCollider;
        public float scaleLimit = 0.5f;
        public float ScaleFactorX = 1;
        public float ScaleFactorY = 1;
        public float ScaleFactorZ = 1;

        public void OnNavigationUpdated(NavigationEventData eventData)
        {
            if (!UIProperties.playmode && !UIProperties.previewmode)
            {
                scaleFactor = eventData.NormalizedOffset.x;

                if (GetComponent<Renderer>().bounds.size.y < scaleLimit || scaleFactor < 0)
                {
                    transform.localScale = new Vector3(manipulationPreviousScale.x + (scaleFactor * ScaleFactorX), manipulationPreviousScale.y + (scaleFactor * ScaleFactorY), manipulationPreviousScale.z + (scaleFactor * ScaleFactorZ));
                }
                GetComponent<HologramSaver>().Save();
            }
        }
        public void OnNavigationStarted(NavigationEventData eventData)
        {
            manipulationPreviousScale = transform.localScale;
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
