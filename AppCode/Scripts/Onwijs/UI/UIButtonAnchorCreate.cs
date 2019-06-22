
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onwijs.Saving;
using Onwijs.Enums;
namespace Onwijs.UI
{
    public class UIButtonAnchorCreate : UIButton
    {
        public string CreatedObjectName = "anchorPoint";
        public GameObject HologramPrefab;
        public override void Start()
        {
            disabled = false;
            base.Start();
        }
        public override void buttonPress()
        {
            GameObject cube = Instantiate(HologramPrefab);
            cube.transform.position = new Vector3(baseUI.transform.position.x + 2,baseUI.transform.position.y,baseUI.transform.position.z);
            cube.GetComponent<AnchorHelper>().click();
        }
    }
}

