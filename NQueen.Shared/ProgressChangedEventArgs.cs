using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NQueen.Shared
{
    public class ProgressValueChangedEventArgs : EventArgs
    {
        public ProgressValueChangedEventArgs(double progress)
        {
            Value = progress;
        }

        public double Value { get; }
    }
}
