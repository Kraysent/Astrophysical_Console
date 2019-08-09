using System;
using AstrophysicalEngine.Model;

namespace AstrophysicalEngine.ViewModel
{
    public class SpectralIndexImportManager : IImportManager
    {
        public Radioobject ProcessObject(string line)
        {
            string[] currLine = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            return new Radioobject(
                            coords: new Coordinates(currLine[0], currLine[1], ':'),
                            spectralIndex: double.Parse(currLine[2].Replace('.', ','))
                            );
        }
    }
}
