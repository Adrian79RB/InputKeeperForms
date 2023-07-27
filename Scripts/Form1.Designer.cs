
namespace InputKeeperForms
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelSideMenu = new System.Windows.Forms.Panel();
            this.btn_SaveGameRoute = new System.Windows.Forms.Button();
            this.txb_gameRoute = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.nud_mouseSensitivity = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.btnPlayInput = new System.Windows.Forms.Button();
            this.btnSaveInput = new System.Windows.Forms.Button();
            this.btnMapConf = new System.Windows.Forms.Button();
            this.panelSelectController = new System.Windows.Forms.Panel();
            this.cmbControllerType = new System.Windows.Forms.ComboBox();
            this.lblController = new System.Windows.Forms.Label();
            this.pnl_ChildForm = new System.Windows.Forms.Panel();
            this.panelSideMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_mouseSensitivity)).BeginInit();
            this.panelSelectController.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelSideMenu
            // 
            this.panelSideMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(93)))), ((int)(((byte)(98)))));
            this.panelSideMenu.Controls.Add(this.btn_SaveGameRoute);
            this.panelSideMenu.Controls.Add(this.txb_gameRoute);
            this.panelSideMenu.Controls.Add(this.label1);
            this.panelSideMenu.Controls.Add(this.nud_mouseSensitivity);
            this.panelSideMenu.Controls.Add(this.label2);
            this.panelSideMenu.Controls.Add(this.btnPlayInput);
            this.panelSideMenu.Controls.Add(this.btnSaveInput);
            this.panelSideMenu.Controls.Add(this.btnMapConf);
            this.panelSideMenu.Controls.Add(this.panelSelectController);
            this.panelSideMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelSideMenu.Location = new System.Drawing.Point(0, 0);
            this.panelSideMenu.Name = "panelSideMenu";
            this.panelSideMenu.Size = new System.Drawing.Size(250, 511);
            this.panelSideMenu.TabIndex = 0;
            // 
            // btn_SaveGameRoute
            // 
            this.btn_SaveGameRoute.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(172)))), ((int)(((byte)(220)))));
            this.btn_SaveGameRoute.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_SaveGameRoute.Font = new System.Drawing.Font("Liberation Sans", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_SaveGameRoute.Location = new System.Drawing.Point(57, 312);
            this.btn_SaveGameRoute.Name = "btn_SaveGameRoute";
            this.btn_SaveGameRoute.Size = new System.Drawing.Size(103, 24);
            this.btn_SaveGameRoute.TabIndex = 8;
            this.btn_SaveGameRoute.Text = "Save Route";
            this.btn_SaveGameRoute.UseVisualStyleBackColor = false;
            this.btn_SaveGameRoute.Click += new System.EventHandler(this.btn_SaveGameRoute_Click);
            // 
            // txb_gameRoute
            // 
            this.txb_gameRoute.Location = new System.Drawing.Point(113, 286);
            this.txb_gameRoute.Name = "txb_gameRoute";
            this.txb_gameRoute.Size = new System.Drawing.Size(131, 20);
            this.txb_gameRoute.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Liberation Sans", 12F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(3, 289);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "Game Route:";
            // 
            // nud_mouseSensitivity
            // 
            this.nud_mouseSensitivity.DecimalPlaces = 1;
            this.nud_mouseSensitivity.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nud_mouseSensitivity.Location = new System.Drawing.Point(166, 244);
            this.nud_mouseSensitivity.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.nud_mouseSensitivity.Name = "nud_mouseSensitivity";
            this.nud_mouseSensitivity.Size = new System.Drawing.Size(57, 20);
            this.nud_mouseSensitivity.TabIndex = 5;
            this.nud_mouseSensitivity.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nud_mouseSensitivity.ValueChanged += new System.EventHandler(this.nud_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Liberation Sans", 12F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(12, 247);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(148, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Mouse Sensitivity:";
            // 
            // btnPlayInput
            // 
            this.btnPlayInput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(93)))), ((int)(((byte)(146)))), ((int)(((byte)(135)))));
            this.btnPlayInput.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnPlayInput.Enabled = false;
            this.btnPlayInput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlayInput.Font = new System.Drawing.Font("Liberation Sans", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPlayInput.Location = new System.Drawing.Point(0, 156);
            this.btnPlayInput.Name = "btnPlayInput";
            this.btnPlayInput.Size = new System.Drawing.Size(250, 45);
            this.btnPlayInput.TabIndex = 3;
            this.btnPlayInput.Text = "Replay Input";
            this.btnPlayInput.UseVisualStyleBackColor = false;
            this.btnPlayInput.Click += new System.EventHandler(this.btnPlayInput_Click);
            // 
            // btnSaveInput
            // 
            this.btnSaveInput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(93)))), ((int)(((byte)(146)))), ((int)(((byte)(135)))));
            this.btnSaveInput.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSaveInput.Enabled = false;
            this.btnSaveInput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveInput.Font = new System.Drawing.Font("Liberation Sans", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveInput.Location = new System.Drawing.Point(0, 111);
            this.btnSaveInput.Name = "btnSaveInput";
            this.btnSaveInput.Size = new System.Drawing.Size(250, 45);
            this.btnSaveInput.TabIndex = 2;
            this.btnSaveInput.Text = "Save Input";
            this.btnSaveInput.UseVisualStyleBackColor = false;
            this.btnSaveInput.Click += new System.EventHandler(this.btnSaveInput_Click);
            // 
            // btnMapConf
            // 
            this.btnMapConf.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(154)))), ((int)(((byte)(213)))), ((int)(((byte)(214)))));
            this.btnMapConf.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnMapConf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMapConf.Font = new System.Drawing.Font("Liberation Sans", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMapConf.Location = new System.Drawing.Point(0, 66);
            this.btnMapConf.Name = "btnMapConf";
            this.btnMapConf.Size = new System.Drawing.Size(250, 45);
            this.btnMapConf.TabIndex = 1;
            this.btnMapConf.Text = "Mapping Configuration";
            this.btnMapConf.UseVisualStyleBackColor = false;
            this.btnMapConf.Click += new System.EventHandler(this.btnMapConf_Click);
            // 
            // panelSelectController
            // 
            this.panelSelectController.Controls.Add(this.cmbControllerType);
            this.panelSelectController.Controls.Add(this.lblController);
            this.panelSelectController.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSelectController.Location = new System.Drawing.Point(0, 0);
            this.panelSelectController.Name = "panelSelectController";
            this.panelSelectController.Size = new System.Drawing.Size(250, 66);
            this.panelSelectController.TabIndex = 0;
            // 
            // cmbControllerType
            // 
            this.cmbControllerType.FormattingEnabled = true;
            this.cmbControllerType.Location = new System.Drawing.Point(106, 16);
            this.cmbControllerType.Name = "cmbControllerType";
            this.cmbControllerType.Size = new System.Drawing.Size(131, 21);
            this.cmbControllerType.TabIndex = 1;
            this.cmbControllerType.SelectedIndexChanged += new System.EventHandler(this.cmbControllerType_SelectedIndexChanged);
            // 
            // lblController
            // 
            this.lblController.AutoSize = true;
            this.lblController.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblController.Font = new System.Drawing.Font("Liberation Sans", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblController.Location = new System.Drawing.Point(0, 0);
            this.lblController.Name = "lblController";
            this.lblController.Padding = new System.Windows.Forms.Padding(10, 20, 0, 0);
            this.lblController.Size = new System.Drawing.Size(100, 37);
            this.lblController.TabIndex = 0;
            this.lblController.Text = "Controller:";
            // 
            // pnl_ChildForm
            // 
            this.pnl_ChildForm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(154)))), ((int)(((byte)(213)))), ((int)(((byte)(214)))));
            this.pnl_ChildForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_ChildForm.Location = new System.Drawing.Point(250, 0);
            this.pnl_ChildForm.Name = "pnl_ChildForm";
            this.pnl_ChildForm.Size = new System.Drawing.Size(634, 511);
            this.pnl_ChildForm.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 511);
            this.Controls.Add(this.pnl_ChildForm);
            this.Controls.Add(this.panelSideMenu);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(900, 550);
            this.Name = "Form1";
            this.Text = "Input Keeper";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panelSideMenu.ResumeLayout(false);
            this.panelSideMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_mouseSensitivity)).EndInit();
            this.panelSelectController.ResumeLayout(false);
            this.panelSelectController.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelSideMenu;
        private System.Windows.Forms.Panel panelSelectController;
        private System.Windows.Forms.Button btnSaveInput;
        private System.Windows.Forms.Button btnMapConf;
        private System.Windows.Forms.ComboBox cmbControllerType;
        private System.Windows.Forms.Label lblController;
        private System.Windows.Forms.Button btnPlayInput;
        private System.Windows.Forms.Panel pnl_ChildForm;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nud_mouseSensitivity;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txb_gameRoute;
        private System.Windows.Forms.Button btn_SaveGameRoute;
    }
}

