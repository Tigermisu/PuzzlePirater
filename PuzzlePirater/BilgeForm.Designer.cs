namespace PuzzlePirater {
    partial class BilgeForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.lblStatusHeader = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.picturePreview = new System.Windows.Forms.PictureBox();
            this.btnToggleStatus = new System.Windows.Forms.Button();
            this.lblPreview = new System.Windows.Forms.Label();
            this.lblRawOutput = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picturePreview)).BeginInit();
            this.SuspendLayout();
            // 
            // lblStatusHeader
            // 
            this.lblStatusHeader.AutoSize = true;
            this.lblStatusHeader.Location = new System.Drawing.Point(12, 12);
            this.lblStatusHeader.Name = "lblStatusHeader";
            this.lblStatusHeader.Size = new System.Drawing.Size(43, 13);
            this.lblStatusHeader.TabIndex = 0;
            this.lblStatusHeader.Text = "Status: ";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblStatus.Location = new System.Drawing.Point(48, 12);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(48, 13);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "Disabled";
            // 
            // picturePreview
            // 
            this.picturePreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picturePreview.Location = new System.Drawing.Point(214, 29);
            this.picturePreview.Name = "picturePreview";
            this.picturePreview.Size = new System.Drawing.Size(272, 542);
            this.picturePreview.TabIndex = 2;
            this.picturePreview.TabStop = false;
            // 
            // btnToggleStatus
            // 
            this.btnToggleStatus.Location = new System.Drawing.Point(12, 40);
            this.btnToggleStatus.Name = "btnToggleStatus";
            this.btnToggleStatus.Size = new System.Drawing.Size(75, 23);
            this.btnToggleStatus.TabIndex = 3;
            this.btnToggleStatus.Text = "Start";
            this.btnToggleStatus.UseVisualStyleBackColor = true;
            this.btnToggleStatus.Click += new System.EventHandler(this.btnToggleStatus_Click);
            // 
            // lblPreview
            // 
            this.lblPreview.AutoSize = true;
            this.lblPreview.Location = new System.Drawing.Point(214, 10);
            this.lblPreview.Name = "lblPreview";
            this.lblPreview.Size = new System.Drawing.Size(45, 13);
            this.lblPreview.TabIndex = 4;
            this.lblPreview.Text = "Preview";
            // 
            // lblRawOutput
            // 
            this.lblRawOutput.AutoSize = true;
            this.lblRawOutput.Location = new System.Drawing.Point(15, 98);
            this.lblRawOutput.Name = "lblRawOutput";
            this.lblRawOutput.Size = new System.Drawing.Size(0, 13);
            this.lblRawOutput.TabIndex = 5;
            // 
            // BilgeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 583);
            this.Controls.Add(this.lblRawOutput);
            this.Controls.Add(this.lblPreview);
            this.Controls.Add(this.btnToggleStatus);
            this.Controls.Add(this.picturePreview);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblStatusHeader);
            this.Name = "BilgeForm";
            this.Text = "Puzzle Pirater - Bilge";
            ((System.ComponentModel.ISupportInitialize)(this.picturePreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblStatusHeader;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.PictureBox picturePreview;
        private System.Windows.Forms.Button btnToggleStatus;
        private System.Windows.Forms.Label lblPreview;
        private System.Windows.Forms.Label lblRawOutput;
    }
}

