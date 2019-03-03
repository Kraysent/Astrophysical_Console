using AstrophysicalEngine.Model;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AstrophysicalEngine.ViewModel
{
    //TODO: Time measurement
    
    public class Session
    {
        public RadioobjectEnumerable Radioobjects { get; set; }
        public int AreaRadius { get; set; }
        public string OutputPath { get; set; }

        public event EventHandler<string> OnLog;
        public event EventHandler<string> OnInsertLog;

        public Session(RadioobjectEnumerable radioobjects, string outputPath, int areaRadius)
        {
            Radioobjects = radioobjects;
            OutputPath = outputPath;
            AreaRadius = areaRadius;
            Radioobjects.OnProcessBegin += ProcessBegin;
            Radioobjects.OnProcessEnd += ProcessEnd;
            Radioobjects.OnProcessProgressed += ProcessProgressed; 
        }
        public Session(RadioobjectEnumerable radioobjects, int areaRadius) : this(radioobjects, Directory.GetCurrentDirectory() + "\\", areaRadius) { }
        public Session(string outputPath) : this(new RadioobjectEnumerable(), outputPath, 0) { }
        public Session() : this(new RadioobjectEnumerable(), 0) { }

        //-----------------------------------------------------------------------//

        public async Task DownloadListOfObjects(Coordinates coords, int radius)
        {
            ReportToLog("Downloading list began.");
            AreaRadius = radius;

            try
            {
                await Radioobjects.DownloadObjectsListAsync(coords, radius);
                ReportToLog("Downloading list ended.");
            }
            catch (Exception ex)
            {
                ReportToLog(ex.Message);
            }
        }

        public void ImportObjects(string[] lines, ImportType importType)
        {
            string[] currLine;

            foreach (string line in lines)
            {
                currLine = line.Split(new char[] { (importType == ImportType.ByFlux) ? char.Parse(Radioobject.STANDART_STRING_DELIMETER) : ' ' }, StringSplitOptions.RemoveEmptyEntries);

                switch (importType)
                {
                    case ImportType.ByFlux:
                        Radioobjects.Add(new Radioobject(
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
                        Radioobjects.Add(new Radioobject(
                            coords: new Coordinates(currLine[0], currLine[1], ':'),
                            spectralIndex: double.Parse(currLine[2].Replace('.', ','))
                            ));
                        break;
                }
            }

            ReportToLog("Objects were imported.");
        }

        public async Task GetPictures()
        {
            ReportToLog("Downloading pictures began.");
            await Radioobjects.DownloadPicturesAsync(OutputPath);
            ReportToLog("Downloading pictures ended.");
        }

        public async Task GetObjectsDensity()
        {
            ReportToLog("Getting density of objects began.");
            await Radioobjects.GetDensityRatioAsync(Radioobjects[0].Coords, 15000);
            ReportToLog("Getting density of objects ended.");
        }

        //-----------------------------------------------------------------------//

        private void ReportToLog(string text)
        {
            OnLog?.Invoke(this, text);
        }

        private void InsertToLog(string text)
        {
            OnInsertLog?.Invoke(this, text);
        }

        private void ProcessBegin(object sender, string text)
        {
            ReportToLog($"Process \"{text}\" began.");
            ReportToLog("");
        }

        private void ProcessEnd(object sender, string text)
        {
            ReportToLog($"Process \"{text}\" ended.");
        }

        private void ProcessProgressed(object sender, ProgressEventArgs e)
        {
            InsertToLog($"Progress: {e.Current} out of {e.Maximum}");
        }
    }
}
