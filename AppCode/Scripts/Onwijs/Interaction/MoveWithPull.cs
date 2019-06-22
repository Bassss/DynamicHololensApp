using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onwijs.Saving;
namespace Onwijs.Interaction
{
    [RequireComponent(typeof(Rigidbody))]
    public class MoveWithPull : Airtap
    {
        //needed objects
        private Rigidbody Rigidbody;
        private GameObject FocusPoint;

        //Pull variables
        public float targetVelocity = 40f;
        public float maxVelocity = 80f;
        public float gainPerSecond = 100f;
        public float maxPullForce = 20f;
        public float stopDistance = 1f;

        // other
        private float distance = 0f;
        private bool isChild = false;

        public void Start()
        {
            Rigidbody = GetComponent<Rigidbody>();
            FocusPoint = GameObject.Find("FocusPoint");
        }
        public override void click()
        {
            //AirTap detected
            if (!UIProperties.playmode && !UIProperties.previewmode)
            {
                if (PlayerPrefs.GetString("UIButtonDelete Button") == "True")
                {
                    GetComponent<HologramSaver>().Remove();
                    return;
                }
                Move();
            }
        }
        private void Move()
        {
            if (!isChild)
            {
                StartCoroutine(PullObject());
                UIProperties.ActiveObject = gameObject;
                UIProperties.activeObjectScale = gameObject.transform.localScale;
                UIProperties.newActiveEvent.Raise();
            }
            else
            {
                if (GetComponent<HologramSaver>().anchor != null)
                {
                    gameObject.transform.parent = GetComponent<HologramSaver>().anchor.transform;
                }
                else
                {
                    gameObject.transform.parent = null;
                }
                isChild = false;
                GetComponent<HologramSaver>().Save();
                UIProperties.ActiveObject = null;
            }
        }
        //Code to move object smoothly to point in front of camera
        IEnumerator PullObject()
        {
            while (FocusPoint == null)
            {
                yield return new WaitForFixedUpdate();
            }
            distance = Vector3.Distance(Rigidbody.transform.position, (FocusPoint.transform.position));
            while (distance > 0.01f)
            {
                distance = Vector3.Distance(Rigidbody.transform.position, (FocusPoint.transform.position));
                Rigidbody.isKinematic = false;
                if (true)
                {
                    if (distance < stopDistance)
                    {
                        // stronger forces to ensure a stop close to the targetpoint
                        Vector3 dist = ((FocusPoint.transform.position)) - Rigidbody.transform.position;
                        Vector3 tgtVel = Vector3.ClampMagnitude(10 * dist, 20);

                        Vector3 error = tgtVel - Rigidbody.velocity;
                        Vector3 force = Vector3.ClampMagnitude(80 * error, 100);

                        Rigidbody.AddForce(force);
                    }
                    else
                    {
                        // Calculate force based on variables 
                        Vector3 dist = ((FocusPoint.transform.position)) - Rigidbody.transform.position;

                        Vector3 tgtVel = Vector3.ClampMagnitude(targetVelocity * dist, maxVelocity);

                        Vector3 error = tgtVel - Rigidbody.velocity;
                        Vector3 force = Vector3.ClampMagnitude(gainPerSecond * error, maxPullForce);

                        Rigidbody.AddForce(force);
                    }
                }
                yield return new WaitForFixedUpdate();
            }
            // make object a child so it wil stay still during movement
            Rigidbody.isKinematic = true;
            gameObject.transform.parent = FocusPoint.transform;
            isChild = true;
        }
    }
}
