using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onwijs.Interaction;
using Onwijs.Saving;
using Onwijs.Enums;
using Onwijs.EventSystem;
namespace Onwijs.UI
{
    public class UIButtonObjectCreator : UIButton
    {
        public string CreatedObjectName = "cube";
        public ObjectType type;
        public GameObject HologramPrefab;
        public GameEvent eventToRaise;
        public override void Start()
        {
            disabled = false;
            base.Start();
        }
        public override void buttonPress()
        {
            GameObject cube = Instantiate(HologramPrefab);
            cube.GetComponent<Rigidbody>().isKinematic = true;
            cube.name = CreatedObjectName;
            if (UIProperties.hasPreviewArea)
            {
                cube.transform.position = new Vector3(UIProperties.previewArea.transform.position.x, UIProperties.previewArea.transform.position.y + 0.5f, UIProperties.previewArea.transform.position.z);
                cube.transform.parent = UIProperties.previewArea.transform;
                UIProperties.ActiveObject = cube;
                UIProperties.activeObjectScale = cube.transform.localScale;
                eventToRaise.Raise();
            }
            else
            {
                cube.transform.position = new Vector3(baseUI.transform.position.x - 2, baseUI.transform.position.y, baseUI.transform.position.z);
                cube.GetComponent<MoveWithPull>().click();
            }
            cube.GetComponent<HologramSaver>().type = type;
        }
    }
}
