using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

namespace Onwijs.Interaction
{
    public class Touch : MonoBehaviour
    {
        private IFocusable touchingObject;
        public Transform VisualR;
        public Transform VisualL;
        public Color touchColor;
        private void Start()
        {
           if( Camera.main.transform.position.x > transform.position.x)
            {
                VisualR.gameObject.SetActive(false);
            }
            else
            {
                VisualL.gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.forward), out hit, Mathf.Infinity))
            {
                VisualR.localPosition = new Vector3(-0.07f, -0.45f, transform.InverseTransformPoint(hit.point).z + 0.5f);
                VisualL.localPosition = new Vector3(0.07f, -0.45f, transform.InverseTransformPoint(hit.point).z + 0.5f);
                if (hit.collider.gameObject.GetComponent<IFocusable>() != null)
                {
                    if (touchingObject == null)
                    {
                        Debug.Log("entering");
                        touchingObject = hit.collider.gameObject.GetComponent<IFocusable>();
                        hit.collider.gameObject.GetComponent<IFocusable>().OnFocusEnter();
                        VisualR.GetComponent<SpriteRenderer>().color = touchColor;
                        VisualL.GetComponent<SpriteRenderer>().color = touchColor;
                    }
                    else if (touchingObject != hit.collider.gameObject.GetComponent<IFocusable>())
                    {
                        Debug.Log("exiting");
                        touchingObject.OnFocusExit();
                        touchingObject = hit.collider.gameObject.GetComponent<IFocusable>();
                        hit.collider.gameObject.GetComponent<IFocusable>().OnFocusEnter();
                        VisualR.GetComponent<SpriteRenderer>().color = touchColor;
                        VisualL.GetComponent<SpriteRenderer>().color = touchColor;
                    }
                }
                else
                {
                    if (touchingObject != null)
                    {
                        Debug.Log("exiting");
                        touchingObject.OnFocusExit();
                        touchingObject = null;
                        VisualR.GetComponent<SpriteRenderer>().color = Color.white;
                        VisualL.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                }
            }
            else
            {
                if (touchingObject != null)
                {
                    Debug.Log("exiting");
                    touchingObject.OnFocusExit();
                    touchingObject = null;
                }
                VisualR.localPosition = new Vector3(-0.07f, -0.45f, 0);
                VisualL.localPosition = new Vector3(0.07f, -0.45f, 0);
                VisualR.GetComponent<SpriteRenderer>().color = Color.white;
                VisualL.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }
}
