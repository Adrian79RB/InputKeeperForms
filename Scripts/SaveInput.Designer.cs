
namespace InputKeeperForms
{
    partial class SaveInput
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbl_ControllerNotDetected = new System.Windows.Forms.Label();
            this.btn_saveJsonFilesRoute = new System.Windows.Forms.Button();
            this.btn_DeleteFile = new System.Windows.Forms.Button();
            this.txb_jsonFilesRoute = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_StopRecording = new System.Windows.Forms.Button();
            this.btn_StartRecording = new System.Windows.Forms.Button();
            this.pnl_InputDataReceived = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(214)))), ((int)(((byte)(154)))));
            this.panel1.Controls.Add(this.lbl_ControllerNotDetected);
            this.panel1.Controls.Add(this.btn_saveJsonFilesRoute);
            this.panel1.Controls.Add(this.btn_DeleteFile);
            this.panel1.Controls.Add(this.txb_jsonFilesRoute);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btn_StopRecording);
            this.panel1.Controls.Add(this.btn_StartRecording);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(550, 111);
            this.panel1.TabIndex = 0;
            this.panel1.Leave += new System.EventHandler(this.WaitingForClickOutside);
            this.panel1.MouseLeave += new System.EventHandler(this.WaitingForClickOutside);
            // 
            // lbl_ControllerNotDetected
            // 
            this.lbl_ControllerNotDetected.AutoSize = true;
            this.lbl_ControllerNotDetected.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lbl_ControllerNotDetected.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_ControllerNotDetected.ForeColor = System.Drawing.Color.Black;
            this.lbl_ControllerNotDetected.Location = new System.Drawing.Point(246, 45);
            this.lbl_ControllerNotDetected.Name = "lbl_ControllerNotDetected";
            this.lbl_ControllerNotDetected.Size = new System.Drawing.Size(175, 17);
            this.lbl_ControllerNotDetected.TabIndex = 6;
            this.lbl_ControllerNotDetected.Text = "Controller not detected";
            this.lbl_ControllerNotDetected.Visible = false;
            // 
            // btn_saveJsonFilesRoute
            // 
            this.btn_saveJsonFilesRoute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_saveJsonFilesRoute.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(172)))), ((int)(((byte)(220)))));
            this.btn_saveJsonFilesRoute.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_saveJsonFilesRoute.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_saveJsonFilesRoute.Location = new System.Drawing.Point(462, 39);
            this.btn_saveJsonFilesRoute.Name = "btn_saveJsonFilesRoute";
            this.btn_saveJsonFilesRoute.Size = new System.Drawing.Size(87, 23);
            this.btn_saveJsonFilesRoute.TabIndex = 5;
            this.btn_saveJsonFilesRoute.Text = "Save Route";
            this.btn_saveJsonFilesRoute.UseVisualStyleBackColor = false;
            this.btn_saveJsonFilesRoute.Click += new System.EventHandler(this.btn_saveJsonFilesRoute_Click);
            // 
            // btn_DeleteFile
            // 
            this.btn_DeleteFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_DeleteFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(114)))), ((int)(((byte)(94)))));
            this.btn_DeleteFile.Enabled = false;
            this.btn_DeleteFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_DeleteFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_DeleteFile.Location = new System.Drawing.Point(314, 76);
            this.btn_DeleteFile.Name = "btn_DeleteFile";
            this.btn_DeleteFile.Size = new System.Drawing.Size(149, 29);
            this.btn_DeleteFile.TabIndex = 4;
            this.btn_DeleteFile.Text = "Delete Last File";
            this.btn_DeleteFile.UseVisualStyleBackColor = false;
            this.btn_DeleteFile.Click += new System.EventHandler(this.btn_DeleteFile_Click);
            // 
            // txb_jsonFilesRoute
            // 
            this.txb_jsonFilesRoute.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txb_jsonFilesRoute.Location = new System.Drawing.Point(361, 12);
            this.txb_jsonFilesRoute.Name = "txb_jsonFilesRoute";
            this.txb_jsonFilesRoute.Size = new System.Drawing.Size(193, 20);
            this.txb_jsonFilesRoute.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(246, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Input files Route: ";
            // 
            // btn_StopRecording
            // 
            this.btn_StopRecording.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(93)))), ((int)(((byte)(146)))), ((int)(((byte)(135)))));
            this.btn_StopRecording.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn_StopRecording.Enabled = false;
            this.btn_StopRecording.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_StopRecording.Font = new System.Drawing.Font("Liberation Sans", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_StopRecording.Location = new System.Drawing.Point(120, 0);
            this.btn_StopRecording.Name = "btn_StopRecording";
            this.btn_StopRecording.Size = new System.Drawing.Size(120, 111);
            this.btn_StopRecording.TabIndex = 1;
            this.btn_StopRecording.Text = "STOP";
            this.btn_StopRecording.UseVisualStyleBackColor = false;
            this.btn_StopRecording.Click += new System.EventHandler(this.btn_StopRecording_Click);
            // 
            // btn_StartRecording
            // 
            this.btn_StartRecording.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(220)))), ((int)(((byte)(198)))));
            this.btn_StartRecording.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn_StartRecording.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_StartRecording.Font = new System.Drawing.Font("Liberation Sans", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_StartRecording.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btn_StartRecording.Location = new System.Drawing.Point(0, 0);
            this.btn_StartRecording.Name = "btn_StartRecording";
            this.btn_StartRecording.Size = new System.Drawing.Size(120, 111);
            this.btn_StartRecording.TabIndex = 0;
            this.btn_StartRecording.Text = "START";
            this.btn_StartRecording.UseVisualStyleBackColor = false;
            this.btn_StartRecording.Click += new System.EventHandler(this.btn_StartRecording_Click);
            // 
            // pnl_InputDataReceived
            // 
            this.pnl_InputDataReceived.AutoScroll = true;
            this.pnl_InputDataReceived.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(154)))), ((int)(((byte)(213)))), ((int)(((byte)(214)))));
            this.pnl_InputDataReceived.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_InputDataReceived.Location = new System.Drawing.Point(0, 111);
            this.pnl_InputDataReceived.Name = "pnl_InputDataReceived";
            this.pnl_InputDataReceived.Size = new System.Drawing.Size(550, 339);
            this.pnl_InputDataReceived.TabIndex = 1;
            // 
            // SaveInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 450);
            this.Controls.Add(this.pnl_InputDataReceived);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(566, 489);
            this.Name = "SaveInput";
            this.Text = "SaveInput";
            this.Activated += new System.EventHandler(this.WaitingForClickInside);
            this.Deactivate += new System.EventHandler(this.WaitingForClickOutside);
            this.Enter += new System.EventHandler(this.WaitingForClickInside);
            this.Leave += new System.EventHandler(this.WaitingForClickOutside);
            this.MouseEnter += new System.EventHandler(this.WaitingForClickInside);
            this.MouseLeave += new System.EventHandler(this.WaitingForClickOutside);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_StartRecording;
        private System.Windows.Forms.Button btn_StopRecording;
        private System.Windows.Forms.Button btn_DeleteFile;
        private System.Windows.Forms.TextBox txb_jsonFilesRoute;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnl_InputDataReceived;
        private System.Windows.Forms.Button btn_saveJsonFilesRoute;
        private System.Windows.Forms.Label lbl_ControllerNotDetected;
    }
}