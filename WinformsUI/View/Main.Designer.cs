namespace WinformsUI.View
{
    partial class Main
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.StructureButton = new System.Windows.Forms.Button();
            this.GetObjectsDensityButton = new System.Windows.Forms.Button();
            this.CurrentListButton = new System.Windows.Forms.Button();
            this.GetPicturesButton = new System.Windows.Forms.Button();
            this.ImportObjectsButton = new System.Windows.Forms.Button();
            this.ProcessProgressBar = new System.Windows.Forms.ProgressBar();
            this.QueryButton = new System.Windows.Forms.Button();
            this.LogTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // StructureButton
            // 
            this.StructureButton.Location = new System.Drawing.Point(13, 192);
            this.StructureButton.Name = "StructureButton";
            this.StructureButton.Size = new System.Drawing.Size(140, 30);
            this.StructureButton.TabIndex = 16;
            this.StructureButton.Text = "Open structure form";
            this.StructureButton.UseVisualStyleBackColor = true;
            // 
            // GetObjectsDensityButton
            // 
            this.GetObjectsDensityButton.Location = new System.Drawing.Point(13, 156);
            this.GetObjectsDensityButton.Name = "GetObjectsDensityButton";
            this.GetObjectsDensityButton.Size = new System.Drawing.Size(140, 30);
            this.GetObjectsDensityButton.TabIndex = 15;
            this.GetObjectsDensityButton.Text = "Get objects density";
            this.GetObjectsDensityButton.UseVisualStyleBackColor = true;
            // 
            // CurrentListButton
            // 
            this.CurrentListButton.Location = new System.Drawing.Point(13, 120);
            this.CurrentListButton.Name = "CurrentListButton";
            this.CurrentListButton.Size = new System.Drawing.Size(140, 30);
            this.CurrentListButton.TabIndex = 14;
            this.CurrentListButton.Text = "Show current list";
            this.CurrentListButton.UseVisualStyleBackColor = true;
            // 
            // GetPicturesButton
            // 
            this.GetPicturesButton.Location = new System.Drawing.Point(13, 84);
            this.GetPicturesButton.Name = "GetPicturesButton";
            this.GetPicturesButton.Size = new System.Drawing.Size(140, 30);
            this.GetPicturesButton.TabIndex = 13;
            this.GetPicturesButton.Text = "Get pictures";
            this.GetPicturesButton.UseVisualStyleBackColor = true;
            // 
            // ImportObjectsButton
            // 
            this.ImportObjectsButton.Location = new System.Drawing.Point(13, 48);
            this.ImportObjectsButton.Name = "ImportObjectsButton";
            this.ImportObjectsButton.Size = new System.Drawing.Size(140, 30);
            this.ImportObjectsButton.TabIndex = 12;
            this.ImportObjectsButton.Text = "Import objects";
            this.ImportObjectsButton.UseVisualStyleBackColor = true;
            this.ImportObjectsButton.Click += new System.EventHandler(this.ImportObjectsButton_Click);
            // 
            // ProcessProgressBar
            // 
            this.ProcessProgressBar.Location = new System.Drawing.Point(13, 228);
            this.ProcessProgressBar.Name = "ProcessProgressBar";
            this.ProcessProgressBar.Size = new System.Drawing.Size(450, 30);
            this.ProcessProgressBar.TabIndex = 11;
            // 
            // QueryButton
            // 
            this.QueryButton.Location = new System.Drawing.Point(13, 12);
            this.QueryButton.Name = "QueryButton";
            this.QueryButton.Size = new System.Drawing.Size(140, 30);
            this.QueryButton.TabIndex = 10;
            this.QueryButton.Text = "Create Query";
            this.QueryButton.UseVisualStyleBackColor = true;
            this.QueryButton.Click += new System.EventHandler(this.QueryButton_ClickAsync);
            // 
            // LogTextBox
            // 
            this.LogTextBox.Location = new System.Drawing.Point(163, 12);
            this.LogTextBox.Multiline = true;
            this.LogTextBox.Name = "LogTextBox";
            this.LogTextBox.ReadOnly = true;
            this.LogTextBox.Size = new System.Drawing.Size(300, 210);
            this.LogTextBox.TabIndex = 9;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 267);
            this.Controls.Add(this.StructureButton);
            this.Controls.Add(this.GetObjectsDensityButton);
            this.Controls.Add(this.CurrentListButton);
            this.Controls.Add(this.GetPicturesButton);
            this.Controls.Add(this.ImportObjectsButton);
            this.Controls.Add(this.ProcessProgressBar);
            this.Controls.Add(this.QueryButton);
            this.Controls.Add(this.LogTextBox);
            this.Name = "Main";
            this.Text = "Astrophysical Console";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StructureButton;
        private System.Windows.Forms.Button GetObjectsDensityButton;
        private System.Windows.Forms.Button CurrentListButton;
        private System.Windows.Forms.Button GetPicturesButton;
        private System.Windows.Forms.Button ImportObjectsButton;
        private System.Windows.Forms.ProgressBar ProcessProgressBar;
        private System.Windows.Forms.Button QueryButton;
        private System.Windows.Forms.TextBox LogTextBox;
    }
}

