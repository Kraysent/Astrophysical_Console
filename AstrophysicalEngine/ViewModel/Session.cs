using AstrophysicalEngine.Model;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AstrophysicalEngine.ViewModel
{
    public class Session
    {
        public RadioobjectEnumerator Radioobjects { get; set; }
        public int AreaRadus { get; set; }
        public string OutputPath { get; set; }

        public event EventHandler<string> Log;

        public Session(RadioobjectEnumerator radioobjects, string outputPath)
        {
            this.Radioobjects = radioobjects;
            OutputPath = outputPath;
        }
        public Session(RadioobjectEnumerator radioobjects)
        {
            this.Radioobjects = radioobjects;
            OutputPath = Directory.GetCurrentDirectory() + "\\";
        }
        public Session()
        {
            Radioobjects = new RadioobjectEnumerator();
            OutputPath = Directory.GetCurrentDirectory() + "\\"; 
        }

        public async Task DownloadListOfObjects(Coordinates coords, int radius)
        {
            try
            {
                await Radioobjects.DownloadObjectsList(coords, radius);
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
        }

        public async Task GetPictures()
        {
            await Radioobjects.DownloadPictures(OutputPath);
        }

        public async Task GetObjectsDensity()
        {
            await Radioobjects.GetDensityRatio(Radioobjects[0].Coords, 15000);
        }
        
        private void ReportToLog(string text)
        {
            Log?.Invoke(this, text);
        }
    }
}
