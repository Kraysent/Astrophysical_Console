using AstrophysicalEngine.Model;
using System;
using System.Windows.Forms;

namespace WinformsUI.View
{
    public partial class QueryDialog : Form
    {
        public Coordinates Coords { get; set; }
        public int Radius { get; set; }
        public bool IsUsed;

        public QueryDialog()
        {
            InitializeComponent();
            IsUsed = false;
        }

        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            Coords = new Coordinates(RATextBox.Text, DecTextBox.Text, ' ');
            Radius = int.Parse(RadiusTextBox.Text);
            IsUsed = true;
            Close();
        }

        private void CancelWindowButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
