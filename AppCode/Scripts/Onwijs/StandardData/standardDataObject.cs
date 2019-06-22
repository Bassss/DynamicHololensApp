using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Onwijs.Saving;

namespace Onwijs.StandardData
{
    public class standardDataObject : MonoBehaviour
    {
        public string type;
        public int localIdentifier;
        public standardObjectData data;
        private Collider Collider;
        protected int tries = 0;
        
        public virtual void Start()
        {
            Collider = GetComponent<Collider>();
        }
        public virtual void loadAsset(bool downloadOverride = false)
        {
            if (downloadOverride)
            {
                tries = 0;
            }
            tries++;
        }
        public bool checkLocalPath(string FilePath)
        {
            if (File.Exists(FilePath) && data.localPath != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void tryDownload()
        {

        }
        public virtual void play(){}
        public virtual void stop(){}
        public virtual void edit(bool editmode)
        {
            Collider.enabled = editmode;
        }
        public void Save()
        {
            GetComponentInParent<HologramSaver>().Save();
        }

    }
}
