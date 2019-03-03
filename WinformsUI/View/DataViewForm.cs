using System;
using System.Data;
using System.Windows.Forms;

namespace WinformsUI.View
{
    public partial class DataViewForm : Form
    {
        public DataViewForm(DataTable data)
        {
            InitializeComponent();
            Resize += DataViewForm_Resize;
            DataGrid.DataSource = data;
        }

        private void DataViewForm_Resize(object sender, EventArgs e)
        {
            DataGrid.Height = Height - 50;
            DataGrid.Width = Width - 50;
        }

        private void DataViewForm_Load(object sender, EventArgs e)
        {

        }
    }
}
