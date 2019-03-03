namespace WinformsUI.View
{
    partial class ObjectStructureForm
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
            this.DoneLabel.Location = new System.Drawing.Point(11, 9);
            this.DoneLabel.Name = "DoneLabel";
            this.DoneLabel.Size = new System.Drawing.Size(36, 13);
            this.DoneLabel.TabIndex = 19;
            this.DoneLabel.Text = "Done:";
            // 
            // UndefLabel
            // 
            this.UndefLabel.AutoSize = true;
            this.UndefLabel.Location = new System.Drawing.Point(121, 336);
            this.UndefLabel.Name = "UndefLabel";
            this.UndefLabel.Size = new System.Drawing.Size(70, 26);
            this.UndefLabel.TabIndex = 18;
            this.UndefLabel.Text = "    DOWN\r\nUNDEFINED";
            // 
            // FRIILabel
            // 
            this.FRIILabel.AutoSize = true;
            this.FRIILabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FRIILabel.Location = new System.Drawing.Point(244, 336);
            this.FRIILabel.Name = "FRIILabel";
            this.FRIILabel.Size = new System.Drawing.Size(69, 20);
            this.FRIILabel.TabIndex = 17;
            this.FRIILabel.Text = "FRII --->";
            // 
            // FRILabel
            // 
            this.FRILabel.AutoSize = true;
            this.FRILabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FRILabel.Location = new System.Drawing.Point(11, 336);
            this.FRILabel.Name = "FRILabel";
            this.FRILabel.Size = new System.Drawing.Size(64, 20);
            this.FRILabel.TabIndex = 16;
            this.FRILabel.Text = "<--- FRI";
            // 
            // MainPictureBox
            // 
            this.MainPictureBox.Location = new System.Drawing.Point(12, 33);
            this.MainPictureBox.Name = "MainPictureBox";
            this.MainPictureBox.Size = new System.Drawing.Size(300, 300);
            this.MainPictureBox.TabIndex = 15;
            this.MainPictureBox.TabStop = false;
            // 
            // ObjectStructureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(325, 367);
            this.Controls.Add(this.DoneLabel);
            this.Controls.Add(this.UndefLabel);
            this.Controls.Add(this.FRIILabel);
            this.Controls.Add(this.FRILabel);
            this.Controls.Add(this.MainPictureBox);
            this.Name = "ObjectStructureForm";
            this.Text = "ObjectStructureForm";
            this.Load += new System.EventHandler(this.ObjectStructureForm_Load);
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