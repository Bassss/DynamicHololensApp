using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Onwijs.Interaction
{
    public class DistanceTool : MonoBehaviour
    {
        // line prefab
        public GameObject DistanceLinePrefab;
        // lines[]
        private List<GameObject> AllLines = new List<GameObject>();
        private List<Transform> AllHits = new List<Transform>();

        // update
        private void Update()
        {
            // check if tool is on
            if (PlayerPrefs.GetString("UIButtonDistanceTool Button") == "True")
            {
                CheckLines();
                updateLines();
            }
        }
        private void CheckLines()
        {
            // do sphere cast range 2m
            // destroy all lines

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1);
            // make line with text  
            List<GameObject> newLines = new List<GameObject>();
            List<Transform> newHits = new List<Transform>();
            foreach (Collider col in hitColliders)
            {
                if (AllHits.Contains(col.transform))
                {
                    newLines.Add(AllLines[AllHits.IndexOf(col.transform)]);
                    newHits.Add(col.transform);
                }
                else
                {
                    if (col.tag == "Hologram")
                    {
                        newHits.Add(col.transform);
                        GameObject DistanceLine = Instantiate(DistanceLinePrefab);
                        newLines.Add(DistanceLine);
                    }
                }
            }
            foreach (GameObject line in AllLines)
            {
                if (!newLines.Contains(line))
                {
                    Destroy(line);
                }
            }
            AllLines = newLines;
            AllHits = newHits;

        }
        private void updateLines()
        {
            for(var i = 0; i< AllLines.Count; i++)
            {
                // set text to distance + m
                AllLines[i].GetComponentInChildren<TextMesh>().text = "M";
                Vector3[] points = new Vector3[2];
                points[0] = transform.position;
                points[1] = AllHits[i].position;
                AllLines[i].GetComponent<LineRenderer>().SetPositions(points);
            }
        }
    }
}
