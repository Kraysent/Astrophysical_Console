namespace Astrophysical_Console.View
{
    partial class ObjStructureForm
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
            this.DoneLabel = new System.Windows.Forms.Label();
            this.UndefLabel = new System.Windows.Forms.Label();
            this.FRIILabel = new System.Windows.Forms.Label();
            this.FRILabel = new System.Windows.Forms.Label();
            this.MainPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.MainPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // DoneLabel
            // 
            this.DoneLabel.AutoSize = true;
            this.DoneLabel.Location = new System.Drawing.Point(10, 7);
            this.DoneLabel.Name = "DoneLabel";
            this.DoneLabel.Size = new System.Drawing.Size(36, 13);
            this.DoneLabel.TabIndex = 14;
            this.DoneLabel.Text = "Done:";
            // 
            // UndefLabel
            // 
            this.UndefLabel.AutoSize = true;
            this.UndefLabel.Location = new System.Drawing.Point(120, 334);
            this.UndefLabel.Name = "UndefLabel";
            this.UndefLabel.Size = new System.Drawing.Size(70, 26);
            this.UndefLabel.TabIndex = 13;
            this.UndefLabel.Text = "    DOWN\r\nUNDEFINED";
            // 
            // FRIILabel
            // 
            this.FRIILabel.AutoSize = true;
            this.FRIILabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FRIILabel.Location = new System.Drawing.Point(243, 334);
            this.FRIILabel.Name = "FRIILabel";
            this.FRIILabel.Size = new System.Drawing.Size(69, 20);
            this.FRIILabel.TabIndex = 12;
            this.FRIILabel.Text = "FRII --->";
            // 
            // FRILabel
            // 
            this.FRILabel.AutoSize = true;
            this.FRILabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FRILabel.Location = new System.Drawing.Point(10, 334);
            this.FRILabel.Name = "FRILabel";
            this.FRILabel.Size = new System.Drawing.Size(64, 20);
            this.FRILabel.TabIndex = 11;
            this.FRILabel.Text = "<--- FRI";
            // 
            // MainPictureBox
            // 
            this.MainPictureBox.Location = new System.Drawing.Point(11, 31);
            this.MainPictureBox.Name = "MainPictureBox";
            this.MainPictureBox.Size = new System.Drawing.Size(300, 300);
            this.MainPictureBox.TabIndex = 10;
            this.MainPictureBox.TabStop = false;
            // 
            // ObjStructureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 369);
            this.Controls.Add(this.DoneLabel);
            this.Controls.Add(this.UndefLabel);
            this.Controls.Add(this.FRIILabel);
            this.Controls.Add(this.FRILabel);
            this.Controls.Add(this.MainPictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ObjStructureForm";
            this.Text = "ObjStructureForm";
            this.Load += new System.EventHandler(this.ObjStructureForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.MainPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label DoneLabel;
        private System.Windows.Forms.Label UndefLabel;
        private System.Windows.Forms.Label FRIILabel;
        private System.Windows.Forms.Label FRILabel;
        private System.Windows.Forms.PictureBox MainPictureBox;
    }
}