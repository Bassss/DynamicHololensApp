using UnityEngine;
namespace Onwijs.Interaction
{
    public class TriggerToPlayAudio : MonoBehaviour
    {

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "MainCamera")
            {
                GetComponent<AudioSource>().Play();
            }
        }

    }
}
