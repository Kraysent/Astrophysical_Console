using System;
using System.IO;
using System.Windows.Forms;
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

            _session.ImportObjects(File.ReadAllLines(openFile.FileName), ImportType.ByFlux);
        }

        private void Log(object sender, string text) => LogTextBox.Invoke(new Action(() => LogTextBox.Text = text + Environment.NewLine + LogTextBox.Text));

        private void InsertLog(object sender, string text)
        {
            string[] output = LogTextBox.Text.Split('\n');
            output[0] = text;
            LogTextBox.Invoke(new Action(() => LogTextBox.Text = string.Join(Environment.NewLine, output)));
        }
    }
}
