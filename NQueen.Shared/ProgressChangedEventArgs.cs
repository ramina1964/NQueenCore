using System;

namespace NQueen.Shared
{
    public class ProgressValueChangedEventArgs : EventArgs
    {
        public ProgressValueChangedEventArgs(double value) => Value = value;

        public double Value { get; }
    }
}
