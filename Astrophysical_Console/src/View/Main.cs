using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Astrophysical_Console.Model;
using AstrophysicalEngine.ViewModel;

namespace Astrophysical_Console.View
{
    public partial class Main : Form
    {
        Label raLabel, decLabel, radLabel;
        TextBox raTextBox, decTextBox, radTextBox;
        Button confirmButton, cancelButton;
        List<Control> mainControls;
        List<Radioobject> currentRadioobjects;
        int ListLength => currentRadioobjects.Count;
        public ImportType importType;
        private Session _session;

        public delegate void ListChangedHandler();
        public event ListChangedHandler ListChanged;

        //---------------------------------------------------------//

        public Main()
        {
            InitializeComponent();
            Shown += new EventHandler(Main_Shown);
            DBQuery.Progress += InsertCounter;
            ListChanged += SaveObjList;
            importType = ImportType.ByFlux;
            _session = new Session();
        }

        private void Main_Shown(object sender, EventArgs e)
        {
            mainControls = new List<Control>
            {
                QueryButton,
                LogTextBox,
                processProgressBar,
                ImportObjectsButton,
                GetPicturesButton,
                CurrentListButton,
                GetObjectsDensityButton,
                StructureButton
            };
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void QueryButton_Click(object sender, EventArgs e)
        {
            CreateQueryControls();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            EnableButtons();
            NarrowWindowDown();
        }
        
        private void ImportObjectsButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            string inputPath;
            string[] contents, currLine;

            currentRadioobjects = new List<Radioobject>();
            fileDialog.ShowDialog();
            inputPath = fileDialog.FileName;

            if (string.IsNullOrEmpty(inputPath) == true)
                return;

            contents = File.ReadAllLines(inputPath);
            
            foreach (string line in contents)
            {
                currLine = line.Split(new char[] { (importType == ImportType.ByFlux) ? char.Parse(Radioobject.STANDART_STRING_DELIMETER) : ' ' }, StringSplitOptions.RemoveEmptyEntries);

                switch (importType)
                {
                    case ImportType.ByFlux:
                        currentRadioobjects.Add(new Radioobject(
                                coords: new Coordinates(currLine[0]),
                                fluxOn325: double.Parse(currLine[1]),
                                fluxOn1400: double.Parse(currLine[2]),
                                spectralIndex: double.Parse(currLine[3].Replace('.', ',')),
                                type: Radioobject.ParseType(currLine[4]),
                                densityRatio: double.Parse(currLine[5]),
                                redshift: double.Parse(currLine[6])
                                ));
                        break;
                    case ImportType.BySpectralIndex:
                        currentRadioobjects.Add(new Radioobject(
                            coords: new Coordinates(currLine[0], currLine[1], ':'),
                            spectralIndex: double.Parse(currLine[2].Replace('.', ','))
                            ));
                        break;
                }
            }

            Log("Objects were imported.");
        }
        
        private void CurrentListButton_Click(object sender, EventArgs e)
        {
            if (ListIsEmptyOrNull() == true)
                return;

            CreateDataTable();
        }

        private void StructureButton_Click(object sender, EventArgs e)
        {
            if (ListIsEmptyOrNull() == true)
                return;

            List<Tuple<Radioobject, Bitmap>> output = new List<Tuple<Radioobject, Bitmap>>();

            if (Directory.Exists("Pictures") == false)
            {
                Log("No pictures downloaded.");
                return;
            }

            foreach (Radioobject obj in currentRadioobjects)
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

            ObjStructureForm objStructure = new ObjStructureForm(output.ToArray());
            objStructure.ShowDialog();

            output = objStructure.objImgPairs.ToList();

            currentRadioobjects = output.Select(x => x.Item1).ToList();
            ListChanged();
        }

        private async void GetPicturesButton_ClickAsync(object sender, EventArgs e)
        {
            if (ListIsEmptyOrNull() == true)
                return;
            
            Log("Downloading pictures...");
            int objListLength = currentRadioobjects.Count;

            Directory.CreateDirectory("Pictures");
            GetPicturesButton.Enabled = false;

            await DBQuery.GetPicture(currentRadioobjects, Directory.GetCurrentDirectory() + "\\Pictures");

            GetPicturesButton.Enabled = true;
            processProgressBar.Value = 0;
            Log("All pictures were downloaded.");
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
            string query1400 = await DBQuery.Query(coords, 1400, radius);
            Log("Elapsed time: " + (DateTime.Now - first).TotalMilliseconds + " ms.");
            processProgressBar.Style = ProgressBarStyle.Blocks;

            Log("Query to cats.sao.ru, freq = 325...");
            processProgressBar.Style = ProgressBarStyle.Marquee;
            first = DateTime.Now;
            string query325 = await DBQuery.Query(coords, 325, radius);
            Log("Elapsed time: " + (DateTime.Now - first).TotalMilliseconds + " ms.");
            processProgressBar.Style = ProgressBarStyle.Blocks;

            Log("Parsing link, freq = 1400...");
            processProgressBar.Style = ProgressBarStyle.Marquee;
            first = DateTime.Now;
            string[] obj1400 = (await DBQuery.HTMLParseLinkToObjects(query1400)).ToArray();
            Log("Elapsed time: " + (DateTime.Now - first).TotalMilliseconds + " ms.");
            processProgressBar.Style = ProgressBarStyle.Blocks;

            Log("Parsing link, freq = 325...");
            processProgressBar.Style = ProgressBarStyle.Marquee;
            first = DateTime.Now;
            string[] obj325 = (await DBQuery.HTMLParseLinkToObjects(query325)).ToArray();
            Log("Elapsed time: " + (DateTime.Now - first).TotalMilliseconds + " ms.");
            processProgressBar.Style = ProgressBarStyle.Blocks;

            Log("Parsing objects...");
            processProgressBar.Style = ProgressBarStyle.Marquee;
            first = DateTime.Now;
            Radioobject[] objects = (await DBQuery.ParseRadioobjects(obj325, obj1400)).ToArray();
            Log("Elapsed time: " + (DateTime.Now - first).TotalMilliseconds + " ms.");
            processProgressBar.Style = ProgressBarStyle.Blocks;

            Log(objects.Length.ToString() + " objects at all.");

            currentRadioobjects = objects.ToList();

            ListChanged();
        }

        private async void GetObjectsDensityButton_ClickAsync(object sender, EventArgs e)
        {
            if (ListIsEmptyOrNull() == true)
                return;
            
            try
            {
                currentRadioobjects = (await DBQuery.GetDensityRatio(currentRadioobjects.Where(x => x.Type == StructureType.FRII).ToList(), currentRadioobjects[0].Coords, 15000)).ToList();
            }
            catch (Exception e1)
            {
                Log(e1.Message);
                //Log("Server error, try again.");
            }

            ListChanged();
        }

        //---------------------------------------------------------//

        private void Log(string text) => LogTextBox.Invoke(new Action(() => LogTextBox.Text = text + Environment.NewLine + LogTextBox.Text));

        private void InsertLog(string text)
        {
            string[] output = LogTextBox.Text.Split('\n');
            output[0] = text;
            LogTextBox.Invoke(new Action(() => LogTextBox.Text = string.Join(Environment.NewLine, output)));
        }

        private void InsertCounter(string process, int curr, int length)
        {
            curr++;
            processProgressBar.Invoke(new Action(() => processProgressBar.Value = (int)(curr / (double)length * 100)));
            InsertLog(process + ": " + curr + " out of " + length);
        }

        private void SaveObjList()
        {
            if (ListIsEmptyOrNull() == true)
                return;

            string[] objects = currentRadioobjects.Select(x => x.ToLongString("|")).ToArray();
            string fileName = "objects-" + DateTime.Now.ToString("dd-MM-yyyy") + ".csv";

            File.WriteAllLines(fileName, objects);
            Log("Objects were saved to \\" + fileName + ".");
        }

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

        private void CreateQueryControls()
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

            for (i = 0; i < currentRadioobjects.Count; i++)
            {
                Radioobject obj = currentRadioobjects[i];
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

        private bool ListIsEmptyOrNull()
        {
            if (currentRadioobjects == null || currentRadioobjects.Count == 0)
            {
                Log("List of objects is empty.");
                return true;
            }
            else
                return false;
        }
    }
}
