using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrophysical_Console.src
{
    class RadioobjectEqualityComparer : IEqualityComparer<Radioobject>
    {
        public bool Equals(Radioobject obj1, Radioobject obj2)
        {
            if (Coordinates.Distance(obj1.Coords, obj2.Coords) < 15)
                return true;
            else
                return false;
        }

        public int GetHashCode(Radioobject obj)
        {
            return obj.Coords.GetHashCode();
        }
    }
}
