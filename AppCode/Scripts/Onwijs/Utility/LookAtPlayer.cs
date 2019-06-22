using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Onwijs.Utility
{
    public class LookAtPlayer : MonoBehaviour
    {
        GameObject player;
        private void Start()
        {
            player = Camera.main.gameObject;
        }
        private void Update()
        {
            transform.LookAt(player.transform);
        }
    }
}
