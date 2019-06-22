using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using Onwijs.Interaction;
namespace Onwijs.Saving
{   
    // require sphere collider to able to detect objects to make childeren
    [RequireComponent(typeof(SphereCollider))]
    public class AnchorHelper : Airtap
    {
        //needed objects
        private Rigidbody Rigidbody;
        private GameObject FocusPoint;
        private SphereCollider SphereCollider;
        private WorldAnchorManager worldAnchorManager;

        //Pull variables
        public float targetVelocity = 40f;
        public float maxVelocity = 80f;
        public float gainPerSecond = 100f;
        public float maxPullForce = 20f;
        public float stopDistance = 1f;

        //Anchor variables
        public float ChildSearchRadius = 1f;
        public GameObject DebugObject;

        //AnchorSave variables
        public AnchorData anchorData;

        // other
        private float distance = 0f;
        private bool isPlaced = false;

        public void Start()
        {
            SphereCollider = GetComponent<SphereCollider>();
            worldAnchorManager = GameObject.Find("AnchorManager").GetComponent<WorldAnchorManager>();
        }
        public override void click()
        {
            //AirTap detected
            //Debug.Log("I'm clicking on " + gameObject.name);
            if (!isPlaced)
            {
                Rigidbody = GetComponent<Rigidbody>();
                FocusPoint = GameObject.Find("FocusPoint");
                StartCoroutine(PullObject());
            }
            else
            {
                gameObject.transform.parent = null;
                checkForChilderen();
                placeAnchor();

            }
        }
        public void checkForChilderen()
        {
            //expand collider to trigger with objects that are close;
            GetComponent<SphereCollider>().radius = ChildSearchRadius;

        }
        public void deleteIfEmpty()
        {
            if(transform.childCount <= 1)
            {
                anchorData.deleted = true;
                SaveHandler.allAnchors.Remove(gameObject);
                SaveHandler.deletedAnchors.Add(anchorData.id);
                GameObject.Find("AnchorManager").GetComponent<WorldAnchorManager>().RemoveAnchor(gameObject);
                SaveHandler.Save();
                Destroy(gameObject);
            }
        }
        public void placeAnchor()
        {
            isPlaced = true;
            // destroy rigidbody because it blocks the world anchor component
            Rigidbody = GetComponent<Rigidbody>();
            GetComponent<BoxCollider>().enabled = false;
            Destroy(Rigidbody);
            if(PlayerPrefs.GetString("UIButtonDebug Anchors button") == "False")
            {
                 DebugObject.SetActive(false);
            }
           
            saveAnchor();
        }
        private void saveAnchor()
        {
            int data = SaveHandler.getFirstEmptyAnchorSpot();
            name = SaveHandler.saveDataWrapper.anchorData[data].id+"";
            SaveHandler.saveDataWrapper.anchorData[data].SpatialAnchorID = name;
            SaveHandler.saveDataWrapper.anchorData[data].unityLocation = transform.position;
            SaveHandler.allAnchors.Add(gameObject);
            SaveHandler.Save();
            anchorData = SaveHandler.saveDataWrapper.anchorData[data];
            if(worldAnchorManager == null)
            {
                worldAnchorManager = GameObject.Find("AnchorManager").GetComponent<WorldAnchorManager>();
            }
             // add the anchor component
            #if !UNITY_EDITOR
                worldAnchorManager.AttachAnchor(gameObject, name);
            #endif
        }
        private void OnTriggerEnter(Collider other)
        {
            // add anchor to the object as reference
            if (other.tag == "Hologram")
            {
                other.GetComponent<HologramSaver>().ChangeAnchor(gameObject, false);
            }

        }
        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Hologram")
            {
                other.GetComponent<HologramSaver>().ChangeAnchor(gameObject, true);
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
            isPlaced = true;
        }
    }
}
