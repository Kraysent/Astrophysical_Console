using static System.Math;

namespace AstrophysicalEngine.Model
{
    public class Radioobject
    {
        public const string STANDART_STRING_DELIMETER = "|";
        
        private readonly Coordinates _coords;
        private readonly double _fluxOn325;
        private readonly double _fluxOn1400;
        private readonly double _spectralIndex;
        private double _redshift; 
        private StructureType _type;
        private double _densityRatio;
        
        public Coordinates Coords => _coords;
        public double FluxOn325 => _fluxOn325;
        public double FluxOn1400 => _fluxOn1400;
        public double SpectralIndex => _spectralIndex;
        public StructureType Type { get => _type; set => _type = value; }
        public double DensityRatio { get => _densityRatio; set => _densityRatio = value; }
        public double Redshift { get => _redshift; set => _redshift = value; }

        public Radioobject(Coordinates coords, double fluxOn325, double fluxOn1400)
        {
            _coords = coords;
            _fluxOn325 = fluxOn325;
            _fluxOn1400 = fluxOn1400;
            _spectralIndex = (Log10(fluxOn1400) - Log10(FluxOn325)) / (Log10(1400) - Log10(325));
        }

        public Radioobject(Coordinates coords, double spectralIndex)
        {
            _coords = coords;
            _spectralIndex = spectralIndex;
        }

        public Radioobject(Coordinates coords, double fluxOn325, double fluxOn1400, double spectralIndex, StructureType type, double densityRatio, double redshift)
        {
            _coords = coords;
            _fluxOn325 = fluxOn325;
            _fluxOn1400 = fluxOn1400;
            _spectralIndex = spectralIndex;
            _type = type;
            _densityRatio = densityRatio;
            _redshift = redshift;
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
