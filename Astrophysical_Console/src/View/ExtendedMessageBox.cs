using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Astrophysical_Console.View
{
    public partial class ExtendedMessageBox : Form
    {
        private ExtendedMessageBox(Bitmap image, string text)
        {
            InitializeComponent();
            ImagePictureBox.Image = image;
            TextLabel.Text = text;
        }

        private void ExtendedMessageBox_Load(object sender, EventArgs e)
        {

        }

        public static void Show(Bitmap image, string text)
        {
            (new ExtendedMessageBox(image, text)).ShowDialog();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
