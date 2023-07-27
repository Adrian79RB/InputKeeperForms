using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
using InputKeeperForms.Scripts;
using System.Numerics;
using System.Runtime.InteropServices;
using SharpDX.XInput;


namespace InputKeeperForms
{
    public partial class SaveInput : Form
    {
        // WParam Hook ids
        private const int KEYDOWN_WPARAM = 256;
        private const int KEYUP_WPARAM = 257;
        private const int MOUSEWHEEL_WPARAM = 522;
        private const int MOUSEMOVE_WPARAM = 512;


        private Form1 mainFormInstance; // Instance of the main form

        private List<InputCode> inputsPerformedByPlayer; // List that keep all the inputs performed by player while the program is recording

        private Controller controller; // Reference to the controller
        private Gamepad gamepad; // Gamepad object that represents the active controller state
        private bool wasConnected = false;

        private Timer timer = new Timer(); // Timer that determines when the user input is checked 
        private Timer secondsCountTimer = new Timer(); // Timer that count the seconds elapsed since the program starts recording
        private float timeRecording; // float that keeps how much time has been recording the program
        private bool isRecording;

        private Vector2 lastMousePos;
        private float mouseDeltaThreshold = 30f;
        private Task lastMouseTask;

        private double triggerThreshold = 0.2; // minimum value needed to detect a trigger button input
        private short stickThreshold = 6000; // minimum value needed to detect a stick button movement
        private double stickDeltaThreshold = 0.4; // minimum distance difference between current stick position and last position to consider it a change in the stick

        private InputCode LeftThumbstick_LastInputX; // Last input in X axis performed by left thumbstick
        private InputCode LeftThumbstick_LastInputY; // Last input in Y axis performed by left thumbstick

        private InputCode RightThumbstick_LastInputX; // Last input in X axis performed by right thumbstick
        private InputCode RightThumbstick_LastInputY; // Last input in Y axis performed by right thumbstick

        private string imageRoute = "C:/Users/Teseo/Desktop/Practicas_2/InputImages/"; // Directory where the gamepad button images are kept
        private Dictionary<string, string> buttonImages; // Dictionary used to link the gamepad button names with their corresponding images

        private bool isOutside; // Variable that determines if the mouse pointer is inside the form or outside

        private float scrollSpeed = 0x90;

        // Dll import of the needed functions to set a hook on the keyboard
        #region BlackBox_KeyBoardHandler
        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr CallNextHookEx(IntPtr hHook, int nCode, IntPtr wParam, IntPtr lParam);

        IntPtr hHook;
        private delegate IntPtr HookProc(int nCode, IntPtr wp, IntPtr lp);
        HookProc lpfn;

        IntPtr scrollWheelHook;
        HookProc scrollWheel_lpfn;
        #endregion

        public SaveInput(Form1 mainForm)
        {
            InitializeComponent();
            mainFormInstance = mainForm;

            timeRecording = 0f;
            secondsCountTimer.Tick += SecondsCountMehtod; // Set the method and the interval that this timer is going to call
            secondsCountTimer.Interval = 10;

            txb_jsonFilesRoute.Text = mainFormInstance.jsonFilesRoute; // Show where the json files are going to be saved

            if (mainFormInstance.activeControllerType == UserController.Gamepad) // If the user is using Gamepad input
            {
                // Reference to the active controller
                controller = new Controller(UserIndex.One);

                if (!controller.IsConnected)
                {
                    btn_StartRecording.Enabled = false;
                    btn_StartRecording.BackColor = mainFormInstance.disabledButtonColor;
                    lbl_ControllerNotDetected.Visible = true;
                    wasConnected = false;
                }
                else
                {
                    gamepad = controller.GetState().Gamepad;
                    wasConnected = true;
                }

                timer.Tick += InputChecking; // Set the method and the interval that this timer is going to call
                timer.Interval = 10;
                timer.Start();
            }
            else
            {
                timer.Tick += CheckMouseButtons;
                timer.Interval = 10;
                timer.Start();
            }

            CheckDelButtonActivation();

            InitializeImagesDictionary();

            SetHook();
            SetScrollWheelHook();
        }

        /// <summary>
        /// Method that establishes the hook to capture every key in the keyboard and call the KeyboardHookProc method
        /// </summary>
        private void SetHook()
        {
            int id_hook = 13; // WH_KEYBOARD_LL - HOOK
            lpfn = new HookProc(KeyboardHookProc);
            using (ProcessModule curModule = Process.GetCurrentProcess().MainModule)
                hHook = SetWindowsHookEx(id_hook, lpfn, GetModuleHandle(curModule.ModuleName), 0);
        }

        /// <summary>
        /// Method that establishes the hook to capture every mouse movement or scroll movement and call the CheckMouseHook
        /// </summary>
        private void SetScrollWheelHook()
        {
            int id_hook = 14; // WH_MOUSE - HOOK
            scrollWheel_lpfn = new HookProc(CheckMouseHook);
            using (ProcessModule curModule = Process.GetCurrentProcess().MainModule)
                scrollWheelHook = SetWindowsHookEx(id_hook, scrollWheel_lpfn, GetModuleHandle(curModule.ModuleName), 0);
        }

        /// <summary>
        /// Method that initializes the inputPerformedByPlayer list and the event that handle the input recording
        /// </summary>
        private void ActivateInputList()
        {
            InputReceived.inputReceived += AddInputToList;

            if (inputsPerformedByPlayer == null)
                inputsPerformedByPlayer = new List<InputCode>();
        }

        /// <summary>
        /// Method that clears the inputPerfomedByPlayer list and the event that handle the input recording
        /// </summary>
        public void DeactivateInputList()
        {
            InputReceived.inputReceived -= AddInputToList;

            inputsPerformedByPlayer = null;
        }

        /// <summary>
        /// Method that adds new inputs to the inputsPerformedByPlayer list
        /// </summary>
        /// <param name="newInput">New input performed by player</param>
        private void AddInputToList(InputCode newInput)
        {
            inputsPerformedByPlayer.Add(newInput);
        }

        /// <summary>
        /// Fills up the imagesDictionary with the images route
        /// </summary>
        private void InitializeImagesDictionary()
        {
            // Initializing the Dictionary
            buttonImages = new Dictionary<string, string>();

            // Saving all the routes related with the GamepadButtons enum
            foreach (var name in Enum.GetNames(typeof(GamepadButtonFlags)))
            {
                buttonImages.Add(name, imageRoute + name + ".PNG");
            }

            // Saving the routes of the buttons that are not saved as doubles in the GamepadButtons Enum
            buttonImages.Add("LeftTrigger", imageRoute + "LeftTrigger.PNG");
            buttonImages.Add("RightTrigger", imageRoute + "RightTrigger.PNG");

            buttonImages.Add("LeftStickUp", imageRoute + "LeftStickUp.PNG");
            buttonImages.Add("LeftStickRight", imageRoute + "LeftStickRight.PNG");
            buttonImages.Add("LeftStickLeft", imageRoute + "LeftStickLeft.PNG");
            buttonImages.Add("LeftStickDown", imageRoute + "LeftStickDown.PNG");

            buttonImages.Add("RightStickUp", imageRoute + "RightStickUp.PNG");
            buttonImages.Add("RightStickRight", imageRoute + "RightStickRight.PNG");
            buttonImages.Add("RightStickLeft", imageRoute + "RightStickLeft.PNG");
            buttonImages.Add("RightStickDown", imageRoute + "RightStickDown.PNG");
        }

        private void btn_saveJsonFilesRoute_Click(object sender, EventArgs e)
        {
            // Set the route specify in the text box in a json file 
            mainFormInstance.jsonFilesRoute = txb_jsonFilesRoute.Text.Replace("\\", "/");
            mainFormInstance.TransformRouteToJsonData(txb_jsonFilesRoute.Text.Replace("\\", "/"));
        }

        private void btn_StartRecording_Click(object sender, EventArgs e)
        {
            if (btn_StartRecording.Enabled) // If the start recording button has not been pressed
            {
                ActivateInputList();
                AppFocusClass.BringMainWindowToFront(@mainFormInstance.gameRoute);

                for (int i = 0; i < mainFormInstance.currentInputs.Count; i++)
                {
                    Debug.WriteLine("Input restante: " + mainFormInstance.currentInputs[i]);
                }

                isRecording = true;
                secondsCountTimer.Start(); // Start the seconds counter

                // Manage all the buttons and texts in the form
                txb_jsonFilesRoute.Enabled = false;
                btn_DeleteFile.Enabled = false;
                btn_DeleteFile.BackColor = mainFormInstance.disabledDelButtonColor;
                btn_saveJsonFilesRoute.Enabled = false;
                btn_saveJsonFilesRoute.BackColor = mainFormInstance.disabledPositiveButtonColor;
                pnl_InputDataReceived.Focus();

                btn_StartRecording.Enabled = false;
                btn_StartRecording.BackColor = mainFormInstance.disabledButtonColor;

                btn_StopRecording.Enabled = true;
                btn_StopRecording.BackColor = mainFormInstance.enabledButtonColor;

                // Deactivate the main form buttons
                mainFormInstance.ButtonDeactivationWhileRecording();

                foreach (var control in pnl_InputDataReceived.Controls)
                {
                    Form currentForm = (Form)control;
                    currentForm.Close();
                    currentForm.Dispose();
                }
                pnl_InputDataReceived.Controls.Clear();
            }
        }

        private void btn_StopRecording_Click(object sender, EventArgs e)
        {
            if (btn_StopRecording.Enabled) // If the stop recording button has not been pressed
            {
                if(mainFormInstance.currentInputs.Count > 0)
                {
                    for (int i = 0; i < mainFormInstance.currentInputs.Count; i++)
                    {
                        if(mainFormInstance.currentInputs[i].Contains("Mouse") || mainFormInstance.currentInputs[i].Contains("Wheel"))
                            KeyUpDetection(mainFormInstance.currentInputs[i], mainFormInstance.currentInputs, mainFormInstance.performedTimes, timeRecording, 0f, Input_Type.Mouse);
                        else
                            KeyUpDetection(mainFormInstance.currentInputs[i], mainFormInstance.currentInputs, mainFormInstance.performedTimes, timeRecording, 0f, Input_Type.Keyboard);
                    }
                }

                InputCodeComparer icc = new InputCodeComparer();
                inputsPerformedByPlayer.Sort(icc);

                TransformPlayerInputToJsonData(); // Save the inputs performed by player into a json file
                DeactivateInputList();

                // Manage all the buttons and texts in the form
                txb_jsonFilesRoute.Enabled = true;
                btn_DeleteFile.Enabled = true;
                btn_DeleteFile.BackColor = mainFormInstance.enabledDelButtonColor;
                btn_saveJsonFilesRoute.Enabled = true;
                btn_saveJsonFilesRoute.BackColor = mainFormInstance.enabledPositiveButtonColor;

                isRecording = false;
                secondsCountTimer.Stop(); // Stop the seconds counter 
                timeRecording = 0f;

                btn_StopRecording.Enabled = false;
                btn_StopRecording.BackColor = mainFormInstance.disabledButtonColor;

                btn_StartRecording.Enabled = true;
                btn_StartRecording.BackColor = mainFormInstance.enabledButtonColor;

                mainFormInstance.CheckPlayBtnActivation(); // Activate the play input button if it is needed
                CheckDelButtonActivation(); // Activate Delete Files button if it is needed

                // Activate the main form buttons
                mainFormInstance.ButtonActivationAfterRecording();
            }
        }

        private void btn_DeleteFile_Click(object sender, EventArgs e)
        {
            var lastFile = mainFormInstance.SearchJsonFile(mainFormInstance.jsonFilesRoute); // Search the last written file in the directory 
            File.Delete(mainFormInstance.jsonFilesRoute + lastFile.Name);

            if (pnl_InputDataReceived.Controls.Count > 0)
                pnl_InputDataReceived.Controls.Clear();

            CheckDelButtonActivation(); // Deactivate Delete Files button if it is needed

            mainFormInstance.CheckPlayBtnActivation(); // Deactivate  the play input button if it is needed
        }

        /// <summary>
        /// Checks if the Delete Last File button should be able or disabled
        /// </summary>
        private void CheckDelButtonActivation()
        {
            if (mainFormInstance.SearchJsonFile(mainFormInstance.jsonFilesRoute) == null) // If there isn't any input json file inside the specified directory
            {
                btn_DeleteFile.Enabled = false;
                btn_DeleteFile.BackColor = mainFormInstance.disabledDelButtonColor;
            }
            else // If there is any input json file in the directory
            {
                btn_DeleteFile.Enabled = true;
                btn_DeleteFile.BackColor = mainFormInstance.enabledDelButtonColor;
            }
        }

        /// <summary>
        /// Transform the inputs performed by player list into a json object
        /// </summary>
        private void TransformPlayerInputToJsonData()
        {
            // Create the input list to establish the Json file structure
            InputCodeList inputList = new InputCodeList();
            inputList.inputList = inputsPerformedByPlayer.ToArray(); // Transform the input list into an array

            string json = JsonConvert.SerializeObject(inputList); // Serialize the list into a json string

            int inputNumber = 1;

            // Search which input file does not exist yet
            while (File.Exists(mainFormInstance.jsonFilesRoute + "input_" + inputNumber + ".json"))
                inputNumber++;

            // Create a new json file
            File.WriteAllText(mainFormInstance.jsonFilesRoute + "input_" + inputNumber + ".json", json);
        }

        // Methods that handle the mouse and keyboard event to save the player input
        #region Keyboard/Mouse input Detection
        /// <summary>
        /// Method called whenever a key is pressed while the inputs are being recorded
        /// </summary>
        /// <param name="code">Input code</param>
        /// <param name="wParam">Pointer that represent the input type</param>
        /// <param name="lParam">Data object that represents the input data</param>
        /// <returns>Send the input to the next handler</returns>
        private IntPtr KeyboardHookProc(int code, IntPtr wParam, IntPtr lParam)
        {
            // If the inputs are being recorded and the user selected controller is the Keyboard
            if (isRecording && mainFormInstance.activeControllerType == UserController.Mouse_Keyboard)
            {
                KeyboardInput data = (KeyboardInput)Marshal.PtrToStructure(lParam, typeof(KeyboardInput)); // Reading data from the keyboard

                // If the current inputs array does not contain the data -> the key is being pressed for the first time
                if (wParam.ToInt32() == KEYDOWN_WPARAM && !mainFormInstance.currentInputs.Contains(((Keys)data.wVk).ToString()))
                {
                    // Saves the key data
                    KeyDownDetection(((Keys)data.wVk).ToString(), mainFormInstance.currentInputs, mainFormInstance.performedTimes, timeRecording);

                    Debug.WriteLine("Arrow: " + (Keys)data.wVk + "; code: " + data.wVk);

                    // Creates an input card to show the info to the user
                    mainFormInstance.OpenChildCardForm(new InputCard(((Keys)data.wVk).ToString(), timeRecording, 0f, true, mainFormInstance.activeControllerType), pnl_InputDataReceived);
                }
                // If the current imput data is already inside the current inputs array -> the key is being released
                else if (wParam.ToInt32() == KEYUP_WPARAM && mainFormInstance.currentInputs.Contains(((Keys)data.wVk).ToString())) 
                { 
                    // Calculates the time that the key has been hold
                    var timeHolding = timeRecording - mainFormInstance.performedTimes[mainFormInstance.currentInputs.IndexOf(((Keys)data.wVk).ToString())];

                    // Saves the key in the inputsPerformedByPlayer list
                    KeyUpDetection(((Keys)data.wVk).ToString(), mainFormInstance.currentInputs, mainFormInstance.performedTimes, timeRecording, timeHolding, Input_Type.Keyboard);

                    // Creates an input card to show the info to the user
                    mainFormInstance.OpenChildCardForm(new InputCard(((Keys)data.wVk).ToString(), timeRecording, timeHolding, false, mainFormInstance.activeControllerType), pnl_InputDataReceived);
                }
            }

            return CallNextHookEx(hHook, code, wParam, lParam);
        }

        /// <summary>
        /// Handler that is called when the mouse pointer gets out the active form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WaitingForClickOutside(object sender, EventArgs e)
        {
            if (isRecording)
            {
                isOutside = true;
                Debug.WriteLine("Cambio outside: " + isOutside);
            }
        }

        /// <summary>
        /// Handler that is called when the mouse pointer gets in the active form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WaitingForClickInside(object sender, EventArgs e)
        {
            if (isRecording)
            {
                isOutside = false;
                Debug.WriteLine("Cambio outside: " + isOutside);
            }
        }

        /// <summary>
        /// Method that checks if any mouse button is being pressed or released
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CheckMouseButtons(object sender, EventArgs e)
        {
            if(isOutside && isRecording && mainFormInstance.activeControllerType == UserController.Mouse_Keyboard) // It Checks only the input performed outside the active form
            {
                Task t = null;

                // If any of the buttons is being pressed and it is not cointained in the current input array
                if ((MouseButtons == MouseButtons.Left || MouseButtons == MouseButtons.Right || MouseButtons == MouseButtons.Middle)
                    && !mainFormInstance.currentInputs.Contains(MouseButtons.ToString()))
                {
                    Debug.WriteLine("key down detection method have been launched");
                    t = KeyDownDetection(MouseButtons.ToString() + "Button", mainFormInstance.currentInputs, mainFormInstance.performedTimes, timeRecording);

                    mainFormInstance.OpenChildCardForm(new InputCard(MouseButtons.ToString() + "Button", timeRecording, 0f, true, UserController.Mouse_Keyboard), pnl_InputDataReceived);
                }
                // If the current input is not the Left mouse button but it is in the current inputs array -> the button is being released
                else if (MouseButtons != MouseButtons.Left && mainFormInstance.currentInputs.Contains(MouseButtons.Left.ToString() + "Button"))
                {
                    Debug.WriteLine("key up detection have been launch");
                    var timeHolding = timeRecording - mainFormInstance.performedTimes[mainFormInstance.currentInputs.IndexOf(MouseButtons.Left.ToString() + "Button")];

                    t = KeyUpDetection(MouseButtons.Left.ToString() + "Button", mainFormInstance.currentInputs, mainFormInstance.performedTimes, timeRecording, timeHolding, Input_Type.Mouse);

                    mainFormInstance.OpenChildCardForm(new InputCard(MouseButtons.Left.ToString() + "Button", timeRecording, timeHolding, false, mainFormInstance.activeControllerType), pnl_InputDataReceived);
                }
                // If the current input is not the Right mouse button but it is in the current inputs array -> the button is being released
                else if (MouseButtons != MouseButtons.Right && mainFormInstance.currentInputs.Contains(MouseButtons.Right.ToString() + "Button"))
                {
                    var timeHolding = timeRecording - mainFormInstance.performedTimes[mainFormInstance.currentInputs.IndexOf(MouseButtons.Right.ToString() + "Button")];
                    t = KeyUpDetection(MouseButtons.Right.ToString() + "Button", mainFormInstance.currentInputs, mainFormInstance.performedTimes, timeRecording, timeHolding, Input_Type.Mouse);

                    mainFormInstance.OpenChildCardForm(new InputCard(MouseButtons.Right.ToString() + "Button", timeRecording, timeHolding, false, mainFormInstance.activeControllerType), pnl_InputDataReceived);
                }
                // If the current input is not the Middle mouse button but it is in the current inputs array -> the button is being released
                else if (MouseButtons != MouseButtons.Middle && mainFormInstance.currentInputs.Contains(MouseButtons.Middle.ToString() + "Button"))
                {
                    var timeHolding = timeRecording - mainFormInstance.performedTimes[mainFormInstance.currentInputs.IndexOf(MouseButtons.Middle.ToString() + "Button")];
                    t = KeyUpDetection(MouseButtons.Middle.ToString() + "Button", mainFormInstance.currentInputs, mainFormInstance.performedTimes, timeRecording, timeHolding, Input_Type.Mouse);

                    mainFormInstance.OpenChildCardForm(new InputCard(MouseButtons.Middle.ToString() + "Button", timeRecording, timeHolding, false, mainFormInstance.activeControllerType), pnl_InputDataReceived);
                }

                if (t != null)
                    await t;
            }
        }

        /// <summary>
        /// Method that checks if the mouse wheel is triggered or any mouse movement
        /// </summary>
        /// <param name="code">Input code</param>
        /// <param name="WParam">Pointer that represents the input type</param>
        /// <param name="LParam">Data object that represents the input data</param>
        /// <returns>Send the input to the next handler</returns>
        private IntPtr CheckMouseHook(int code, IntPtr WParam, IntPtr LParam)
        {
            if(isOutside && isRecording && mainFormInstance.activeControllerType == UserController.Mouse_Keyboard)
            {
                // Data loaded from the data object
                MouseInput data = (MouseInput)Marshal.PtrToStructure(LParam, typeof(MouseInput));

                // Mouse Wheel input
                if (WParam.ToInt32() == MOUSEWHEEL_WPARAM)
                {
                    CheckScrollWheel(data);
                }
                // Mouse movement input
                else if (WParam.ToInt32() == MOUSEMOVE_WPARAM)
                {
                    CheckMousePos(data);
                }

            }

            return CallNextHookEx(scrollWheelHook, code, WParam, LParam);
        }

        /// <summary>
        /// Method that detects the scroll wheel inputs and creates its cards
        /// </summary>
        /// <param name="data">Data related with scroll input</param>
        /// <returns></returns>
        private async Task CheckScrollWheel(MouseInput data)
        {
            mainFormInstance.currentInputs.Add("Mouse_Wheel");
            mainFormInstance.performedTimes.Add(timeRecording);

            // Select the first input data digit 
            var initDigit = Convert.ToString(data.mouseData, 16).ElementAt<char>(0);

            Task t = KeyUpDetection("Mouse_Wheel", mainFormInstance.currentInputs, mainFormInstance.performedTimes,
                initDigit == 'f' ? -1 : 1, initDigit == 'f' ? -scrollSpeed : scrollSpeed, Input_Type.Mouse);

            mainFormInstance.OpenChildCardForm(new InputCard("Mouse Wheel", timeRecording, initDigit == 'f' ? -1 : 1, true, mainFormInstance.activeControllerType), pnl_InputDataReceived);
            
            await t;
        }

        /// <summary>
        /// Method that detects all mouse position changes 
        /// </summary>
        /// <param name="data">Data related with mouse position inputs</param>
        /// <returns></returns>
        private async Task CheckMousePos(MouseInput data)
        {
            if (lastMousePos == null) // Last mouse input position
                lastMousePos = new Vector2(data.pt.X, data.pt.Y);

            // If the difference between mouse positions is greater enought to be bigger than the threshold
            if (Math.Abs(data.pt.X - lastMousePos.X) > mouseDeltaThreshold || Math.Abs(data.pt.Y - lastMousePos.Y) > mouseDeltaThreshold)
            {
                if (lastMouseTask != null)
                    await lastMouseTask;

                Task t = Task.Run(() =>
                {
                    var delta = new Vector2(data.pt.X, data.pt.Y) - lastMousePos;
                    //MouseMoveDetection(delta);
                    lastMousePos = new Vector2(data.pt.X, data.pt.Y);

                });

                mainFormInstance.OpenChildCardForm(new InputCard("Mouse Move", timeRecording, 0f, false, mainFormInstance.activeControllerType), pnl_InputDataReceived);

                lastMouseTask = t;

                await t;
            }
        }


        /// <summary>
        /// Thread that save the mouse position
        /// </summary>
        /// <param name="delta">Difference between last mouse position and the current one</param>
        /// <returns></returns>
        private void MouseMoveDetection(Vector2 delta)
        {

            if (delta.X != 0 || delta.Y != 0)
            {
                var auxVector = Vector2.Zero;
                var addVector = Vector2.Zero;

                // Adding one to the needed axis in order to get to the mouse delta position
                while (auxVector.X != Math.Floor(delta.X) || auxVector.Y != Math.Floor(delta.Y))
                {
                    if (auxVector.X < Math.Floor(delta.X))
                    {
                        addVector.X = 1;
                        auxVector.X++;
                    }
                    else if (auxVector.X > Math.Floor(delta.X))
                    {
                        addVector.X = -1;
                        auxVector.X--;
                    }

                    if (auxVector.Y < Math.Floor(delta.Y))
                    {
                        addVector.Y = 1;
                        auxVector.Y++;

                    }
                    else if (auxVector.Y > Math.Floor(delta.Y))
                    {
                        addVector.Y = -1;
                        auxVector.Y--;
                    }

                    InputReceived.invokeEvent(new InputCode(0f, "mouseMove", addVector, timeRecording, Input_Type.Mouse));
                }
            }
        }
        #endregion

        // Methods that handle the gamepad events to save the player input
        #region Gamepad input Detection

        /// <summary>
        /// Checks if the program is already recording and if the input device is a gamepad, then it calls to the CheckingGamepadInputs method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void InputChecking(object sender, EventArgs e)
        {
            // If the app is recording the inputs, the device used by the player is a gamepad and there is a gamepad connected
            if (isRecording && mainFormInstance.activeControllerType == UserController.Gamepad && controller.IsConnected)
            {
                if (!wasConnected) // If there was no gamepad connected during the last input check
                {
                    // Activate the recording buttons
                    btn_StartRecording.Enabled = true;
                    btn_StartRecording.BackColor = mainFormInstance.enabledButtonColor;

                    // Deactivate the warining text
                    lbl_ControllerNotDetected.Visible = false;
                    wasConnected = true;
                }

                await CheckingGamepadInputs();

            }
            else if (!controller.IsConnected && wasConnected) // If there was a gamepad connected in the last input check but now it has been disconnected
            {
                // Deactivate the recording buttons
                btn_StartRecording.Enabled = false;
                btn_StartRecording.BackColor = mainFormInstance.disabledButtonColor;

                // Activate the warning text
                lbl_ControllerNotDetected.Visible = true;
                wasConnected = false;
            }
        }

        /// <summary>
        /// Checks if each gamepad button is being pressed or released
        /// </summary>
        /// <param name="gamepadReading">Data provided by the gamepad driver that determine which buttons are being pressed</param>
        private async Task CheckingGamepadInputs()
        {
            gamepad = controller.GetState().Gamepad;

            CheckGamepadButton(gamepad.Buttons, GamepadButtonFlags.A);

            CheckGamepadButton(gamepad.Buttons, GamepadButtonFlags.B);

            CheckGamepadButton(gamepad.Buttons, GamepadButtonFlags.X);

            CheckGamepadButton(gamepad.Buttons, GamepadButtonFlags.Y);

            CheckGamepadButton(gamepad.Buttons, GamepadButtonFlags.DPadDown);

            CheckGamepadButton(gamepad.Buttons, GamepadButtonFlags.DPadRight);

            CheckGamepadButton(gamepad.Buttons, GamepadButtonFlags.DPadLeft);

            CheckGamepadButton(gamepad.Buttons, GamepadButtonFlags.DPadUp);

            CheckGamepadButton(gamepad.Buttons, GamepadButtonFlags.LeftShoulder);

            CheckGamepadButton(gamepad.Buttons, GamepadButtonFlags.RightShoulder);

            CheckGamepadButton(gamepad.Buttons, GamepadButtonFlags.LeftThumb);

            CheckGamepadButton(gamepad.Buttons, GamepadButtonFlags.RightThumb);

            CheckGamepadTrigger("cmb_LeftTrigger", gamepad.LeftTrigger);

            CheckGamepadTrigger("cmb_RightTrigger", gamepad.RightTrigger);

            // Left Thumb checking inputs
            if (Math.Abs(gamepad.LeftThumbX + 1) > stickThreshold || Math.Abs(gamepad.LeftThumbY + 1) > stickThreshold)
            {
                if(gamepad.LeftThumbX > stickThreshold)
                    CheckGamepadStick("cmb_LeftStickRight", (float)gamepad.LeftThumbX / short.MinValue * -1, (float)gamepad.LeftThumbY / short.MaxValue,
                        true, ref LeftThumbstick_LastInputX);
                else if(gamepad.LeftThumbX < -stickThreshold)
                    CheckGamepadStick("cmb_LeftStickLeft", (float)gamepad.LeftThumbX / short.MinValue * -1, (float)gamepad.LeftThumbY / short.MaxValue,
                        true, ref LeftThumbstick_LastInputX);

                if(gamepad.LeftThumbY > stickThreshold)
                    CheckGamepadStick("cmb_LeftStickUp", (float)gamepad.LeftThumbX / short.MinValue * -1, (float)gamepad.LeftThumbY / short.MaxValue,
                        false, ref LeftThumbstick_LastInputY);
                else if(gamepad.LeftThumbY < -stickThreshold)
                    CheckGamepadStick("cmb_LeftStickDown", (float)gamepad.LeftThumbX / short.MinValue * -1, (float)gamepad.LeftThumbY / short.MaxValue,
                        false, ref LeftThumbstick_LastInputY);
            }
            else if(Math.Abs(gamepad.LeftThumbX + 1) < stickThreshold || Math.Abs(gamepad.LeftThumbY + 1) < stickThreshold)
            {
                Debug.WriteLine("Left last input X: " + LeftThumbstick_LastInputX.movementAxis + "; Left last input Y: " + LeftThumbstick_LastInputY.movementAxis + "; current value: " + gamepad.LeftThumbX + ", "+ gamepad.LeftThumbY);
                if(LeftThumbstick_LastInputX.keyString != null)
                {
                    if (gamepad.LeftThumbX < stickThreshold && LeftThumbstick_LastInputX.movementAxis.X > stickThreshold / short.MaxValue)
                        CheckGamepadStickRelease("cmb_LeftStickRight", ref LeftThumbstick_LastInputX);
                    else if (gamepad.LeftThumbX > -stickThreshold && LeftThumbstick_LastInputX.movementAxis.X < -(stickThreshold / short.MaxValue))
                        CheckGamepadStickRelease("cmb_LeftStickLeft", ref LeftThumbstick_LastInputX);
                }

                if(LeftThumbstick_LastInputY.keyString != null)
                {
                    if (gamepad.LeftThumbY < stickThreshold && LeftThumbstick_LastInputY.movementAxis.Y > stickThreshold / short.MaxValue)
                        CheckGamepadStickRelease("cmb_LeftStickUp", ref LeftThumbstick_LastInputY);
                    else if (gamepad.LeftThumbY > -stickThreshold && LeftThumbstick_LastInputY.movementAxis.Y < -(stickThreshold / short.MaxValue))
                        CheckGamepadStickRelease("cmb_LeftStickDown", ref LeftThumbstick_LastInputY);
                }
            }

            // Right Thumb checking inputs
            if (Math.Abs(gamepad.RightThumbX + 1) > stickThreshold || Math.Abs(gamepad.RightThumbY + 1) > stickThreshold)
            {
                if (gamepad.RightThumbX > stickThreshold)
                    CheckGamepadStick("cmb_RightStickRight", (float)gamepad.RightThumbX / short.MaxValue, (float)gamepad.RightThumbY / short.MaxValue,
                        true, ref RightThumbstick_LastInputX);
                else if (gamepad.RightThumbX < -stickThreshold)
                    CheckGamepadStick("cmb_RightStickLeft", (float)gamepad.RightThumbX / short.MaxValue, (float)gamepad.RightThumbY / short.MaxValue,
                        true, ref RightThumbstick_LastInputX);

                if (gamepad.RightThumbY > stickThreshold)
                    CheckGamepadStick("cmb_RightStickUp", (float)gamepad.RightThumbX / short.MaxValue, (float)gamepad.RightThumbY / short.MaxValue,
                        false, ref RightThumbstick_LastInputY);
                else if (gamepad.RightThumbY < -stickThreshold)
                    CheckGamepadStick("cmb_RightStickDown", (float)gamepad.RightThumbX / short.MaxValue, (float)gamepad.RightThumbY / short.MaxValue,
                        false, ref RightThumbstick_LastInputY);
            }
            else if (Math.Abs(gamepad.RightThumbX + 1) < stickThreshold || Math.Abs(gamepad.RightThumbY + 1) < stickThreshold)
            {
                if (RightThumbstick_LastInputX.keyString != null)
                {
                    if (gamepad.RightThumbX < stickThreshold && RightThumbstick_LastInputX.movementAxis.X > stickThreshold)
                        CheckGamepadStickRelease("cmb_RightStickRight", ref RightThumbstick_LastInputX);
                    else if (gamepad.RightThumbX > -stickThreshold && RightThumbstick_LastInputX.movementAxis.X < -stickThreshold)
                        CheckGamepadStickRelease("cmb_RightStickLeft", ref RightThumbstick_LastInputX);
                }

                if (RightThumbstick_LastInputY.keyString != null)
                {
                    if (gamepad.RightThumbY < stickThreshold && RightThumbstick_LastInputY.movementAxis.Y > stickThreshold)
                        CheckGamepadStickRelease("cmb_RightStickUp", ref RightThumbstick_LastInputY);
                    else if (gamepad.RightThumbY > -stickThreshold && RightThumbstick_LastInputY.movementAxis.Y < -stickThreshold)
                        CheckGamepadStickRelease("cmb_RightStickDown", ref RightThumbstick_LastInputY);
                }
            }
        }

        /// <summary>
        /// Check whether a button is being pressed or it is being released
        /// </summary>
        /// <param name="gamepadReading">Data provided by the gamepad driver that determine which buttons are being pressed</param>
        /// <param name="button">Specific button that the program is checking</param>
        private async void CheckGamepadButton(GamepadButtonFlags gamepadReading, GamepadButtonFlags button)
        {
            Task t = null;

            // If the button is sent by the gamepad reading and it isn't inside the current inputs list -> button being pressed
            if ((gamepadReading & button) == button
                && !mainFormInstance.currentInputs.Contains(GetButtonName(mainFormInstance.mappedGamepadButtons["cmb_" + button.ToString()])))
            {
                t = KeyDownDetection(GetButtonName(mainFormInstance.mappedGamepadButtons["cmb_" + button.ToString()]),
                    mainFormInstance.currentInputs, mainFormInstance.performedTimes, timeRecording);

                mainFormInstance.OpenChildCardForm(new InputCard(buttonImages[button.ToString()], timeRecording,
                    0f, true, mainFormInstance.activeControllerType), pnl_InputDataReceived);
            }

            // If the button is not sent by the gamepad reading and it is inside the current inputs list -> button being released
            else if (mainFormInstance.currentInputs.Contains(GetButtonName(mainFormInstance.mappedGamepadButtons["cmb_" + button.ToString()]))
                && (gamepadReading & button) != button)
            {
                // Search the name of the mapped button to find the index in the currentInputs array to find the input performed time
                var timeHolding = timeRecording - mainFormInstance.performedTimes[mainFormInstance.currentInputs.IndexOf(GetButtonName(mainFormInstance.mappedGamepadButtons["cmb_" + button.ToString()]))];

                t = KeyUpDetection(GetButtonName(mainFormInstance.mappedGamepadButtons["cmb_" + button.ToString()]),
                    mainFormInstance.currentInputs, mainFormInstance.performedTimes, timeRecording, timeHolding,
                    GetInputType(mainFormInstance.mappedGamepadButtons["cmb_" + button.ToString()]));

                mainFormInstance.OpenChildCardForm(new InputCard(buttonImages[button.ToString()], timeRecording,
                    timeHolding, false, mainFormInstance.activeControllerType), pnl_InputDataReceived);
            }

            if(t != null)
                await t;
        }

        /// <summary>
        /// Check whether a trigger is being hold or it is not
        /// </summary>
        /// <param name="trigger">Name of the specific trigger</param>
        /// <param name="triggerValue">Value generated by the user input</param>
        private async void CheckGamepadTrigger(string trigger, double triggerValue)
        {
            Task t = null;

            // If the trigger value is greater than the detection threshold and it is not inside the current input -> trigger is just being pressed
            if (Math.Abs(triggerValue) > triggerThreshold
                && !mainFormInstance.currentInputs.Contains(GetButtonName(mainFormInstance.mappedGamepadButtons[trigger])))
            {
                t = KeyDownDetection(GetButtonName(mainFormInstance.mappedGamepadButtons[trigger]),
                    mainFormInstance.currentInputs, mainFormInstance.performedTimes, timeRecording);

                mainFormInstance.OpenChildCardForm(new InputCard(buttonImages[trigger.Replace("cmb_", "")], timeRecording,
                    0f, true, mainFormInstance.activeControllerType), pnl_InputDataReceived);
            }
            // If the trigger value is lower than the detection value and it is inside the current input -> trigger is just being released
            else if (Math.Abs(triggerValue) < triggerThreshold
                && mainFormInstance.currentInputs.Contains(GetButtonName(mainFormInstance.mappedGamepadButtons[trigger])))
            {
                // Search the name of the mapped button to find the index in the currentInputs array to find the input performed time
                var timeHolding = timeRecording - mainFormInstance.performedTimes[mainFormInstance.currentInputs.IndexOf(GetButtonName(mainFormInstance.mappedGamepadButtons[trigger]))];

                t = KeyUpDetection(GetButtonName(mainFormInstance.mappedGamepadButtons[trigger]),
                    mainFormInstance.currentInputs, mainFormInstance.performedTimes, timeRecording,
                    timeHolding, GetInputType(mainFormInstance.mappedGamepadButtons[trigger]));

                mainFormInstance.OpenChildCardForm(new InputCard(buttonImages[trigger.Replace("cmb_", "")], timeRecording,
                    timeHolding, false, mainFormInstance.activeControllerType), pnl_InputDataReceived);
            }

            if(t != null)
                await t;
        }

        /// <summary>
        /// Method that checks the joystick movement to determine when it is considered an input
        /// </summary>
        /// <param name="stickAxis">Which is axis of the joystick movement</param>
        /// <param name="xThumbsticValue">Value of the X axis</param>
        /// <param name="yThumbstickValue">Value of the Y axis</param>
        /// <param name="LastPosAxis">Position of the joystick in the last stick input</param>
        /// <param name="LastThumbstickInput">Last input of the joystick</param>
        public void CheckGamepadStick(string stickAxis, float xThumbsticValue, float yThumbstickValue,
            bool LastPosAxis, ref InputCode LastThumbstickInput)
        {
            if (mainFormInstance.mappedGamepadButtons[stickAxis].buttonCode == ButtonCode.MouseMove) // If the joystick is considered as the mouse movement
            {
                // It is kept every movement of the joystick, it is needed to be more accurate
                InputReceived.invokeEvent(new InputCode(0f, GetButtonName(mainFormInstance.mappedGamepadButtons[stickAxis]),
                    new Vector2(xThumbsticValue, yThumbstickValue), timeRecording, Input_Type.Mouse));

                mainFormInstance.OpenChildCardForm(new InputCard(buttonImages[stickAxis.Replace("cmb_", "")], timeRecording, 0f,
                    false, mainFormInstance.activeControllerType), pnl_InputDataReceived);
            }
            else if (mainFormInstance.mappedGamepadButtons[stickAxis].keyCode != Keys.None
                || mainFormInstance.mappedGamepadButtons[stickAxis].buttonCode != ButtonCode.None) // If the joystick is considered as singular keys depending on its axis values
            {

                if ( (LastPosAxis && Math.Abs(LastThumbstickInput.movementAxis.X) < Math.Abs(xThumbsticValue))
                || (!LastPosAxis && Math.Abs(LastThumbstickInput.movementAxis.Y) < Math.Abs(yThumbstickValue)) ) // if difference between the current stick position and the last stick input pos
                {
                    LastThumbstickInput = new InputCode(0f, GetButtonName(mainFormInstance.mappedGamepadButtons[stickAxis]),
                        new Vector2(xThumbsticValue, yThumbstickValue), timeRecording,
                        GetInputType(mainFormInstance.mappedGamepadButtons[stickAxis]));
                }
            }
        }

        /// <summary>
        /// Check the joystick values to determine when it is considered an input
        /// </summary>
        /// <param name="stickAxis">Joystick movement axis</param>
        /// <param name="LastThumbstickInput">Last joystick value considered an input</param>
        private void CheckGamepadStickRelease(string stickAxis, ref InputCode LastThumbstickInput)
        {
            var holdingTime = timeRecording - LastThumbstickInput.performedTime;

            InputReceived.invokeEvent(new InputCode(holdingTime, LastThumbstickInput.keyString, LastThumbstickInput.movementAxis, timeRecording, LastThumbstickInput.device));

            mainFormInstance.OpenChildCardForm(new InputCard(buttonImages[stickAxis.Replace("cmb_", "")], timeRecording, holdingTime, false, mainFormInstance.activeControllerType), pnl_InputDataReceived);

            LastThumbstickInput = new InputCode(0f, null, Vector2.Zero, 0f, Input_Type.Hardware);
        }

        /// <summary>
        /// Get the button name depending on if it is mapped as a key or as a mouse button
        /// </summary>
        /// <param name="input">New input readed</param>
        /// <returns></returns>
        private string GetButtonName(ExtendedKeyCode input)
        {
            if (input.codeType)
                return input.keyCode.ToString();
            else
                return input.buttonCode.ToString();

        }

        /// <summary>
        /// Get the button type depending on if it is mapped as a key or as a mouse button
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private Input_Type GetInputType(ExtendedKeyCode input)
        {
            // Getting mapped key when the user is using a gamepad
            if (input.codeType)
                return input.keyCode.ToString().Contains("Button") ? Input_Type.Mouse : Input_Type.Keyboard;
            else
                return Input_Type.Mouse;
        }
        #endregion

        /// <summary>
        /// Thread that save the key pressed info when an input is received
        /// </summary>
        /// <param name="key">Button pressed</param>
        /// <param name="currentInputs">List of all the inputs performed</param>
        /// <param name="performedTimes">List of the pressing times for each input</param>
        /// <param name="timeRecording">Current recording time</param>
        /// <returns></returns>
        private async Task KeyDownDetection(string key, List<string> currentInputs, List<float> performedTimes, float timeRecording)
        {
            if (key != "None" && !currentInputs.Contains(key))
            {
                Debug.WriteLine("Estoy guardando keyDown");
                currentInputs.Add(key);
                performedTimes.Add(timeRecording);
            }
        }

        /// <summary>
        /// Thread that save the key released info when an input is received
        /// </summary>
        /// <param name="key">Button pressed</param>
        /// <param name="currentInputs">List of all the inputs performed</param>
        /// <param name="performedTimes">List of the pressing times for each input</param>
        /// <param name="timeRecording">Current recording time</param>
        /// <param name="device">Device used to check the user input</param>
        /// <returns></returns>
        private async Task KeyUpDetection(string key, List<string> currentInputs, List<float> performedTimes, float timeRecording, float timeHolding, Input_Type device)
        {
            if (key != "None" && currentInputs.Contains(key))
            {
                int index = currentInputs.IndexOf(key);

                var performedTime = performedTimes[index];

                InputReceived.invokeEvent(new InputCode(timeHolding, key, Vector2.Zero, performedTime, device));

                performedTimes.RemoveAt(index);
                currentInputs.Remove(key);

                Debug.WriteLine("...Saving Finished");
            }
        }

        /// <summary>
        /// Counter that ticks every 10 miliseconds since the recording has started
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SecondsCountMehtod(object sender, EventArgs e)
        {
            Task t = Task.Run(() =>
            {
                timeRecording += 0.01f;
            });

            await t;
        }
    }
}
