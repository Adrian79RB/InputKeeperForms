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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InputKeeperForms
{
    public partial class Form1 : Form
    {
        public UserController activeControllerType; // The user determines the kind of controller that is going to be captured
        public Dictionary<string, ExtendedKeyCode> mappedGamepadButtons; // Object where is related each gamepad button name with a key name or a mouse button name

        // Route where can be found the json file where the json files directory route is kept
        public string staticRoute = "C:/Users/adria/Desktop/Universidad/InputKeeperForms/InputFilesRoute/";
        // Route where can be found the json file where the main form config is kept
        public string configRoute = "C:/Users/adria/Desktop/Universidad/InputKeeperForms/MainFormConfig/";
        // Route to the directory where all the input json files are kept
        public string jsonFilesRoute = "C:/Users/adria/Desktop/Universidad/InputKeeperForms/InputFilesRoute/";

        // List used to keep the inputs done during the recording
        public List<string> currentInputs;
        // List used to keep the times when each input is done (it starts when the recording button is pressed)
        public List<float> performedTimes;

        // Mouse sensitivity value; used to imitate the mouse movement with accurancy when user is using the gamepad joystics
        public float mouseSensitivity;

        // Route to the .exe of the Game that we want to load the input
        public string gameRoute;

        public Color enabledButtonColor = Color.FromArgb(117, 220, 198); // Enabled color for the start and stop recording buttons
        public Color disabledButtonColor = Color.FromArgb(93, 146, 135); // Disabled color for the start and stop recording buttons
        public Color enabledDelButtonColor = Color.FromArgb(220, 147, 117); // Enabled color for the delete json file button
        public Color disabledDelButtonColor = Color.FromArgb(162, 114, 94); // Disabled color for the delete json file button
        public Color enabledPositiveButtonColor = Color.FromArgb(117, 172, 220); // Enabled color for the positive button in the forms
        public Color disabledPositiveButtonColor = Color.FromArgb(106, 141, 173); // Disabled color for the positive button in the forms

        private Form activeForm = null; // Form that it is currently being shown in the main Form panel
        private MainFormConfig mainFormConfig; // Main form data keeps User Controller and Mouse Sensitivity data

        private Color enabledMenuButtonColor = Color.FromArgb(154, 213, 214); // Color that is apply to the enabled buttons
        private Color disabledMenuButtonColor = Color.FromArgb(93, 146, 135); // Color that is apply to the disabled buttons

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            VariableInitialization();
        }

        private void btnMapConf_Click(object sender, EventArgs e)
        {
            OpenChildForm(new MappingForm(this));

            btnSaveInput.Enabled = true;
            btnSaveInput.BackColor = enabledMenuButtonColor;
        }

        private void btnSaveInput_Click(object sender, EventArgs e)
        {
            OpenChildForm(new SaveInput(this));
        }

        private void btnPlayInput_Click(object sender, EventArgs e)
        {
            OpenChildForm(new ReplayInput(this));
        }

        private void btn_SaveGameRoute_Click(object sender, EventArgs e)
        {
            gameRoute = txb_gameRoute.Text;
            mainFormConfig.gameRoute = txb_gameRoute.Text;
        }

        /// <summary>
        /// Method used to open different form inside a main form panel
        /// </summary>
        /// <param name="childForm"></param>
        private void OpenChildForm(Form childForm)
        {
            TransformConfigToJsonData();

            if (activeForm != null)
            {
                if (activeForm.GetType() == childForm.GetType())
                    return;

                // Delete every resource that can be used by the opened form
                activeForm.Close();
                pnl_ChildForm.Controls.Remove(activeForm);
                activeForm.Dispose();
            }

            activeForm = childForm;

            // Setting the new form properties
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            pnl_ChildForm.Controls.Add(childForm);
            pnl_ChildForm.Tag = childForm;

            childForm.BringToFront();
            childForm.Show();

        }

        /// <summary>
        /// Initialize the variable needed to map the gamepad Buttons and the enum that determines the controller type
        /// </summary>
        private void VariableInitialization()
        {
            cmbControllerType.DataSource = Enum.GetValues(typeof(UserController)); // Introduce all the enum values to the drop-down list of the combo box
            mappedGamepadButtons = new Dictionary<string, ExtendedKeyCode>();

            if (currentInputs == null)
                currentInputs = new List<string>();

            if (performedTimes == null)
                performedTimes = new List<float>();

            jsonFilesRoute = TransformJsonDataToRoute(); // Loading the json files directory route from a json file

            LoadMainFormConfig();

            CheckPlayBtnActivation();
        }

        /// <summary>
        /// Event that handles the changing of the selected item value in the combo box, it reloads the mapping form if it is needed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbControllerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            activeControllerType = (UserController)Enum.Parse(typeof(UserController), cmbControllerType.SelectedItem.ToString());
            mainFormConfig.controllerSelected = activeControllerType;

            if (activeForm != null && activeForm.GetType() == typeof(MappingForm))
                OpenChildForm(new MappingForm(this));
        }

        private void nud_ValueChanged(object sender, EventArgs e)
        {
            mouseSensitivity = (float)nud_mouseSensitivity.Value;
            mainFormConfig.mouseSensitivity = mouseSensitivity;
        }

        /// <summary>
        /// It loads the main form config saved in the json file
        /// </summary>
        /// <returns>The mainFormConfig object with the saved information</returns>
        private MainFormConfig TransformJsonDataToConfig()
        {
            var currentConfigFile = SearchJsonFile(configRoute);

            if (currentConfigFile != null)
            {
                string json = File.ReadAllText(configRoute + currentConfigFile);
                return JsonConvert.DeserializeObject<MainFormConfig>(json);
            }
            else
            {
                Debug.WriteLine("The file does not exist");
                return new MainFormConfig(UserController.Gamepad, (float)nud_mouseSensitivity.Value, "");
            }
        }

        /// <summary>
        /// Save the data in the mainFormConfig inside a json file
        /// </summary>
        private void TransformConfigToJsonData()
        {
            string json = JsonConvert.SerializeObject(new MainFormConfig(activeControllerType, mouseSensitivity, gameRoute));
            File.WriteAllText(configRoute + "mainFormConfig.json", json);
        }

        /// <summary>
        /// Load the data from the json file into the controls in the form
        /// </summary>
        private void LoadMainFormConfig()
        {
            mainFormConfig = TransformJsonDataToConfig(); // Call to the method that deserializes the json file

            // Setting the saved values in the corresponding controls
            nud_mouseSensitivity.Value = (decimal)mainFormConfig.mouseSensitivity;
            mouseSensitivity = mainFormConfig.mouseSensitivity;
            gameRoute = mainFormConfig.gameRoute;

            // Load the values in the correct contols
            cmbControllerType.SelectedItem = mainFormConfig.controllerSelected;
            activeControllerType = mainFormConfig.controllerSelected;
            txb_gameRoute.Text = mainFormConfig.gameRoute;
        }

        /// <summary>
        /// It handles when a form decides to close itself
        /// </summary>
        /// <param name="form">Form that is closing itself</param>
        public void CurrentFormClosed(Form form)
        {
            if (form == activeForm)
                activeForm = null;
        }

        /// <summary>
        /// Creates an InputCard form inside the control that is passed as argument
        /// </summary>
        /// <param name="newForm">New Input Card form</param>
        /// <param name="currentControl">Control where the cards have to be displayed</param>
        public void OpenChildCardForm(Form newForm, Control currentControl)
        {
            newForm.TopLevel = false;
            newForm.FormBorderStyle = FormBorderStyle.None;
            newForm.Dock = DockStyle.Top;

            currentControl.Controls.Add(newForm);
            
            newForm.BringToFront();
            newForm.Show();
        }

        /// <summary>
        /// It loads the route to the input files directory from a json file
        /// </summary>
        /// <returns>The json data transform into a string</returns>
        public string TransformJsonDataToRoute()
        {
            var currentRouteFile = SearchJsonFile(staticRoute);

            if (currentRouteFile != null)
            {
                string json = File.ReadAllText(staticRoute + currentRouteFile);
                return JsonConvert.DeserializeObject<string>(json);
            }
            else
            {
                Debug.WriteLine("The file does not exist");
                return null;
            }
        }

        /// <summary>
        /// Save a route as an individual json file
        /// </summary>
        /// <param name="route">Route that is being saved</param>
        public void TransformRouteToJsonData(string route)
        {
            string json = JsonConvert.SerializeObject(route);
            File.WriteAllText(staticRoute + "jsonFilesRoute.json", json);
        }

        /// <summary>
        /// Search the last written file of a directory
        /// </summary>
        /// <param name="route">Absolute route to the directory where the user want to look into</param>
        /// <returns>File where the json data is kept</returns>
        public FileInfo SearchJsonFile(string route)
        {
            DirectoryInfo directory = new DirectoryInfo(route);
            FileInfo[] fileInfo = directory.GetFiles("*.json");
            FileInfo currentFile = null;

            foreach (var file in fileInfo)
            {
                if (currentFile == null || currentFile.LastWriteTime < file.LastWriteTime)
                    currentFile = file;
            }

            return currentFile;
        }

        /// <summary>
        /// Check if there is any input file inside the directory determined by the jsonFilesRoute variable
        /// </summary>
        public void CheckPlayBtnActivation()
        {
            if (SearchJsonFile(jsonFilesRoute) != null)
            {
                btnPlayInput.Enabled = true;
                btnPlayInput.BackColor = enabledMenuButtonColor;
            }
            else
            {
                btnPlayInput.Enabled = false;
                btnPlayInput.BackColor = disabledMenuButtonColor;

                if(activeForm != null && activeForm.GetType() == typeof(ReplayInput))
                {
                    OpenChildForm(new MappingForm(this));
                }
            }
        }

        /// <summary>
        /// Method that deactivates the side panel buttons of the main form
        /// </summary>
        public void ButtonDeactivationWhileRecording()
        {
            btnSaveInput.Enabled = false;
            btnSaveInput.BackColor = disabledMenuButtonColor;

            btnPlayInput.Enabled = false;
            btnPlayInput.BackColor = disabledMenuButtonColor;

            btnMapConf.Enabled = false;
            btnMapConf.BackColor = disabledMenuButtonColor;

            btn_SaveGameRoute.Enabled = false;
            btn_SaveGameRoute.BackColor = disabledPositiveButtonColor;

            txb_gameRoute.Enabled = false;
        }

        /// <summary>
        /// Method that activates the side panel buttons of the main form
        /// </summary>
        public void ButtonActivationAfterRecording()
        {
            btnSaveInput.Enabled = true;
            btnSaveInput.BackColor = enabledMenuButtonColor;

            btnPlayInput.Enabled = true;
            btnPlayInput.BackColor = enabledMenuButtonColor;

            btnMapConf.Enabled = true;
            btnMapConf.BackColor = enabledMenuButtonColor;

            btn_SaveGameRoute.Enabled = true;
            btn_SaveGameRoute.BackColor = enabledPositiveButtonColor;

            txb_gameRoute.Enabled = true;
        }
    }
}
