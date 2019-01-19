﻿using System;

namespace Astrophysical_Console
{
    class Radioobject
    {
        private string catalog;
        private string name;
        private Coordinates coords;
        private double fluxOn325;
        private double fluxOn1400;

        public Radioobject(string catalog, string name, Coordinates coords, double fluxOn325, double fluxOn1400)
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
        public double FluxOn325 { get => fluxOn325; set => fluxOn325 = value; }
        public double FluxOn1400 { get => fluxOn1400; set => fluxOn1400 = value; }
        public double SpectralIndex => (Math.Log10(FluxOn1400) - Math.Log10(FluxOn325)) / (Math.Log10(1400) - Math.Log(325));
    }
}