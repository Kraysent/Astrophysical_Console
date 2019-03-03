namespace WinformsUI.View
{
    partial class QueryDialog
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
            this.RALabel = new System.Windows.Forms.Label();
            this.DecLabel = new System.Windows.Forms.Label();
            this.RadLabel = new System.Windows.Forms.Label();
            this.ConfirmButton = new System.Windows.Forms.Button();
            this.CancelWindowButton = new System.Windows.Forms.Button();
            this.RATextBox = new System.Windows.Forms.TextBox();
            this.DecTextBox = new System.Windows.Forms.TextBox();
            this.RadiusTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // RALabel
            // 
            this.RALabel.AutoSize = true;
            this.RALabel.Location = new System.Drawing.Point(12, 12);
            this.RALabel.Name = "RALabel";
            this.RALabel.Size = new System.Drawing.Size(163, 13);
            this.RALabel.TabIndex = 0;
            this.RALabel.Text = "Enter right ascension of the area:";
            // 
            // DecLabel
            // 
            this.DecLabel.AutoSize = true;
            this.DecLabel.Location = new System.Drawing.Point(12, 35);
            this.DecLabel.Name = "DecLabel";
            this.DecLabel.Size = new System.Drawing.Size(126, 13);
            this.DecLabel.TabIndex = 1;
            this.DecLabel.Text = "Enter decline of the area:";
            // 
            // RadLabel
            // 
            this.RadLabel.AutoSize = true;
            this.RadLabel.Location = new System.Drawing.Point(10, 61);
            this.RadLabel.Name = "RadLabel";
            this.RadLabel.Size = new System.Drawing.Size(120, 13);
            this.RadLabel.TabIndex = 2;
            this.RadLabel.Text = "Enter radius of the area:";
            // 
            // ConfirmButton
            // 
            this.ConfirmButton.Location = new System.Drawing.Point(13, 84);
            this.ConfirmButton.Name = "ConfirmButton";
            this.ConfirmButton.Size = new System.Drawing.Size(297, 32);
            this.ConfirmButton.TabIndex = 3;
            this.ConfirmButton.Text = "Confirm";
            this.ConfirmButton.UseVisualStyleBackColor = true;
            this.ConfirmButton.Click += new System.EventHandler(this.ConfirmButton_Click);
            // 
            // CancelWindowButton
            // 
            this.CancelWindowButton.Location = new System.Drawing.Point(15, 122);
            this.CancelWindowButton.Name = "CancelWindowButton";
            this.CancelWindowButton.Size = new System.Drawing.Size(297, 32);
            this.CancelWindowButton.TabIndex = 4;
            this.CancelWindowButton.Text = "Cancel";
            this.CancelWindowButton.UseVisualStyleBackColor = true;
            this.CancelWindowButton.Click += new System.EventHandler(this.CancelWindowButton_Click);
            // 
            // RATextBox
            // 
            this.RATextBox.Location = new System.Drawing.Point(182, 9);
            this.RATextBox.Name = "RATextBox";
            this.RATextBox.Size = new System.Drawing.Size(130, 20);
            this.RATextBox.TabIndex = 5;
            // 
            // DecTextBox
            // 
            this.DecTextBox.Location = new System.Drawing.Point(182, 32);
            this.DecTextBox.Name = "DecTextBox";
            this.DecTextBox.Size = new System.Drawing.Size(130, 20);
            this.DecTextBox.TabIndex = 6;
            // 
            // RadiusTextBox
            // 
            this.RadiusTextBox.Location = new System.Drawing.Point(182, 58);
            this.RadiusTextBox.Name = "RadiusTextBox";
            this.RadiusTextBox.Size = new System.Drawing.Size(130, 20);
            this.RadiusTextBox.TabIndex = 7;
            // 
            // QueryDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 164);
            this.Controls.Add(this.RadiusTextBox);
            this.Controls.Add(this.DecTextBox);
            this.Controls.Add(this.RATextBox);
            this.Controls.Add(this.CancelWindowButton);
            this.Controls.Add(this.ConfirmButton);
            this.Controls.Add(this.RadLabel);
            this.Controls.Add(this.DecLabel);
            this.Controls.Add(this.RALabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "QueryDialog";
            this.Text = " Query Dialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label RALabel;
        private System.Windows.Forms.Label DecLabel;
        private System.Windows.Forms.Label RadLabel;
        private System.Windows.Forms.Button ConfirmButton;
        private System.Windows.Forms.Button CancelWindowButton;
        private System.Windows.Forms.TextBox RATextBox;
        private System.Windows.Forms.TextBox DecTextBox;
        private System.Windows.Forms.TextBox RadiusTextBox;
    }
}