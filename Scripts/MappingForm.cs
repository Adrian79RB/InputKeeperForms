using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace InputKeeperForms
{
    public partial class MappingForm : Form
    {
        private Form1 mainFormInstance; // Reference to the main form instance

        // Route where the Gamepad mapping configuration is kept
        private string route = "C:/Users/adria/Desktop/UniversidadInputKeeperForms/GamepadConfig/";
        
        private Color btnKeyColor = Color.FromArgb(220, 203, 117); // Determine that the cmb box is shwoing the keys enum data
        private Color btnButtonColor = Color.FromArgb(223, 145, 204); // Determine that the cmb box is showing the Button Code enum data

        private Dictionary<string, bool> buttonBooleans; // Keep the info about which cmb boxes are showing keys data and button code data; True -> Keys / False -> Button Code

        public MappingForm(Form1 mainForm)
        {
            InitializeComponent();

            mainFormInstance = mainForm; // Instance that represents the main form
            buttonBooleans = new Dictionary<string, bool>();
            InitializeComboBoxesData();
        }

        /// <summary>
        /// Load the combo boxes with the json files data
        /// </summary>
        private void InitializeComboBoxesData()
        {
            if (mainFormInstance.activeControllerType == UserController.Gamepad)
            {
                TransformJsonToGamepadConfig(); // Read the data from the Json file if they exist
                lbl_ControllerWarning.Text = "You are using Gamepad inputs"; // Change the info text

                // Loop through all the controls in the form
                for (int i = 0; i < pnl_MappingForn.Controls.Count; i++)
                {
                    pnl_MappingForn.Controls[i].Visible = true;

                    if (pnl_MappingForn.Controls[i].GetType() == typeof(ComboBox)) // If he control is a combo box
                    {
                        var aux = (ComboBox)pnl_MappingForn.Controls[i];

                        if (mainFormInstance.mappedGamepadButtons.Count > 0) // if there is any gamepad button data already saved
                        {
                            // Loop through all the mapped buttons that have been saved
                            foreach (var keyCode in mainFormInstance.mappedGamepadButtons.Values)
                            {
                                if (keyCode.cmb_name == aux.Name) // if the combo box and the mapped button match
                                {
                                    buttonBooleans[aux.Name] = keyCode.codeType; // Save the mapped button type into the dictionary

                                    // Load the correct data depending on the button type
                                    if (keyCode.codeType)
                                    {
                                        aux.DataSource = Enum.GetValues(typeof(Keys));
                                        aux.SelectedItem = keyCode.keyCode;
                                    }
                                    else
                                    {
                                        aux.DataSource = Enum.GetValues(typeof(ButtonCode));
                                        aux.SelectedItem = keyCode.buttonCode;
                                    }
                                }
                            }
                        }
                        else // If there is no gamepad button data saved
                        {
                            aux.DataSource = Enum.GetValues(typeof(Keys));
                            buttonBooleans[aux.Name] = true;
                        }

                    }
                    else if(pnl_MappingForn.Controls[i].GetType() == typeof(Button)) // If the control is a button
                    {
                        var aux = (Button)pnl_MappingForn.Controls[i];
                        if(mainFormInstance.mappedGamepadButtons.Count > 0) // If there is any gamepad button data already saved
                        {
                            // Loop through all the mapped button that have been saved
                            foreach (var keyCode in mainFormInstance.mappedGamepadButtons.Values)
                            {
                                // All the keys start with 'cmb' because they are based on combo boxes names
                                // But we need to change this for 'btn' to compare the data with the buttons of the form
                                var name = keyCode.cmb_name.Replace("cmb", "btn");

                                if(name == aux.Name) // If the button name and the key altered named match
                                {
                                    aux.BackColor = keyCode.codeType ? btnKeyColor : btnButtonColor; // Set the correct color depending on the data type
                                    aux.Text = keyCode.codeType ? "K" : "B"; // Change button text (K -> Keys enum; B -> Button Code enum
                                }
                            }
                        }
                        else if(pnl_MappingForn.Controls[i] != btnExit && pnl_MappingForn.Controls[i] != btn_SaveConfig 
                            && pnl_MappingForn.Controls[i] != btn_ResetConfig) // Keep this buttons with the usual color
                        {
                            aux.BackColor = btnKeyColor;
                            aux.Text = "K";
                        }
                    }
                }
            }
            else // If the user is using Keyboard & Mouse input
            {
                lbl_ControllerWarning.Text = "You are using Keyboard/Mouse input";

                for (int i = 0; i < pnl_MappingForn.Controls.Count; i++)
                {
                    // Make invisible all the combo boxes and buttons except this ones
                    if (pnl_MappingForn.Controls[i] != lbl_ControllerWarning && pnl_MappingForn.Controls[i] != btnExit
                        && pnl_MappingForn.Controls[i] != btn_ResetConfig && pnl_MappingForn.Controls[i] != btn_SaveConfig)
                        pnl_MappingForn.Controls[i].Visible = false;
                }
            }
        }

        /// <summary>
        /// Save the mapped gamepad data into a json file
        /// </summary>
        private void TransformGamepadConfigToJson()
        {
            
            List<ExtendedKeyCode> auxiliarList = new List<ExtendedKeyCode>();
            foreach (var key in mainFormInstance.mappedGamepadButtons.Keys) // Loop through all the keys in the mapped gamepad buttons dictionary
                auxiliarList.Add(mainFormInstance.mappedGamepadButtons[key]);

            ExtendedKeyCodeList mappedButons = new ExtendedKeyCodeList(); // The Extended Key Code List is needed to set the format and write/read the json file easily
            mappedButons.mappedKeys = auxiliarList.ToArray();
            string json = JsonConvert.SerializeObject(mappedButons); // Transform the list into a json object

            File.WriteAllText(route + "config.json", json); // Create a file using the json object
        }

        /// <summary>
        /// Load the Json data into the Extended Key Code List 
        /// </summary>
        private void TransformJsonToGamepadConfig()
        {
            FileInfo currentConfig = mainFormInstance.SearchJsonFile(route); // Find the mapped config file

            if (currentConfig != null) // Load the file and transform it to actual data
            {
                mainFormInstance.mappedGamepadButtons.Clear();

                string json = File.ReadAllText(route + currentConfig); // Load data into a json string object
                ExtendedKeyCodeList mappedButtons = JsonConvert.DeserializeObject<ExtendedKeyCodeList>(json);

                if(mappedButtons.mappedKeys[0].cmb_name != "") // If the data has been load correctly
                {
                    foreach (var keyCode in mappedButtons.mappedKeys)
                    {
                        mainFormInstance.mappedGamepadButtons.Add(keyCode.cmb_name, keyCode); // Add the data loaded into mappedGamepadButtons dictionary
                    }
                }
            }
            else
                Debug.WriteLine("That File does not exit");
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            mainFormInstance.CurrentFormClosed(this); // Call the closing method in the main form
            this.Dispose();
            this.Close();
        }

        private void btn_SaveConfig_Click(object sender, EventArgs e)
        {
            if(mainFormInstance.activeControllerType == UserController.Gamepad)
            {
                mainFormInstance.mappedGamepadButtons.Clear(); // Clear all the data kept in the mapped gamepad button dictionary

                // Loop through all the controls of the form
                for (int i = 0; i < pnl_MappingForn.Controls.Count; i++)
                {
                    if (pnl_MappingForn.Controls[i].GetType() == typeof(ComboBox)) // if the control is a combo box
                    {
                        var aux = (ComboBox)pnl_MappingForn.Controls[i];
                        ExtendedKeyCode newCode = new ExtendedKeyCode();

                        // Assign the combo box info into a extended key code object to add it to the mapped gamepad buttons dictionary
                        newCode.cmb_name = aux.Name;
                        newCode.codeType = buttonBooleans[pnl_MappingForn.Controls[i].Name];
                        newCode.keyCode = aux.SelectedItem.GetType() == typeof(Keys) ? (Keys)aux.SelectedItem : Keys.None;
                        newCode.buttonCode = aux.SelectedItem.GetType() == typeof(ButtonCode) ? (ButtonCode)aux.SelectedItem : ButtonCode.None;

                        mainFormInstance.mappedGamepadButtons.Add(newCode.cmb_name, newCode);
                    }
                }

                TransformGamepadConfigToJson();
            }
        }

        private void btn_ResetConfig_Click(object sender, EventArgs e)
        {
            if(mainFormInstance.activeControllerType == UserController.Gamepad)
            {
                // Loop through all the controls in the form
                for (int i = 0; i < pnl_MappingForn.Controls.Count; i++)
                {
                    if (pnl_MappingForn.Controls[i].GetType() == typeof(ComboBox)) // if the control is a Combo box
                    {
                        // Set the default data
                        var aux = (ComboBox)pnl_MappingForn.Controls[i];
                        aux.DataSource = Enum.GetValues(typeof(Keys));
                        aux.SelectedItem = 0;
                    }
                    else if (pnl_MappingForn.Controls[i].GetType() == typeof(Button) && pnl_MappingForn.Controls[i] != btnExit
                        && pnl_MappingForn.Controls[i] != btn_SaveConfig && pnl_MappingForn.Controls[i] != btn_ResetConfig) // if the control is a button different from the specified ones
                    {
                        // Set the default data
                        var aux = (Button)pnl_MappingForn.Controls[i];
                        aux.Text = "K";
                        aux.BackColor = btnKeyColor;
                    }
                }

                // Set the default values in the mapped gamepad buttons dictionary
                foreach (var keyCode in mainFormInstance.mappedGamepadButtons.Values)
                {
                    keyCode.cmb_name = "";
                    keyCode.keyCode = Keys.None;
                    keyCode.buttonCode = ButtonCode.None;
                }

                TransformGamepadConfigToJson();
            }
        }

        /// <summary>
        /// Change the combo boxes data from a enum type to another
        /// </summary>
        /// <param name="currentButton">Button which has to be changed</param>
        /// <param name="currentComboBox">Combo box whose data has to be changed</param>
        private void ChangeComboBoxContent(Button currentButton, ComboBox currentComboBox)
        {
            // Change the type of the enum data kept in the button Booleans dictionary
            buttonBooleans[currentComboBox.Name] = !buttonBooleans[currentComboBox.Name];

            if (buttonBooleans[currentComboBox.Name]) // if the enum is Keys
            {
                // Set the correct color, text and data depending on the type of enum
                currentButton.BackColor = btnKeyColor;
                currentButton.Text = "K";
                currentComboBox.DataSource = Enum.GetValues(typeof(Keys));
            }
            else // if the enum is Button Code
            {
                // Set the correct color, text and data depending on the type of enum
                currentButton.BackColor = btnButtonColor;
                currentButton.Text = "B";
                currentComboBox.DataSource = Enum.GetValues(typeof(ButtonCode));
            }

            currentComboBox.SelectedIndex = 0;
        }

        // Method which each form button calls when it is clicked 
        // Each button is related with a gamepad button individually
        #region buttonMethods
        private void btn_leftStickButton_Click(object sender, EventArgs e)
        {
            ChangeComboBoxContent(btn_LeftThumbstick, cmb_LeftThumb);
        }

        private void btn_rightStickButton_Click(object sender, EventArgs e)
        {
            ChangeComboBoxContent(btn_RightThumbstick, cmb_RightThumb);
        }

        private void btn_buttonSouth_Click(object sender, EventArgs e)
        {
            ChangeComboBoxContent(btn_A, cmb_A);
        }

        private void btn_buttonEast_Click(object sender, EventArgs e)
        {
            ChangeComboBoxContent(btn_B, cmb_B);
        }

        private void btn_buttonWest_Click(object sender, EventArgs e)
        {
            ChangeComboBoxContent(btn_X, cmb_X);
        }

        private void btn_buttonNorth_Click(object sender, EventArgs e)
        {
            ChangeComboBoxContent(btn_Y, cmb_Y);
        }

        private void btn_dpadDown_Click(object sender, EventArgs e)
        {
            ChangeComboBoxContent(btn_DPadDown, cmb_DPadDown);
        }

        private void btn_dpadRight_Click(object sender, EventArgs e)
        {
            ChangeComboBoxContent(btn_DPadRight, cmb_DPadRight);
        }

        private void btn_dpadLeft_Click(object sender, EventArgs e)
        {
            ChangeComboBoxContent(btn_DPadLeft, cmb_DPadLeft);
        }

        private void btn_dpadUp_Click(object sender, EventArgs e)
        {
            ChangeComboBoxContent(btn_DPadUp, cmb_DPadUp);
        }

        private void btn_leftShoulder_Click(object sender, EventArgs e)
        {
            ChangeComboBoxContent(btn_LeftShoulder, cmb_LeftShoulder);
        }

        private void btn_rightShoulder_Click(object sender, EventArgs e)
        {
            ChangeComboBoxContent(btn_RightShoulder, cmb_RightShoulder);
        }

        private void btn_leftTrigger_Click(object sender, EventArgs e)
        {
            ChangeComboBoxContent(btn_LeftTrigger, cmb_LeftTrigger);
        }

        private void btn_rightTrigger_Click(object sender, EventArgs e)
        {
            ChangeComboBoxContent(btn_RightTrigger, cmb_RightTrigger);
        }

        private void btn_leftStickUp_Click(object sender, EventArgs e)
        {
            ChangeComboBoxContent(btn_LeftStickUp, cmb_LeftStickUp);
        }

        private void btn_leftStickLeft_Click(object sender, EventArgs e)
        {
            ChangeComboBoxContent(btn_LeftStickLeft, cmb_LeftStickLeft);
        }

        private void btn_leftStickRight_Click(object sender, EventArgs e)
        {
            ChangeComboBoxContent(btn_LeftStickRight, cmb_LeftStickRight);
        }

        private void btn_leftStickDown_Click(object sender, EventArgs e)
        {
            ChangeComboBoxContent(btn_LeftStickDown, cmb_LeftStickDown);
        }

        private void btn_rightStickUp_Click(object sender, EventArgs e)
        {
            ChangeComboBoxContent(btn_RightStickUp, cmb_RightStickUp);
        }

        private void btn_rightStickLeft_Click(object sender, EventArgs e)
        {
            ChangeComboBoxContent(btn_RightStickLeft, cmb_RightStickLeft);
        }

        private void btn_rightStickRight_Click(object sender, EventArgs e)
        {
            ChangeComboBoxContent(btn_RightStickRight, cmb_RightStickRight);
        }

        private void btn_rightStickDown_Click(object sender, EventArgs e)
        {
            ChangeComboBoxContent(btn_RightStickDown, cmb_RightStickDown);
        }
        #endregion
    }
}
