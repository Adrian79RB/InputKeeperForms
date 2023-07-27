using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace InputKeeperForms
{
    [StructLayout(LayoutKind.Sequential)]
    public struct KeyboardInput // Represents how the driver understands keyboard input
    {
        public ushort wVk;
        public ushort wScan;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MouseInput // Represents how the driver understands mouse input
    {
        public Point pt;
        public uint mouseData;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HardwareInput // Represents how the driver understands hardware input
    {
        public uint uMsg;
        public ushort wParamL;
        public ushort wParamH;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct InputUnion // Struct needed to join all the ways to represent input in a single object
    {
        [FieldOffset(0)] public MouseInput mi;
        [FieldOffset(0)] public KeyboardInput ki;
        [FieldOffset(0)] public HardwareInput hi;
    }

    public struct INPUT // Struct used to relate the input generated to the input struct needed from the InputUnion Struct
    {
        public int type;
        public InputUnion u;
    }

    [Flags]
    public enum Input_Type // Represents the different types of input available in the current system
    {
        Mouse = 0,
        Keyboard = 1,
        Hardware = 2
    }

    [Flags]
    public enum KeyEventF // Representation of the actions that can be done with a key
    {
        KeyDown = 0x0000,
        ExtendedKey = 0x0001,
        KeyUp = 0x0002,
        Unicode = 0x0004,
        Scancode = 0x0008
    }

    [Flags]
    public enum MouseEventF // Representation of the actions that can be done with a mouse button
    {
        Absolute = 0x8000,
        HWheel = 0x01000,
        MouseMove = 0x0001,
        MoveNoCoalesce = 0x2000,
        LeftButton_0 = 0x0002,
        LeftButton_1 = 0x0004,
        RightButton_0 = 0x0008,
        RightButton_1 = 0x0010,
        MiddleButton_0 = 0x0020,
        MiddleButton_1 = 0x0040,
        VirtualDesk = 0x4000,
        Mouse_Wheel = 0x0800,
        XDown = 0x0080,
        XUp = 0x0100
    }

    internal enum ScanCodeShort : ushort // Representation of the scan codes assign to each key in the keyboard
    {
        NONE = 0x00,
        CLEAR = 0x4C,
        RETURN = 0x1C,
        MENU = 0x38,
        CAPSLOCK = 0x3A,
        ESCAPE = 0x01,
        TAB = 0x0F,
        SPACE = 0x39,
        PAGEUP = 0xE049,
        PAGEDOWN = 0xE051,
        END = 0x4F,
        LEFT = 0xE04B,
        UP = 0xE048,
        RIGHT = 0xE04D,
        DOWN = 0xE050,
        INSERT = 0x52,
        DELETE = 0x53,
        KEYPAD0 = 0x0B,
        KEYPAD1 = 0x02,
        KEYPAD2 = 0x03,
        KEYPAD3 = 0x04,
        KEYPAD4 = 0x05,
        KEYPAD5 = 0x06,
        KEYPAD6 = 0x07,
        KEYPAD7 = 0x08,
        KEYPAD8 = 0x09,
        KEYPAD9 = 0x0A,
        A = 0x1E,
        B = 0x30,
        C = 0x2E,
        D = 0x20,
        E = 0x12,
        F = 0x21,
        G = 0x22,
        H = 0x23,
        I = 0x17,
        J = 0x24,
        K = 0x25,
        L = 0x26,
        M = 0x32,
        N = 0x31,
        O = 0x18,
        P = 0x19,
        Q = 0x10,
        R = 0x13,
        S = 0x1F,
        T = 0x14,
        U = 0x16,
        V = 0x2F,
        W = 0x11,
        X = 0x2D,
        Y = 0x15,
        Z = 0x2C,
        NUMPAD0 = 0x52,
        NUMPAD1 = 0x4F,
        NUMPAD2 = 0x50,
        NUMPAD3 = 0x51,
        NUMPAD4 = 0x4B,
        NUMPAD5 = 0x4C,
        NUMPAD6 = 0x4D,
        NUMPAD7 = 0x47,
        NUMPAD8 = 0x48,
        NUMPAD9 = 0x49,
        F1 = 0x3B,
        F2 = 0x3C,
        F3 = 0x3D,
        F4 = 0x3E,
        F5 = 0x3F,
        F6 = 0x40,
        F7 = 0x41,
        F8 = 0x42,
        F9 = 0x43,
        F10 = 0x44,
        F11 = 0x57,
        F12 = 0x58,
        F13 = 0x64,
        F14 = 0x65,
        F15 = 0x66,
        F16 = 0x67,
        F17 = 0x68,
        F18 = 0x69,
        F19 = 0x6A,
        F20 = 0x6B,
        F21 = 0x6C,
        F22 = 0x6D,
        F23 = 0x6E,
        F24 = 0x76,
        SCROLL = 0x46,
        LSHIFTKEY = 0x2A,
        LCONTROLKEY = 0x1D
    }

    public static class InputSimulation
    {
        private static Dictionary<string, ushort> scanCodeDictionary; // Dictionary that links the key name with the certain scan code

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll")]
        private static extern IntPtr GetMessageExtraInfo();

        public static void SendPressInputData(InputCode inputCode, int pressOption, float mouseSensitivity)
        {
            INPUT[] inputs = new INPUT[1];

            Debug.WriteLine("Creando Input info de: " + inputCode.keyString);
            inputs[0] = CreateInputInfo(inputCode, pressOption, mouseSensitivity);

            SendInput((uint)inputs.Length, inputs, Marshal.SizeOf<INPUT>());
        }

        /// <summary>
        /// Method used to determine which is the source of the input
        /// </summary>
        /// <param name="inputCode">Simulated input</param>
        /// <param name="pressOption">Used to determine if the input is a key/button pressed or released</param>
        /// <returns></returns>
        private static INPUT CreateInputInfo(InputCode inputCode, int pressOption, float mouseSensitivity)
        {
            INPUT newInput = new INPUT();

            if (inputCode.device == Input_Type.Keyboard)
            {
                newInput = createKeyBoardInput(inputCode, pressOption);
            }
            else if (inputCode.device == Input_Type.Mouse)
            {
                newInput = createMouseInput(inputCode, pressOption, mouseSensitivity);
            }

            return newInput;
        }

        /// <summary>
        /// Method that creates the keyboard input that simulates the player's input
        /// </summary>
        /// <param name="inputCode">Simulated input</param>
        /// <param name="pressOption">Used to determine if the input is a key pressed or a key released</param>
        /// <returns></returns>
        private static INPUT createKeyBoardInput(InputCode inputCode, int pressOption)
        {
            Debug.WriteLine("Creating key board Input");
            return new INPUT
            {
                type = (int)Input_Type.Keyboard,
                u = new InputUnion
                {
                    ki = new KeyboardInput
                    {
                        wVk = 0,
                        wScan = TranslateToHex(inputCode.keyString.ToLower()),
                        dwFlags = pressOption == 0 ? (uint)KeyEventF.Scancode : (uint)(KeyEventF.KeyUp | KeyEventF.Scancode),
                        time = 0,
                        dwExtraInfo = GetMessageExtraInfo()
                    }
                }
            };
        }


        /// <summary>
        /// Method used to create the mouse input that simulates the player's input
        /// </summary>
        /// <param name="inputCode">Simulated input</param>
        /// <param name="pressOption">Used to determine if the input is a button pressed or a button released</param>
        /// <returns></returns>
        private static INPUT createMouseInput(InputCode inputCode, int pressOption, float mouseSensitivity)
        {
            // Check if the current input is a press or release action of a mouse button
            if (inputCode.keyString.ToLower().Contains("button"))
            {
                var pressed = pressOption == 0 ? "_0" : "_1";
                var finalKeyString = inputCode.keyString + pressed;
                return new INPUT()
                {
                    type = (int)Input_Type.Mouse,
                    u = new InputUnion
                    {
                        mi = new MouseInput
                        {
                            dwFlags = (uint)TranslateToHex(finalKeyString.ToLower()),
                            time = 0,
                            dwExtraInfo = GetMessageExtraInfo()
                        }
                    }
                };
            }
            else if (inputCode.keyString.Contains("Wheel")) // Check if the current input is a mouse wheel movement
            {
                return new INPUT()
                {
                    type = (int)Input_Type.Mouse,
                    u = new InputUnion
                    {
                        mi = new MouseInput
                        {
                            mouseData = (uint)inputCode.holdingTime,
                            dwFlags = (uint)TranslateToHex(inputCode.keyString.ToLower()),
                            time = 0,
                            dwExtraInfo = GetMessageExtraInfo()
                        }
                    }
                };
            }
            else // Check if the current input is a mouse movement
            {
                Debug.WriteLine("Movement vector: " + inputCode.movementAxis + "; rounded: (" + (inputCode.movementAxis.X > 0.2 ? 1 : inputCode.movementAxis.X < -0.2 ? -1 : 0) + "; "
                    + (inputCode.movementAxis.Y > 0.1 ? 1 : inputCode.movementAxis.Y < -0.1 ? -1 : 0) + ")");
                return new INPUT()
                {
                    type = (int)Input_Type.Mouse,
                    u = new InputUnion
                    {
                        mi = new MouseInput
                        {
                            pt = new Point( (inputCode.movementAxis.X > 0.2 ? 1 : inputCode.movementAxis.X < -0.2 ? -1 : 0) * (int)Math.Round(mouseSensitivity),
                            (inputCode.movementAxis.Y > 0.2 ? 1 : inputCode.movementAxis.Y < -0.2 ? -1 : 0) * (int)Math.Round(mouseSensitivity) ),
                            dwFlags = (uint)TranslateToHex(inputCode.keyString.ToLower()),
                            time = 0,
                            dwExtraInfo = GetMessageExtraInfo()
                        }
                    }
                };
            }
        }

        /// <summary>
        /// Method that translates the Int32 form of the keys to a HEX form
        /// </summary>
        /// <param name="key">Key to search the key scan code in the dictionary</param>
        /// <returns></returns>
        private static ushort TranslateToHex(string key)
        {
            Debug.WriteLine("Key: " + key + "; scan code: " + (0x0 + scanCodeDictionary[key]));
            return (ushort)(0x0 + scanCodeDictionary[key]); // Translate the scan code from int to hex
        }

        /// <summary>
        /// Method used to create and initialize the dictionary where all the scan codes are kept
        /// </summary>
        /// <param name="newType">Enum type used as keys of the dictionary</param>
        public static void CreateDictionary(Type newType)
        {
            if (scanCodeDictionary == null)
            {
                // Saving the scan codes of the keys
                scanCodeDictionary = new Dictionary<string, ushort>();
                foreach (ScanCodeShort code in Enum.GetValues(typeof(ScanCodeShort)))
                {
                    if (scanCodeDictionary.ContainsKey(code.ToString().ToLower()))
                        continue;

                    foreach (var enumName in Enum.GetValues(newType))
                    {
                        if (code.ToString().ToLower() == enumName.ToString().ToLower())
                        {
                            scanCodeDictionary.Add(enumName.ToString().ToLower(), (ushort)code);
                            break;
                        }
                    }
                }

                // Saving the flag codes of the mouse inputs 
                foreach (MouseEventF mouseFlag in Enum.GetValues(typeof(MouseEventF)))
                {
                    if (scanCodeDictionary.ContainsKey(mouseFlag.ToString().ToLower()))
                        continue;
                    else
                    {
                        scanCodeDictionary.Add(mouseFlag.ToString().ToLower(), (ushort)mouseFlag);
                    }
                }
            }
        }
    }
}
