using static System.Math;

namespace AstrophysicalEngine.Model
{
    public class Radioobject
    {
        public const string STANDART_STRING_DELIMETER = "|";

        public Coordinates Coords { get; set;  }
        public double FluxOn325 { get; set; }
        public double FluxOn1400 { get; set; }
        public double SpectralIndex { get; set; }
        public StructureType Type { get; set; }
        public double DensityRatio { get; set; }
        public double Redshift { get; set; }

        public Radioobject(Coordinates coords, double fluxOn325, double fluxOn1400)
        {
            Coords = coords;
            FluxOn325 = fluxOn325;
            FluxOn1400 = fluxOn1400;
            SpectralIndex = (Log10(fluxOn1400) - Log10(FluxOn325)) / (Log10(1400) - Log10(325));
        }

        public Radioobject(Coordinates coords, double spectralIndex)
        {
            Coords = coords;
            SpectralIndex = spectralIndex;
        }

        public Radioobject(Coordinates coords, double fluxOn325, double fluxOn1400, double spectralIndex, StructureType type, double densityRatio, double redshift)
        {
            Coords = coords;
            FluxOn325 = fluxOn325;
            FluxOn1400 = fluxOn1400;
            SpectralIndex = spectralIndex;
            Type = type;
            DensityRatio = densityRatio;
            Redshift = redshift;
        }

        public override string ToString()
        {
            return Coords.ToString() + STANDART_STRING_DELIMETER + FluxOn1400 + STANDART_STRING_DELIMETER + FluxOn325 + STANDART_STRING_DELIMETER + SpectralIndex;
        }

        public string ToString(string delimeter = STANDART_STRING_DELIMETER)
        {
            return Coords.ToString() + delimeter + FluxOn1400 + delimeter + FluxOn325 + delimeter + SpectralIndex + delimeter + Type.ToString();
        }

        public string ToLongString(string delimeter = STANDART_STRING_DELIMETER)
        {
            return Coords.ToString() + delimeter + FluxOn325 + delimeter + FluxOn1400 + delimeter + SpectralIndex + 
                delimeter + Type.ToString() + delimeter + DensityRatio + delimeter + Redshift;
        }
        
        public static StructureType ParseType(string str)
        {
            switch (str)
            {
                case "FRI":
                    return StructureType.FRI;
                case "FRII":
                    return StructureType.FRII;
                case "Undefined":
                    return StructureType.Undefined;
                case "Unchecked":
                    return StructureType.Unchecked;
                default:
                    return StructureType.Unchecked;
            }
        }
    }
}
