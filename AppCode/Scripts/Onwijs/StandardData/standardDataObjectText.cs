using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Onwijs.StandardData
{
    public class standardDataObjectText : standardDataObject
    {
        private TextMesh textComponent;

        public override void Start()
        {
            base.Start();
            type = "text";
            textComponent = transform.GetChild(0).GetComponent<TextMesh>();
        }
        public override void loadAsset(bool downloadOverride = false)
        {
            base.loadAsset(downloadOverride);
            if (textComponent == null)
            {
                textComponent = transform.GetChild(0).GetComponent<TextMesh>();
            }
            textComponent.text = data.localPath;
        }
    }
}
