using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Astrophysical_Console
{
    public partial class Main : Form
    {
        Label raLabel, decLabel, radLabel;
        TextBox raTextBox, decTextBox, radTextBox;
        Button confirmButton;

        public Main()
        {
            InitializeComponent();
            Shown += new EventHandler(Main_Shown);
        }

        private void Main_Shown(object sender, EventArgs e)
        {

        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void QueryButton_Click(object sender, EventArgs e)
        {
            Query();
        }

        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            Log("Entered");
            Thread.Sleep(1000);
            string ra = raTextBox.Text;
            string dec = decTextBox.Text;
            int radius = int.Parse(radTextBox.Text);
            Coordinates coords = new Coordinates(ra, dec, ' ');

            Log("Query to cats.sao.ru, freq = 1400...");            
            string[] query1400 = DBQuery.Query(coords, 1400, radius);
            MessageBox.Show("Query to cats.sao.ru, freq = 1400...");
            Log("Query to cats.sao.ru, freq = 325...");
            string[] query325 = DBQuery.Query(coords, 325, radius);
            Log("Parsing link, freq = 1400...");
            string[] obj1400 = (string[])DBQuery.HTMLParseLinkToObjects(query1400);
            Log("Parsing link, freq = 325...");
            string[] obj325 = (string[])DBQuery.HTMLParseLinkToObjects(query325);
            Log("Parsing objects...");
            Radioobject[] objects = (Radioobject[])DBQuery.ParseRadioobjects(obj325, obj1400);
            Log("Parsed!");

            foreach (Radioobject obj in objects)
            {
                Log(obj.ToString());
            }
        }

        private void Log(string text) => LogTextBox.Text = text + Environment.NewLine + LogTextBox.Text;

        private void DisableButtons()
        {
            QueryButton.Enabled = false;
        }

        private void ExpandWindow()
        {
            Width += 250;
        }

        private void Query()
        {
            raLabel = new Label();
            raLabel.Width += 10;
            raLabel.Text = "Enter RA of the area: ";
            raLabel.Location = new Point(Width, 10);
            Controls.Add(raLabel);

            raTextBox = new TextBox();
            raTextBox.Location = new Point(Width + raLabel.Width, 10);
            Controls.Add(raTextBox);

            decLabel = new Label();
            decLabel.Width += 10;
            decLabel.Text = "Enter dec of the area: ";
            decLabel.Location = new Point(Width, 40);
            Controls.Add(decLabel);
            
            decTextBox = new TextBox();
            decTextBox.Location = new Point(Width + decLabel.Width, 40);
            Controls.Add(decTextBox);

            radLabel = new Label();
            radLabel.Width += 10;
            radLabel.Text = "Enter rad of the area: ";
            radLabel.Location = new Point(Width, 70);
            Controls.Add(radLabel);

            radTextBox = new TextBox();
            radTextBox.Location = new Point(Width + decLabel.Width, 70);
            Controls.Add(radTextBox);

            confirmButton = new Button();
            confirmButton.Text = "Confirm";
            confirmButton.Location = new Point(Width, 100);
            confirmButton.Size = new Size(decLabel.Width + decTextBox.Width, 30);
            confirmButton.Click += new EventHandler(ConfirmButton_Click);
            Controls.Add(confirmButton);
            
            DisableButtons();
            ExpandWindow();
        }
    }
}
