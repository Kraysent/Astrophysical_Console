using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrophysical_Console
{
    class Radioobject
    {
        private string catalog;
        private string name;
        private Coordinates coords;
        private int fluxOn325;
        private int fluxOn1400;

        public Radioobject(string catalog, string name, Coordinates coords, int fluxOn325, int fluxOn1400)
        {
            Catalog = catalog;
            Name = name;
            Coords = coords;
            FluxOn325 = fluxOn325;
            FluxOn1400 = fluxOn1400;
        }

        public string Catalog { get => catalog; set => catalog = value; }
        public string Name { get => name; set => name = value; }
        public Coordinates Coords { get => coords; set => coords = value; }
        public int FluxOn325 { get => fluxOn325; set => fluxOn325 = value; }
        public int FluxOn1400 { get => fluxOn1400; set => fluxOn1400 = value; }
    }
}
