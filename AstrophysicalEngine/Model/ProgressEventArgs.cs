using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstrophysicalEngine.Model
{
    public class ProgressEventArgs : EventArgs
    {
        public int Current { get; set; }
        public int Maximum { get; set; }

        public ProgressEventArgs(int current, int maximum) : base()
        {
            Current = current;
            Maximum = maximum;
        }
    }
}
