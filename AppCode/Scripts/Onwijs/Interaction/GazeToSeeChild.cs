using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Onwijs.Interaction
{
    public class GazeToSeeChild : Gaze
    {
        private Vector3[] oldScale;
        private Transform[] childeren;
        private bool scalingUp = false;
        public bool stayActive = false;
        private void Start()
        {
            childeren = GetComponentsInChildren<Transform>();
            oldScale = new Vector3[childeren.Length];
            for (int i = 1; i < childeren.Length; i++)
            {    
                oldScale[i] = childeren[i].localScale;
                StartCoroutine(ScaleDown(i));
            }
          
           
        }
        public void setStayActive(bool Bool) { stayActive = Bool; }
        public override void OnFocusEnter()
        {
            base.OnFocusEnter();
            for (int i = 1; i < childeren.Length; i++)
            {
                StartCoroutine(ScaleUp(i));
            }
        }
        public override void OnFocusExit()
        {
            base.OnFocusExit();
            if (!stayActive)
            {
                for (int i = 1; i < childeren.Length; i++)
                {
                    StartCoroutine(ScaleDown(i));
                }
            }
        }
        IEnumerator ScaleUp(int i)
        {
            scalingUp = true;
           while(childeren[i].localScale != oldScale[i])
            {
                childeren[i].localScale = childeren[i].localScale + new Vector3(0.05f, 0.05f, 0.05f);
                yield return new WaitForFixedUpdate();
            }
            scalingUp = false;
        }
        IEnumerator ScaleDown(int i)
        {
  
            while (childeren[i].localScale != new Vector3(0, 0, 0))
            {
                if (!scalingUp)
                {
                    childeren[i].localScale = childeren[i].localScale - new Vector3(0.05f, 0.05f, 0.05f);
                }
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
