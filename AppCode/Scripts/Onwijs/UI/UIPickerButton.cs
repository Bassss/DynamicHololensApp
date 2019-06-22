using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Onwijs.UI
{
    public class UIPickerButton : MonoBehaviour
    {
        public UIPickerMenu Menu;

        public void click()
        {
            Menu.PickItem(gameObject.name);
        }
    }
}
