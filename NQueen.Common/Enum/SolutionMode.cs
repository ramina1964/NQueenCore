using System.ComponentModel;

namespace NQueen.Common.Enum
{
    public enum SolutionMode
    {
        [Description("A Single Solution of NQueen-Problem")]
        Single,

        [Description("Unique, Non-Symmetrical Solution, of NQueen-Problem")]
        Unique,

        [Description("All Solution, of NQueen-Problem. Included Symmetrical Ones")]
        All
    }
}