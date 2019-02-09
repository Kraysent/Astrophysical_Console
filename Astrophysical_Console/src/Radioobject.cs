using static System.Math;

namespace Astrophysical_Console
{
    public class Radioobject
    {
        private string _catalog;
        private string _name;
        private Coordinates _coords;
        private double _fluxOn325;
        private double _fluxOn1400;
        private StructureType _type;
        private double _densityRatio;

        public Radioobject(string catalog, string name, Coordinates coords, double fluxOn325, double fluxOn1400)
        {
            Catalog = catalog;
            Name = name;
            Coords = coords;
            FluxOn325 = fluxOn325;
            FluxOn1400 = fluxOn1400;
            Type = StructureType.Unchecked;
        }

        public Radioobject()
        {
        }

        public string Catalog { get => _catalog; set => _catalog = value; }
        public string Name { get => _name; set => _name = value; }
        public Coordinates Coords { get => _coords; set => _coords = value; }
        public double FluxOn325 { get => _fluxOn325; set => _fluxOn325 = value; }
        public double FluxOn1400 { get => _fluxOn1400; set => _fluxOn1400 = value; }
        public StructureType Type { get => _type; set => _type = value; }
        public double DensityRatio { get => _densityRatio; set => _densityRatio = value; }
        public double SpectralIndex => (Log10(FluxOn1400) - Log10(FluxOn325)) / (Log10(1400) - Log(325));
        
        public override string ToString()
        {
            return Coords.ToString() + " - " + FluxOn1400 + " - " + FluxOn325 + " - " + SpectralIndex;
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

        public string ToString(string delimeter = " - ")
        {
            return Coords.ToString() + delimeter + FluxOn1400 + delimeter + FluxOn325 + delimeter + SpectralIndex + delimeter + Type.ToString();
        }

        public string ToLongString(string delimeter = " - ")
        {
            return Catalog + delimeter + Name + delimeter + Coords.ToString() + delimeter
                + FluxOn325 + delimeter + FluxOn1400 + delimeter + Type.ToString() + delimeter + DensityRatio;
        }
    }
}
