using UnityEngine;
using UnityEngine.InputSystem;

namespace MinhRoom
{
    public class LightSwitch : MonoBehaviour
    {
        public InputActionReference action;
        private Light myLight;
        private bool isRed = false;

        void Start()
        {
            myLight = GetComponent<Light>();
            action.action.Enable();
            action.action.performed += (ctx) =>
            {
                ChangeColor();
            };
        }

        void ChangeColor()
        {
            isRed = !isRed;
            myLight.color = isRed ? Color.red : Color.white;
        }
    }
}
