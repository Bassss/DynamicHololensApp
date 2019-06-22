using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using ZXing;


namespace Onwijs.Utility
{
    public class QRScanner
    {
        private static WebCamTexture webCam = null;

        private static bool isRunning = false;

        public static IEnumerator QRReaderCoroutine(Action<string> action, bool loop = false)
        {
#if !UNITY_EDITOR
                    webCam = new WebCamTexture();
                    webCam.Play();
                    yield return new WaitForSeconds(1.0f);
                    IBarcodeReader reader = new BarcodeReader();
                    var bytes = new byte[webCam.width * webCam.height * 4];
                    isRunning = true;
#endif
            while (isRunning)
            {
#if !UNITY_EDITOR
                        var dataColor = webCam.GetPixels32();
                        for (var i = 0; i < dataColor.Length; i++)
                        {
                            bytes[i * 4 + 0] = dataColor[i].b;
                            bytes[i * 4 + 1] = dataColor[i].g;
                            bytes[i * 4 + 2] = dataColor[i].r;
                            bytes[i * 4 + 3] = dataColor[i].a;
                        }

                        var res = reader.Decode(bytes, webCam.width, webCam.height, BitmapFormat.BGR32);

                        if (res != null)
                        {
                            if (loop == false) StopWebCam();
                            if (action != null) action.Invoke(res.Text);
                        }
#endif
                yield return new WaitForSeconds(2f);
            }

        }
        //        public static IEnumerator QRReaderCoroutine(Action<string> action, bool loop = false)
        //        {
        //            webCam = new WebCamTexture();
        //            webCam.Play();
        //            yield return new WaitForSeconds(1.0f);
        //            string result = null;
        //#if !UNITY_EDITOR
        //                    var QRTask = new Task<string>( () =>
        //                    {
        //                        IBarcodeReader reader = new BarcodeReader();
        //                        var bytes = new byte[webCam.width * webCam.height * 4];
        //                        var dataColor = webCam.GetPixels32();
        //                        for (var i = 0; i < dataColor.Length; i++)
        //                        {
        //                            bytes[i * 4 + 0] = dataColor[i].b;
        //                            bytes[i * 4 + 1] = dataColor[i].g;
        //                            bytes[i * 4 + 2] = dataColor[i].r;
        //                            bytes[i * 4 + 3] = dataColor[i].a;
        //                        }
        //                        var res = reader.Decode(bytes, webCam.width, webCam.height, BitmapFormat.BGR32);
        //                        return res.Text;
        //                    });
        //                   QRTask.Start();
        //                    while (result == null)
        //                   {
        //                     Debug.Log(QRTask.Status);
        //                if (QRTask.Status == TaskStatus.Running)
        //                {
        //                    QRTask.Wait();
        //                }
        //                if (QRTask.Status == TaskStatus.RanToCompletion )
        //                       {
        //                            QRTask.Start();
        //                       }
        //                   }
        //#endif
        //            action.Invoke(result);
        //        }
        public static void StopWebCam()
        {
            isRunning = false;
            webCam.Stop();
        }
    }
}
