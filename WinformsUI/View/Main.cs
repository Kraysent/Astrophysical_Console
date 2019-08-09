using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AstrophysicalEngine.Model;
using AstrophysicalEngine.ViewModel;

namespace WinformsUI.View
{
    public partial class Main : Form
    {
        Session _session;

        public Main()
        {
            InitializeComponent();
            _session = new Session();
            _session.OnLog += Log;
            _session.OnInsertLog += InsertLog;
        }
        
        private async void QueryButton_ClickAsync(object sender, EventArgs e)
        {
            QueryDialog query = new QueryDialog();

            query.ShowDialog();

            if (query.IsUsed == true)
            {
                await _session.DownloadListOfObjects(query.Coords, query.Radius);
            }
        }

        private void ImportObjectsButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();

            openFile.ShowDialog();

            if (openFile.FileName == null || openFile.FileName == "")
                return;
            
            _session.ImportObjects(File.ReadAllLines(openFile.FileName), new FluxImportManager());
        }

        private async void GetPicturesButton_ClickAsync(object sender, EventArgs e)
        {
            await _session.GetPictures();
        }

        private async void GetObjectsDensityButton_ClickAsync(object sender, EventArgs e)
        {
            await _session.GetObjectsDensity();
        }

        private void CurrentListButton_Click(object sender, EventArgs e)
        {
            CreateDataTable();
        }

        private void StructureButton_Click(object sender, EventArgs e)
        {
            List<Tuple<Radioobject, Bitmap>> output = new List<Tuple<Radioobject, Bitmap>>();
            
            foreach (Radioobject obj in _session.Radioobjects)
            {
                try
                {
                    Bitmap img = new Bitmap(@"Pictures\" + obj.Coords.ToString() + ".jpg");
                    output.Add(new Tuple<Radioobject, Bitmap>(obj, img));
                }
                catch
                {
                    output.Add(new Tuple<Radioobject, Bitmap>(obj, null));
                }
            }

            ObjectStructureForm objStructure = new ObjectStructureForm(output.ToArray());
            objStructure.ShowDialog();

            output = objStructure.objImgPairs.ToList();

            _session.Radioobjects.Clear();
            _session.Radioobjects.AddRange(output.Select(x => x.Item1));
        }

        private void Log(object sender, string text) => LogTextBox.Invoke(new Action(() => LogTextBox.Text = text + Environment.NewLine + LogTextBox.Text));

        private void InsertLog(object sender, string text)
        {
            string[] output = LogTextBox.Text.Split('\n');
            output[0] = text;
            LogTextBox.Invoke(new Action(() => LogTextBox.Text = string.Join(Environment.NewLine, output)));
        }

        private void CreateDataTable()
        {
            int i;
            DataTable objectsTable = new DataTable();
            DataColumn column;
            DataRow row;

            column = new DataColumn("ID", typeof(string));
            column.Caption = "ID";
            column.ReadOnly = true;
            objectsTable.Columns.Add(column);

            column = new DataColumn("Coordinates", typeof(string));
            column.Caption = "Coordinates";
            column.ReadOnly = true;
            objectsTable.Columns.Add(column);

            column = new DataColumn("Flux on 325", typeof(string));
            column.Caption = "Flux on 325";
            column.ReadOnly = true;
            objectsTable.Columns.Add(column);

            column = new DataColumn("Flux on 1400", typeof(string));
            column.Caption = "Flux on 1400";
            column.ReadOnly = true;
            objectsTable.Columns.Add(column);

            column = new DataColumn("Structure type", typeof(string));
            column.Caption = "Structure type";
            column.ReadOnly = true;
            objectsTable.Columns.Add(column);

            column = new DataColumn("Density ratio", typeof(string));
            column.Caption = "Density ratio";
            column.ReadOnly = true;
            objectsTable.Columns.Add(column);

            column = new DataColumn("Spectral index", typeof(string));
            column.Caption = "Spectral index";
            column.ReadOnly = true;
            objectsTable.Columns.Add(column);

            column = new DataColumn("Redshift", typeof(string));
            column.Caption = "Redshift";
            column.ReadOnly = true;
            objectsTable.Columns.Add(column);

            for (i = 0; i < _session.Radioobjects.Count; i++)
            {
                Radioobject obj = _session.Radioobjects[i];
                row = objectsTable.NewRow();
                row["ID"] = i;
                row["Coordinates"] = obj.Coords.ToString();
                row["Flux on 325"] = obj.FluxOn325.ToString();
                row["Flux on 1400"] = obj.FluxOn1400.ToString();
                row["Structure type"] = obj.Type.ToString();
                row["Density ratio"] = obj.DensityRatio.ToString();
                row["Spectral index"] = obj.SpectralIndex.ToString();
                row["Redshift"] = obj.Redshift.ToString();
                objectsTable.Rows.Add(row);
            }

            DataViewForm form = new DataViewForm(objectsTable);
            form.ShowDialog();
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }
    }
}
