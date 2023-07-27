
namespace InputKeeperForms
{
    partial class ReplayInput
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
            this.nl_Controls = new System.Windows.Forms.Panel();
            this.btn_SelectedInputFile = new System.Windows.Forms.Button();
            this.btn_DeleteInputFile = new System.Windows.Forms.Button();
            this.cmb_SelectedInputFile = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_StopPlaying = new System.Windows.Forms.Button();
            this.btn_PlayInput = new System.Windows.Forms.Button();
            this.pnl_InputShower = new System.Windows.Forms.Panel();
            this.nl_Controls.SuspendLayout();
            this.SuspendLayout();
            // 
            // nl_Controls
            // 
            this.nl_Controls.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(214)))), ((int)(((byte)(154)))));
            this.nl_Controls.Controls.Add(this.btn_SelectedInputFile);
            this.nl_Controls.Controls.Add(this.btn_DeleteInputFile);
            this.nl_Controls.Controls.Add(this.cmb_SelectedInputFile);
            this.nl_Controls.Controls.Add(this.label1);
            this.nl_Controls.Controls.Add(this.btn_StopPlaying);
            this.nl_Controls.Controls.Add(this.btn_PlayInput);
            this.nl_Controls.Dock = System.Windows.Forms.DockStyle.Top;
            this.nl_Controls.Location = new System.Drawing.Point(0, 0);
            this.nl_Controls.Name = "nl_Controls";
            this.nl_Controls.Size = new System.Drawing.Size(566, 111);
            this.nl_Controls.TabIndex = 0;
            // 
            // btn_SelectedInputFile
            // 
            this.btn_SelectedInputFile.AutoSize = true;
            this.btn_SelectedInputFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(172)))), ((int)(((byte)(220)))));
            this.btn_SelectedInputFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_SelectedInputFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_SelectedInputFile.Location = new System.Drawing.Point(248, 63);
            this.btn_SelectedInputFile.Name = "btn_SelectedInputFile";
            this.btn_SelectedInputFile.Size = new System.Drawing.Size(150, 30);
            this.btn_SelectedInputFile.TabIndex = 5;
            this.btn_SelectedInputFile.Text = "Select Input File";
            this.btn_SelectedInputFile.UseVisualStyleBackColor = false;
            this.btn_SelectedInputFile.Click += new System.EventHandler(this.btn_SelectedInputFile_Click);
            // 
            // btn_DeleteInputFile
            // 
            this.btn_DeleteInputFile.AutoSize = true;
            this.btn_DeleteInputFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(114)))), ((int)(((byte)(94)))));
            this.btn_DeleteInputFile.Enabled = false;
            this.btn_DeleteInputFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_DeleteInputFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_DeleteInputFile.Location = new System.Drawing.Point(404, 63);
            this.btn_DeleteInputFile.Name = "btn_DeleteInputFile";
            this.btn_DeleteInputFile.Size = new System.Drawing.Size(150, 30);
            this.btn_DeleteInputFile.TabIndex = 4;
            this.btn_DeleteInputFile.Text = "Delete Input File";
            this.btn_DeleteInputFile.UseVisualStyleBackColor = false;
            this.btn_DeleteInputFile.Click += new System.EventHandler(this.btn_DeleteInputFile_Click);
            // 
            // cmb_SelectedInputFile
            // 
            this.cmb_SelectedInputFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmb_SelectedInputFile.FormattingEnabled = true;
            this.cmb_SelectedInputFile.Location = new System.Drawing.Point(368, 23);
            this.cmb_SelectedInputFile.Name = "cmb_SelectedInputFile";
            this.cmb_SelectedInputFile.Size = new System.Drawing.Size(186, 21);
            this.cmb_SelectedInputFile.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(259, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Selected Input:";
            // 
            // btn_StopPlaying
            // 
            this.btn_StopPlaying.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(93)))), ((int)(((byte)(146)))), ((int)(((byte)(135)))));
            this.btn_StopPlaying.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn_StopPlaying.Enabled = false;
            this.btn_StopPlaying.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_StopPlaying.Font = new System.Drawing.Font("Liberation Sans", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_StopPlaying.Location = new System.Drawing.Point(120, 0);
            this.btn_StopPlaying.Name = "btn_StopPlaying";
            this.btn_StopPlaying.Size = new System.Drawing.Size(120, 111);
            this.btn_StopPlaying.TabIndex = 1;
            this.btn_StopPlaying.Text = "STOP";
            this.btn_StopPlaying.UseVisualStyleBackColor = false;
            this.btn_StopPlaying.Click += new System.EventHandler(this.btn_StopPlaying_Click);
            // 
            // btn_PlayInput
            // 
            this.btn_PlayInput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(93)))), ((int)(((byte)(146)))), ((int)(((byte)(135)))));
            this.btn_PlayInput.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn_PlayInput.Enabled = false;
            this.btn_PlayInput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_PlayInput.Font = new System.Drawing.Font("Liberation Sans", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_PlayInput.Location = new System.Drawing.Point(0, 0);
            this.btn_PlayInput.Name = "btn_PlayInput";
            this.btn_PlayInput.Size = new System.Drawing.Size(120, 111);
            this.btn_PlayInput.TabIndex = 0;
            this.btn_PlayInput.Text = "PLAY";
            this.btn_PlayInput.UseVisualStyleBackColor = false;
            this.btn_PlayInput.Click += new System.EventHandler(this.btn_PlayInput_Click);
            // 
            // pnl_InputShower
            // 
            this.pnl_InputShower.AutoScroll = true;
            this.pnl_InputShower.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(154)))), ((int)(((byte)(213)))), ((int)(((byte)(214)))));
            this.pnl_InputShower.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_InputShower.Location = new System.Drawing.Point(0, 111);
            this.pnl_InputShower.Name = "pnl_InputShower";
            this.pnl_InputShower.Size = new System.Drawing.Size(566, 378);
            this.pnl_InputShower.TabIndex = 1;
            // 
            // ReplayInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 489);
            this.Controls.Add(this.pnl_InputShower);
            this.Controls.Add(this.nl_Controls);
            this.Name = "ReplayInput";
            this.Text = "ReplayInput";
            this.nl_Controls.ResumeLayout(false);
            this.nl_Controls.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel nl_Controls;
        private System.Windows.Forms.Button btn_StopPlaying;
        private System.Windows.Forms.Button btn_PlayInput;
        private System.Windows.Forms.Panel pnl_InputShower;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmb_SelectedInputFile;
        private System.Windows.Forms.Button btn_DeleteInputFile;
        private System.Windows.Forms.Button btn_SelectedInputFile;
    }
}