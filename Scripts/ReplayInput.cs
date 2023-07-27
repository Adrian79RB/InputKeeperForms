using InputKeeperForms.Scripts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InputKeeperForms
{
    public partial class ReplayInput : Form
    {
        // Instance of the main form
        private Form1 mainFormInstance;

        // InputCode list where we can load all the data saved in the json file
        private InputCodeList inputData;

        private System.Windows.Forms.Timer secondsCounter = new System.Windows.Forms.Timer(); // Timer used to count the seconds since the input replay started
        private float timeRecording; // Var used to keep count of the seconds that have passed

        // Token used to kill all the active and unfinished threads 
        private CancellationTokenSource cancellationTokenSource;

        public ReplayInput(Form1 mainForm)
        {
            InitializeComponent();
            mainFormInstance = mainForm;

            InputSimulation.CreateDictionary(typeof(Keys)); // We need to activate the dictionary to link the keyboard keys name with their scan codes.

            // Setting all the parameters needed to make the timer work properly
            timeRecording = 0f;
            secondsCounter.Tick += SecondsCountAsync;
            secondsCounter.Interval = 10;

            // Load all the json files kept on the mainFormInstance.jsonFilesRoute
            LoadFilesFromDirectory();
        }

        private void btn_DeleteInputFile_Click(object sender, EventArgs e)
        {
            File.Delete(mainFormInstance.jsonFilesRoute + cmb_SelectedInputFile.SelectedItem.ToString()); // Deleting the combo box selected item
            LoadFilesFromDirectory(); // Reloading the combo box info after deleting a file

            mainFormInstance.CheckPlayBtnActivation(); // Checking if there are enough json files to reproduce one
        }

        private void btn_SelectedInputFile_Click(object sender, EventArgs e)
        {
            if (pnl_InputShower.Controls.Count > 0)
                pnl_InputShower.Controls.Clear(); // Clear the pnl_inputShower to load it with the new Input data

            TransformJsonDataToInputData(); // Load the json file data into the input data list

            foreach (var input in inputData.inputList) // Creates one Card for each input in the pnl_InputShower
            {
                if (input.keyString.Contains("Wheel")) // If the input is the Scroll Wheel -> show as time holding the direction of the wheel movement
                {
                    mainFormInstance.OpenChildCardForm(new InputCard(input.keyString, input.performedTime, Convert.ToString((uint)input.holdingTime, 16).ElementAt<char>(0) == 'f' ? -1 : 1 ,
                        input.holdingTime > 0 ? false : true, UserController.Mouse_Keyboard), pnl_InputShower);
                }
                else // If not -> show the real holding time of the input
                {
                    mainFormInstance.OpenChildCardForm(new InputCard(input.keyString, input.performedTime, input.holdingTime,
                        input.holdingTime > 0 ? false : true, UserController.Mouse_Keyboard), pnl_InputShower);
                }
            }

            // Activate Play button
            ManageButtonState(btn_PlayInput, null, mainFormInstance.enabledButtonColor, Color.White);
        }

        private void btn_PlayInput_Click(object sender, EventArgs e)
        {
            // Managing the main buttons activation
            ManageButtonState(btn_StopPlaying, btn_PlayInput, mainFormInstance.enabledButtonColor, mainFormInstance.disabledButtonColor);
            ManageButtonState(null, btn_DeleteInputFile, Color.White, mainFormInstance.disabledDelButtonColor);
            ManageButtonState(null, btn_SelectedInputFile, Color.White, mainFormInstance.disabledPositiveButtonColor);
            cmb_SelectedInputFile.Enabled = false;
            mainFormInstance.ButtonDeactivationWhileRecording();

            // The cancellationTokenSource is reset in each input data execution
            cancellationTokenSource = new CancellationTokenSource();

            // Starting the seconds counter Timer
            timeRecording = 0f;
            secondsCounter.Start();

            // Call the method to bring the game to the front and focus it
            Debug.WriteLine("Bring app: " + @mainFormInstance.gameRoute);
            AppFocusClass.BringMainWindowToFront(@mainFormInstance.gameRoute);
            RepeatActionsAsync(cancellationTokenSource.Token); // Execute all the actions saved
        }

        private void btn_StopPlaying_Click(object sender, EventArgs e)
        {
            // Launch the Cancellation token -> It finishes all the active threads
            cancellationTokenSource.Cancel();
            Debug.WriteLine("Cancellation Token sent");

            // Restart the Timer
            secondsCounter.Stop();
            timeRecording = 0f;

            // Managing the main buttons activation
            ManageButtonState(btn_PlayInput, btn_StopPlaying, mainFormInstance.enabledButtonColor, mainFormInstance.disabledButtonColor);
            ManageButtonState(btn_DeleteInputFile, null, mainFormInstance.enabledDelButtonColor, Color.White);
            ManageButtonState( btn_SelectedInputFile, null, mainFormInstance.enabledPositiveButtonColor, Color.White);
            cmb_SelectedInputFile.Enabled = true;
            mainFormInstance.ButtonActivationAfterRecording();
        }

        /// <summary>
        /// Method that search the directory where the json files are kept and load their names into the cmb_SelectedInputFile Data Source
        /// </summary>
        private void LoadFilesFromDirectory()
        {
            // Find the directory and the files
            DirectoryInfo directory = new DirectoryInfo(mainFormInstance.jsonFilesRoute);
            FileInfo[] fileInfo = directory.GetFiles("*.json");

            // Setting the properties of the cmb_SelectedInputFile
            cmb_SelectedInputFile.DataSource = fileInfo;
            cmb_SelectedInputFile.ValueMember = "LastWriteTime";
            cmb_SelectedInputFile.DisplayMember = "Name";

            // Check if buttons have to be activated or deactivated based on the existent json files
            CheckDelButtonActivation();
            CheckPlayButtonActivation();
        }

        /// <summary>
        /// Method that checks if the Delete Json Files has to be activated or deactivated
        /// </summary>
        private void CheckDelButtonActivation()
        {
            if (mainFormInstance.SearchJsonFile(mainFormInstance.jsonFilesRoute) == null) // If there isn't any json file in the directory
                ManageButtonState(null, btn_DeleteInputFile, Color.White, mainFormInstance.disabledDelButtonColor);
            else // If there is any json file in the directory
                ManageButtonState(btn_DeleteInputFile, null, mainFormInstance.enabledDelButtonColor, Color.White);
        }

        /// <summary>
        /// Method that checks if the Play Button has to be activated or deactivated
        /// </summary>
        private void CheckPlayButtonActivation()
        {
            if (btn_PlayInput.Enabled)
            {
                pnl_InputShower.Controls.Clear();
                ManageButtonState(null, btn_PlayInput, Color.White, mainFormInstance.disabledButtonColor);
            }
        }

        /// <summary>
        /// Method that finds the selected JsonData file and load its info in the input data list
        /// </summary>
        private void TransformJsonDataToInputData()
        {
            DirectoryInfo directory = new DirectoryInfo(mainFormInstance.jsonFilesRoute);
            FileInfo[] files = directory.GetFiles(cmb_SelectedInputFile.SelectedItem.ToString()); // Find the file selected with the combo box

            if (files.Length > 0) // if the file selected exist in the directory
            {
                string json = File.ReadAllText(files[0].FullName);

                inputData = JsonConvert.DeserializeObject<InputCodeList>(json);
            }
            else
                Debug.WriteLine("The selected file does not exist in the Json files directory");
        }

        /// <summary>
        /// Activates the disabled Control and deactivates the enabled Control
        /// </summary>
        /// <param name="disabledOne">Control that is currently disabled and it is wanted to enable it (Nulable)</param>
        /// <param name="enabledOne">Control that is currently enabled and it is wanted to disable it (Nulable)</param>
        /// <param name="enabledColor">Color that corresponds to the currently disabledControl</param>
        /// <param name="disabledColor">Color that corresponds to the currently enabledControl</param>
        private void ManageButtonState(Control disabledOne, Control enabledOne, Color enabledColor, Color disabledColor)
        {
            if(disabledOne != null) // if the disable control is not null, enable it
            {
                disabledOne.Enabled = true;
                disabledOne.BackColor = enabledColor;
            }

            if(enabledOne != null) // if the enable control is not null, disable it
            {
                enabledOne.Enabled = false;
                enabledOne.BackColor = disabledColor;
            }
        }

        /// <summary>
        /// Method used to launch a thread for each of the inputs performed by the player
        /// </summary>
        /// <returns>Return the async task to manage the threads</returns>
        private async Task RepeatActionsAsync(CancellationToken cancellationToken)
        {
            Task lastInput = null;

            // Going through all the inputs kept in the inputData.inputsList
            foreach (var input in inputData.inputList)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                lastInput = DoActionAsync(input, cancellationToken);
            }

            if(lastInput != null)
                await lastInput; // Waiting for the last input to be sent

            Debug.WriteLine("Last input 'do action' done");
            secondsCounter.Stop();
            timeRecording = 0f;

            // Manage the main buttons activation
            ManageButtonState(btn_PlayInput, btn_StopPlaying, mainFormInstance.enabledButtonColor, mainFormInstance.disabledButtonColor);
            ManageButtonState(btn_DeleteInputFile, null, mainFormInstance.enabledDelButtonColor, Color.White);
            ManageButtonState(btn_SelectedInputFile, null, mainFormInstance.enabledPositiveButtonColor, Color.White);
            cmb_SelectedInputFile.Enabled = true;
            mainFormInstance.ButtonActivationAfterRecording();
        }

        /// <summary>
        /// Method that Waits for the time when the input was performed 
        /// </summary>
        /// <param name="input">Input data that represents the input performed by the player</param>
        /// <param name="lastAction">Input performed just before the one that it is being sent</param>
        /// <param name="cancellationToken">Token needed to close the threads if the user presses the stop button</param>
        /// <returns></returns>
        private async Task DoActionAsync(InputCode input, CancellationToken cancellationToken)
        {
            Debug.WriteLine("Tiempo de espera de " + input.keyString + ": " + input.performedTime);
            await Task.Delay(TimeSpan.FromSeconds(input.performedTime));
            Debug.WriteLine("Tiempo esperado de " + input.keyString);

            if (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    Debug.WriteLine("Sending Input data");
                    InputSimulation.SendPressInputData(input, 0, mainFormInstance.mouseSensitivity);

                    if (!cancellationToken.IsCancellationRequested && !input.keyString.Contains("Wheel") && !input.keyString.Contains("Mouse"))
                    {
                        await Task.Delay(TimeSpan.FromMilliseconds(input.holdingTime * 1000));

                        InputSimulation.SendPressInputData(input, 1, mainFormInstance.mouseSensitivity);
                    }
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    MessageBox.Show("Excepcion out of range; message: " + ex.Message + "; linea: " + ex.InnerException);
                }
                catch (KeyNotFoundException e)
                {
                    MessageBox.Show("Excepcion key not found; message: " + e.Message + "; linea: " + e.InnerException);
                }

            }
        }

        /// <summary>
        /// Method that counts the seconds since the player presses the start button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SecondsCountAsync(object sender, EventArgs e)
        {
            Task t = Task.Run(() =>
            {
                timeRecording += 0.01f;
            });

            await t;
        }
    }
}
