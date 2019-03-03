using AstrophysicalEngine.Model;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinformsUI.View
{
    public partial class ObjectStructureForm : Form
    {
        public Tuple<Radioobject, Bitmap>[] objImgPairs;
        private int current;

        public ObjectStructureForm(Tuple<Radioobject, Bitmap>[] objImgPairs)
        {
            InitializeComponent();
            KeyUp += OnKeyUp;
            current = -1;
            this.objImgPairs = objImgPairs;
        }

        private void ObjectStructureForm_Load(object sender, EventArgs e)
        {
            Next();
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Right)
            {
                objImgPairs[current].Item1.Type = StructureType.FRII;
                Next();
            }

            if (e.KeyData == Keys.Left)
            {
                objImgPairs[current].Item1.Type = StructureType.FRI;
                Next();
            }

            if (e.KeyData == Keys.Down)
            {
                objImgPairs[current].Item1.Type = StructureType.Undefined;
                Next();
            }
        }

        private void Next()
        {
            if (current >= objImgPairs.Length - 1)
            {
                MessageBox.Show("List ended");
                Close();
                return;
            }

            current++;

            try
            {
                MainPictureBox.Image = FitImage(objImgPairs[current].Item2);
            }
            catch
            {
                Next();
            }

            DoneLabel.Text = "Done: " + (current + 1) + " out of " + objImgPairs.Length;
        }

        private Bitmap FitImage(Bitmap image)
        {
            const int scale = 5; //Number of pixels per real pixel
            Bitmap newImage = new Bitmap(image.Width * scale, image.Height * scale);
            Graphics graph = Graphics.FromImage(newImage);
            int i, j;

            for (i = 0; i < image.Width; i++)
            {
                for (j = 0; j < image.Width; j++)
                {
                    graph.FillRectangle(new SolidBrush(image.GetPixel(i, j)), i * scale, j * scale, scale, scale);
                }
            }

            return newImage;
        }
    }
}
