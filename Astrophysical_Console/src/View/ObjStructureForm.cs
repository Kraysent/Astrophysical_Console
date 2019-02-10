using Astrophysical_Console.Model;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Astrophysical_Console.View
{
    public partial class ObjStructureForm : Form
    {
        public Tuple<Radioobject, Bitmap>[] objImgPairs;
        private int current;

        public ObjStructureForm(Tuple<Radioobject, Bitmap>[] objImgPairs)
        {
            InitializeComponent();
            KeyUp += OnKeyUp;
            current = -1;
            this.objImgPairs = objImgPairs;
        }

        private void ObjStructureForm_Load(object sender, EventArgs e)
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
                MainPictureBox.Image = objImgPairs[current].Item2;
            }
            catch
            {
                Next();
            }

            DoneLabel.Text = "Done: " + (current + 1) + " out of " + objImgPairs.Length;
        }
    }
}
