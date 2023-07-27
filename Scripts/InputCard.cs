using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InputKeeperForms.Scripts
{
    public partial class InputCard : Form
    {
        public InputCard(string imageRoute , float timeRecording, float timeHolding, bool pressed, UserController controller)
        {
            InitializeComponent();

            if (controller == UserController.Gamepad)
            {
                pb_ButtonImage.Image = new Bitmap(imageRoute);
                pb_ButtonImage.SizeMode = PictureBoxSizeMode.Zoom;
            }
            else
            {
                lbl_KeyName.Visible = true;
                lbl_KeyName.Text = imageRoute;
            }

            lbl_TimeRecording.Text = Math.Round(timeRecording, 3).ToString();
            lbl_TimeHolding.Text = Math.Round(timeHolding, 3).ToString();
            lbl_Type.Text = pressed ? "Pressed" : "Released";
        }
    }
}
