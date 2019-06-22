using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

namespace Onwijs.StandardData
{
    public class standardDataObjectImage : standardDataObject
    {
        private SpriteRenderer imageComponent;

        public override void Start()
        {
            base.Start();
            type = "image";
            imageComponent = GetComponent<SpriteRenderer>();
        }
        public override void loadAsset(bool downloadOverride = false)
        {
            base.loadAsset(downloadOverride);
            string Path = Application.persistentDataPath + data.localPath;
            if (checkLocalPath(Path) && !downloadOverride)
            {
                Sprite NewSprite;
                Texture2D SpriteTexture = LoadTexture(Path);
                NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0.5f, 0.5f), 100.0f); // create sprite with downloaded texture, pivot in center, pixelPerUnit 
                GetComponent<SpriteRenderer>().sprite = NewSprite;
            }
            else if(data.dataUrl != null && data.dataUrl != "" && tries < 2)
            {
                StartCoroutine(DownloadImage(data.dataUrl));
            }
            else
            {
                Debug.Log("no data found for " + type + " with localIdentifier " + localIdentifier);
            }
        }
        public Texture2D LoadTexture(string FilePath)
        {
            Texture2D Tex2D;
            byte[] FileData;

            if (File.Exists(FilePath))
            {
                FileData = File.ReadAllBytes(FilePath);
                Tex2D = new Texture2D(2, 2);  // Create new "empty" texture
                if (Tex2D.LoadImage(FileData))
                {         // Load the imagedata into the texture (size is set automatically)
                    return Tex2D;     
                }
            }
            return null; 
        }
        IEnumerator DownloadImage(string MediaUrl) // download image in coroutine to keep performance on main thread high
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
                Debug.Log("help: "+request.error);
            else
            {
                Sprite NewSprite;
                Texture2D SpriteTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
                GetComponent<SpriteRenderer>().sprite = NewSprite;
                if(!Directory.Exists(Application.persistentDataPath + "/images/"))
                {
                    Directory.CreateDirectory(Application.persistentDataPath + "/images");
                }
                string[] words = MediaUrl.Split('/');
                string[] fileName = words[words.Length - 1].Split('.');
                string newLocalPath =  "/images/" + fileName[0]+".png";
              //  Debug.Log(Application.persistentDataPath + newLocalPath);
                File.WriteAllBytes(Application.persistentDataPath + newLocalPath, SpriteTexture.EncodeToPNG());
                data.localPath = newLocalPath;
                Save();
            }
        }
    }
}
