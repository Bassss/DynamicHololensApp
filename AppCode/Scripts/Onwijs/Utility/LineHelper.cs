using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onwijs.Utility
{
    public class LineHelper : MonoBehaviour
    {
        public bool isActive = false;
        public GameObject UnderScoreObject;
      
        private LineRenderer line;
        void Start()
        {
            line = GetComponent<LineRenderer>();
            Vector3[] Vectors = new Vector3[3];
            Vectors[0] = transform.localPosition;
            Vectors[1] = UnderScoreObject.transform.localPosition;
            Bounds bounds = UnderScoreObject.transform.GetChild(0).GetComponent<MeshRenderer>().bounds;
            Vectors[2] = new Vector3(transform.InverseTransformPoint(bounds.max).x * 2, Vectors[1].y, Vectors[1].z);
            line.SetPositions(Vectors);
        }
    }
}
