using AstrophysicalEngine.Model;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AstrophysicalEngine.ViewModel
{
    public class Session
    {
        public RadioobjectEnumerable Radioobjects { get; set; }
        public int AreaRadius { get; set; }
        public string OutputPath { get; set; }

        public event EventHandler<string> Log;

        public Session(RadioobjectEnumerable radioobjects, string outputPath, int areaRadius)
        {
            Radioobjects = radioobjects;
            OutputPath = outputPath;
            AreaRadius = areaRadius;
        }
        public Session(RadioobjectEnumerable radioobjects, int areaRadius) : this(radioobjects, Directory.GetCurrentDirectory() + "\\", areaRadius) { }
        public Session() : this(new RadioobjectEnumerable(), 0) { }

        public async Task DownloadListOfObjects(Coordinates coords, int radius)
        {
            AreaRadius = radius;

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
