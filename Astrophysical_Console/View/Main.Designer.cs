namespace Astrophysical_Console.View
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
            this.LogTextBox = new System.Windows.Forms.TextBox();
            this.QueryButton = new System.Windows.Forms.Button();
            this.processProgressBar = new System.Windows.Forms.ProgressBar();
            this.ExportObjectsButton = new System.Windows.Forms.Button();
            this.ImportObjectsButton = new System.Windows.Forms.Button();
            this.GetPicturesButton = new System.Windows.Forms.Button();
            this.CurrentListButton = new System.Windows.Forms.Button();
            this.GetObjectsDensityButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LogTextBox
            // 
            this.LogTextBox.Location = new System.Drawing.Point(162, 12);
            this.LogTextBox.Multiline = true;
            this.LogTextBox.Name = "LogTextBox";
            this.LogTextBox.ReadOnly = true;
            this.LogTextBox.Size = new System.Drawing.Size(300, 210);
            this.LogTextBox.TabIndex = 0;
            // 
            // QueryButton
            // 
            this.QueryButton.Location = new System.Drawing.Point(12, 12);
            this.QueryButton.Name = "QueryButton";
            this.QueryButton.Size = new System.Drawing.Size(140, 30);
            this.QueryButton.TabIndex = 1;
            this.QueryButton.Text = "Create Query";
            this.QueryButton.UseVisualStyleBackColor = true;
            this.QueryButton.Click += new System.EventHandler(this.QueryButton_Click);
            // 
            // processProgressBar
            // 
            this.processProgressBar.Location = new System.Drawing.Point(12, 228);
            this.processProgressBar.Name = "processProgressBar";
            this.processProgressBar.Size = new System.Drawing.Size(450, 30);
            this.processProgressBar.TabIndex = 2;
            // 
            // ExportObjectsButton
            // 
            this.ExportObjectsButton.Location = new System.Drawing.Point(12, 48);
            this.ExportObjectsButton.Name = "ExportObjectsButton";
            this.ExportObjectsButton.Size = new System.Drawing.Size(140, 30);
            this.ExportObjectsButton.TabIndex = 3;
            this.ExportObjectsButton.Text = "Export objects";
            this.ExportObjectsButton.UseVisualStyleBackColor = true;
            this.ExportObjectsButton.Click += new System.EventHandler(this.ExportObjectsButton_Click);
            // 
            // ImportObjectsButton
            // 
            this.ImportObjectsButton.Location = new System.Drawing.Point(12, 84);
            this.ImportObjectsButton.Name = "ImportObjectsButton";
            this.ImportObjectsButton.Size = new System.Drawing.Size(140, 30);
            this.ImportObjectsButton.TabIndex = 4;
            this.ImportObjectsButton.Text = "Import objects";
            this.ImportObjectsButton.UseVisualStyleBackColor = true;
            this.ImportObjectsButton.Click += new System.EventHandler(this.ImportObjectsButton_Click);
            // 
            // GetPicturesButton
            // 
            this.GetPicturesButton.Location = new System.Drawing.Point(12, 120);
            this.GetPicturesButton.Name = "GetPicturesButton";
            this.GetPicturesButton.Size = new System.Drawing.Size(140, 30);
            this.GetPicturesButton.TabIndex = 5;
            this.GetPicturesButton.Text = "Get pictures";
            this.GetPicturesButton.UseVisualStyleBackColor = true;
            this.GetPicturesButton.Click += new System.EventHandler(this.GetPicturesButton_Click);
            // 
            // CurrentListButton
            // 
            this.CurrentListButton.Location = new System.Drawing.Point(12, 156);
            this.CurrentListButton.Name = "CurrentListButton";
            this.CurrentListButton.Size = new System.Drawing.Size(140, 30);
            this.CurrentListButton.TabIndex = 6;
            this.CurrentListButton.Text = "Show current list";
            this.CurrentListButton.UseVisualStyleBackColor = true;
            this.CurrentListButton.Click += new System.EventHandler(this.CurrentListButton_Click);
            // 
            // GetObjectsDensityButton
            // 
            this.GetObjectsDensityButton.Location = new System.Drawing.Point(12, 192);
            this.GetObjectsDensityButton.Name = "GetObjectsDensityButton";
            this.GetObjectsDensityButton.Size = new System.Drawing.Size(140, 30);
            this.GetObjectsDensityButton.TabIndex = 7;
            this.GetObjectsDensityButton.Text = "Get objects density";
            this.GetObjectsDensityButton.UseVisualStyleBackColor = true;
            this.GetObjectsDensityButton.Click += new System.EventHandler(this.GetObjectsDensityButton_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 268);
            this.Controls.Add(this.GetObjectsDensityButton);
            this.Controls.Add(this.CurrentListButton);
            this.Controls.Add(this.GetPicturesButton);
            this.Controls.Add(this.ImportObjectsButton);
            this.Controls.Add(this.ExportObjectsButton);
            this.Controls.Add(this.processProgressBar);
            this.Controls.Add(this.QueryButton);
            this.Controls.Add(this.LogTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MinimizeBox = false;
            this.Name = "Main";
            this.Text = "Astrophysical Console";
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox LogTextBox;
        private System.Windows.Forms.Button QueryButton;
        private System.Windows.Forms.ProgressBar processProgressBar;
        private System.Windows.Forms.Button ExportObjectsButton;
        private System.Windows.Forms.Button ImportObjectsButton;
        private System.Windows.Forms.Button GetPicturesButton;
        private System.Windows.Forms.Button CurrentListButton;
        private System.Windows.Forms.Button GetObjectsDensityButton;
    }
}

