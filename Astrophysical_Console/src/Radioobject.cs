using static System.Math;

namespace Astrophysical_Console
{
    public class Radioobject
    {
        public const string STANDART_STRING_DELIMETER = "|";

        private readonly string _catalog;
        private readonly string _name;
        private readonly Coordinates _coords;
        private readonly double _fluxOn325;
        private readonly double _fluxOn1400;
        private readonly double _spectralIndex;
        private StructureType _type;
        private double _densityRatio;

        public string Catalog => _catalog;
        public string Name => _name;
        public Coordinates Coords => _coords;
        public double FluxOn325 => _fluxOn325;
        public double FluxOn1400 => _fluxOn1400;
        public double SpectralIndex => _spectralIndex;
        public StructureType Type { get => _type; set => _type = value; }
        public double DensityRatio { get => _densityRatio; set => _densityRatio = value; }

        public Radioobject(string catalog, string name, Coordinates coords, double fluxOn325, double fluxOn1400)
        {
            _catalog = catalog;
            _name = name;
            _coords = coords;
            _fluxOn325 = fluxOn325;
            _fluxOn1400 = fluxOn1400;
            _spectralIndex = (Log10(fluxOn1400) - Log10(FluxOn325)) / (Log10(1400) - Log10(325));
        }

        public Radioobject(string catalog, string name, Coordinates coords, double spectralIndex)
        {
            _catalog = catalog;
            _name = name;
            _coords = coords;
            _spectralIndex = spectralIndex;
        }

        public Radioobject(string catalog, string name, Coordinates coords, double fluxOn325, double fluxOn1400, StructureType type, double densityRatio) : this(catalog, name, coords, fluxOn325, fluxOn1400)
        {
            _type = type;
            _densityRatio = densityRatio;
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
            return Catalog + delimeter + Name + delimeter + Coords.ToString() + delimeter
                + FluxOn325 + delimeter + FluxOn1400 + delimeter + Type.ToString() + delimeter + DensityRatio;
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
