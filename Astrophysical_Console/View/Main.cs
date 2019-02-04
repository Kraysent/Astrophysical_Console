using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Astrophysical_Console.Model;

namespace Astrophysical_Console.View
{
    public partial class Main : Form
    {
        Label raLabel, decLabel, radLabel;
        TextBox raTextBox, decTextBox, radTextBox;
        Button confirmButton, cancelButton;
        List<Control> mainControls;
        List<Radioobject> currentRadioobjects;

        //---------------------------------------------------------//

        public Main()
        {
            InitializeComponent();
            Shown += new EventHandler(Main_Shown);
        }

        private void Main_Shown(object sender, EventArgs e)
        {
            mainControls = new List<Control>();
            mainControls.Add(QueryButton);
            mainControls.Add(LogTextBox);
            mainControls.Add(processProgressBar);
            mainControls.Add(ExportObjectsButton);
            mainControls.Add(ImportObjectsButton);
            mainControls.Add(GetPicturesButton);
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void QueryButton_Click(object sender, EventArgs e)
        {
            Query();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            EnableButtons();
            NarrowWindowDown();
        }

        private async void ConfirmButton_ClickAsync(object sender, EventArgs e)
        {
            string ra = raTextBox.Text;
            string dec = decTextBox.Text;
            int radius = int.Parse(radTextBox.Text);
            Coordinates coords = new Coordinates(ra, dec, ' ');
            DateTime first;

            Log("Query to cats.sao.ru, freq = 1400...");
            processProgressBar.Style = ProgressBarStyle.Marquee;
            first = DateTime.Now;
            string[] query1400 = await DBQuery.Query(coords, 1400, radius);
            Log("Elapsed time: " + (DateTime.Now - first).TotalMilliseconds + " ms.");
            processProgressBar.Style = ProgressBarStyle.Blocks;

            Log("Query to cats.sao.ru, freq = 325...");
            processProgressBar.Style = ProgressBarStyle.Marquee;
            first = DateTime.Now;
            string[] query325 = await DBQuery.Query(coords, 325, radius);
            Log("Elapsed time: " + (DateTime.Now - first).TotalMilliseconds + " ms.");
            processProgressBar.Style = ProgressBarStyle.Blocks;

            Log("Parsing link, freq = 1400...");
            processProgressBar.Style = ProgressBarStyle.Marquee;
            first = DateTime.Now;
            string[] obj1400 = DBQuery.HTMLParseLinkToObjects(query1400).ToArray();
            Log("Elapsed time: " + (DateTime.Now - first).TotalMilliseconds + " ms.");
            processProgressBar.Style = ProgressBarStyle.Blocks;

            Log("Parsing link, freq = 325...");
            processProgressBar.Style = ProgressBarStyle.Marquee;
            first = DateTime.Now;
            string[] obj325 = DBQuery.HTMLParseLinkToObjects(query325).ToArray();
            Log("Elapsed time: " + (DateTime.Now - first).TotalMilliseconds + " ms.");
            processProgressBar.Style = ProgressBarStyle.Blocks;

            Log("Parsing objects...");
            processProgressBar.Style = ProgressBarStyle.Marquee;
            first = DateTime.Now;
            Radioobject[] objects = DBQuery.ParseRadioobjects(obj325, obj1400).ToArray();
            Log("Elapsed time: " + (DateTime.Now - first).TotalMilliseconds + " ms.");
            processProgressBar.Style = ProgressBarStyle.Blocks;
            Log("Parsed!");

            Log(objects.Length.ToString() + " objects at all.");

            currentRadioobjects = objects.ToList();
            //File.WriteAllLines("objects.csv", objects.Select(x => x.ToString()));
        }

        private void ExportObjectsButton_Click(object sender, EventArgs e)
        {
            string[] objects = currentRadioobjects.Select(x => x.ToLongString("-")).ToArray();

            File.WriteAllLines("objects-" + DateTime.Now.ToString("dd-MM-yyyy") + ".csv", objects);
            Log("Objects were exported.");
        }

        private void ImportObjectsButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            string inputPath;
            string[] contents, currLine;

            currentRadioobjects = new List<Radioobject>();
            fileDialog.ShowDialog();
            inputPath = fileDialog.FileName;
            contents = File.ReadAllLines(inputPath);

            foreach (string line in contents)
            {
                currLine = line.Split('-');
                currentRadioobjects.Add(new Radioobject(currLine[0], currLine[1], new Coordinates(currLine[2]), double.Parse(currLine[3]), double.Parse(currLine[4])));
            }

            Log("Ojects were imported.");
        }

        private void GetPicturesButton_Click(object sender, EventArgs e)
        {
            Directory.CreateDirectory("Pictures");
            Log("Downloading pictures...");
            DBQuery.GetPicture(currentRadioobjects, @"Pictures\");
            Log("All pictures were downloaded.");
        }

        private void CurrentListButton_Click(object sender, EventArgs e)
        {
            CreateDataTable();
        }

        //---------------------------------------------------------//

        private void Log(string text) => LogTextBox.Text = text + Environment.NewLine + LogTextBox.Text;
        
        private void DisableButtons()
        {
            foreach (Control ctrl in mainControls)
                if (ctrl is Button)
                    ctrl.Enabled = false;
        }

        private void EnableButtons()
        {
            foreach (Control ctrl in mainControls)
                if (ctrl is Button)
                    ctrl.Enabled = true;
        }

        private void ExpandWindow()
        {
            Width += 250;
        }

        private void NarrowWindowDown()
        {
            Width -= 250;

            foreach (Control ctrl in Controls)
                ctrl.Hide();

            foreach (Control ctrl in mainControls)
                ctrl.Show();
        }

        private void Query()
        {
            const int labelWidth = 110;

            raLabel = new Label();
            raLabel.Width = labelWidth;
            raLabel.Text = "Enter RA of the area: ";
            raLabel.Location = new Point(Width, 10);
            Controls.Add(raLabel);

            raTextBox = new TextBox();
            raTextBox.Location = new Point(Width + labelWidth, 10);
            Controls.Add(raTextBox);

            decLabel = new Label();
            decLabel.Width = labelWidth;
            decLabel.Text = "Enter dec of the area: ";
            decLabel.Location = new Point(Width, 40);
            Controls.Add(decLabel);
            
            decTextBox = new TextBox();
            decTextBox.Location = new Point(Width + labelWidth, 40);
            Controls.Add(decTextBox);

            radLabel = new Label();
            radLabel.Width = labelWidth;
            radLabel.Text = "Enter rad of the area: ";
            radLabel.Location = new Point(Width, 70);
            Controls.Add(radLabel);

            radTextBox = new TextBox();
            radTextBox.Location = new Point(Width + labelWidth, 70);
            Controls.Add(radTextBox);

            confirmButton = new Button();
            confirmButton.Text = "Confirm";
            confirmButton.Location = new Point(Width, 100);
            confirmButton.Size = new Size(decLabel.Width + decTextBox.Width, 30);
            confirmButton.Click += new EventHandler(ConfirmButton_ClickAsync);
            Controls.Add(confirmButton);

            cancelButton = new Button();
            cancelButton.Text = "Cancel";
            cancelButton.Location = new Point(Width, processProgressBar.Top);
            cancelButton.Size = new Size(decLabel.Width + decTextBox.Width, 30);
            cancelButton.Click += new EventHandler(CancelButton_Click);
            Controls.Add(cancelButton);

            DisableButtons();
            ExpandWindow();
        }
        
        private void CreateDataTable()
        {
            if (currentRadioobjects.Count <= 0)
            {
                Log("No objects in memory.");
            }
        }

    }
}
