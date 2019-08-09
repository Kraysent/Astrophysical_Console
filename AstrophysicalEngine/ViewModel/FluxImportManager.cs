using System;
using AstrophysicalEngine.Model;

namespace AstrophysicalEngine.ViewModel
{
    public class FluxImportManager : IImportManager
    {
        public Radioobject ProcessObject(string line)
        {
            string[] currLine = line.Split(new char[] { char.Parse(Radioobject.STANDART_STRING_DELIMETER) }, StringSplitOptions.RemoveEmptyEntries);

            return new Radioobject(
                                coords: new Coordinates(currLine[0]),
                                fluxOn325: double.Parse(currLine[1]),
                                fluxOn1400: double.Parse(currLine[2]),
                                spectralIndex: double.Parse(currLine[3].Replace('.', ',')),
                                type: Radioobject.ParseType(currLine[4]),
                                densityRatio: double.Parse(currLine[5]),
                                redshift: double.Parse(currLine[6])
                                );
        }
    }
}
