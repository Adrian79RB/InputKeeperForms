using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InputKeeperForms
{
    public enum ButtonCode { None, ScrollWheel_Forward, ScrollWheel_Backward, MouseMove } // Enum that represent the actions that could be performed using the mouse
    public enum UserController { Gamepad, Mouse_Keyboard } // Enum that represent the Controller devices variety whose input can be read by the system

    // Struct used to represent the inputs done by the user
    public struct InputCode
    {
        public float holdingTime;
        public string keyString;
        public Vector2 movementAxis;
        public float performedTime;
        public Input_Type device;

        public InputCode(float newHoldingTime, string newKey, Vector2 newAxis, float newPerformedTime, Input_Type newDevice)
        {
            holdingTime = newHoldingTime;
            keyString = newKey;
            movementAxis = newAxis;
            performedTime = newPerformedTime;
            device = newDevice;
        }
    }

    public class InputCodeComparer : IComparer<InputCode>
    {
        public int Compare(InputCode x, InputCode y)
        {
            if (x.performedTime > y.performedTime)
                return 1;
            else if (x.performedTime < y.performedTime)
                return -1;
            else
                return 0;
        }
    }

    public struct MainFormConfig
    {
        public UserController controllerSelected;
        public float mouseSensitivity;
        public string gameRoute;

        public MainFormConfig(UserController controller, float sensitivity, string route)
        {
            controllerSelected = controller;
            mouseSensitivity = sensitivity;
            gameRoute = route;
        }
    }

    // Class used to represent the gamepad buttons configuration in the json file
    public class ExtendedKeyCodeList
    {
        public ExtendedKeyCode[] mappedKeys;
    }

    // Class used to map the gamepad buttons as Keys and Mouse buttons
    public class ExtendedKeyCode
    {
        public string cmb_name;
        public bool codeType;
        public Keys keyCode;
        public ButtonCode buttonCode;

        public ExtendedKeyCode()
        {
            cmb_name = "";
            codeType = true;
            keyCode = Keys.None;
            buttonCode = ButtonCode.None;
        }
    }
}
