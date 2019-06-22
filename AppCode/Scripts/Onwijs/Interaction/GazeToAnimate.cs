using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Onwijs.Interaction
{
    [RequireComponent(typeof(Animation))]
    public class GazeToAnimate : Gaze
    {
        private Animation AnimationComponent;
        public void Start()
        {
            AnimationComponent = GetComponent<Animation>();

        }
        public override void OnFocusEnter()
        {
            base.OnFocusEnter();
            AnimationComponent.Play();
        }
        public override void OnFocusExit()
        {
            base.OnFocusExit();
            AnimationComponent.Stop();
        }
    }
}
