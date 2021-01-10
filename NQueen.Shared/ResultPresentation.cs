using NQueen.Shared.Enums;
using NQueen.Shared.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace NQueen.Shared
{
    public class ResultPresentation
    {
        #region Constructor
        public ResultPresentation(ISimulationResults results)
        {
            MaxNoOfSolutionsInOutput = 50;
            BoardSize = results.BoardSize;
            Solutions = new ObservableCollection<Solution>(results.Solutions);
            NoOfSolutions = Solutions.Count;
            ElapsedTimeInSec = results.ElapsedTimeInSec;
            NoOfSolutionsInOutput = Math.Min(NoOfSolutions, MaxNoOfSolutionsInOutput);
        }
        #endregion Constructor

        internal ObservableCollection<Solution> Solutions { get; }

        internal double ElapsedTimeInSec { get; set; }

        internal int NoOfSolutions { get; }

        internal int MaxNoOfSolutionsInOutput { get; }

        internal int BoardSize { get; set; }

        internal int NoOfSolutionsInOutput { get; }

        internal static StringBuilder FormatSingleSolution(Solution solution) =>
            new StringBuilder().Append($"{solution.Details}");

        public string Write2File(SolutionMode solutionMode)
        {
            var dir = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent.Parent;
            var fileName = $"Board Size - {BoardSize}" + ".txt";
            var subFolder = Path.Combine(dir.FullName, "Results");
            var filePath = Path.Combine(subFolder, fileName);

            var _ = Directory.Exists(subFolder)
                ? new DirectoryInfo(subFolder)
                : Directory.CreateDirectory(subFolder);

            File.WriteAllText(filePath, PrintFinalResults(solutionMode));
            return filePath;
        }

        public StringBuilder FormatSolutionTitle(SolutionMode solutionMode)
        {
            const string prefix = "Sol. No.";
            const int prefixLength = 8;
            const int counterPlaces = prefixLength - 28;

            var title = Utility.SolutionTitle(solutionMode, NoOfSolutions);
            var sb = new StringBuilder().AppendLine(title);

            for (var index = 0; index < NoOfSolutionsInOutput; index++)
            {
                var solTitle = $"{prefix,0} {index + 1,counterPlaces}";
                sb.Append(solTitle).Append(FormatSingleSolution(Solutions[index]) + "\n");
            }

            return sb.AppendLine();
        }

        #region PrivateMembers
        private StringBuilder GetSummary(SolutionMode solutionMode)
        {
            const int placeHolderText = -28;
            const int decimalPlaces = 2;
            const int placeHolderInt = 20;

            var sb = new StringBuilder().AppendLine()
                .AppendLine("Summary of Results:")
                .AppendLine($"{"Date and Time",placeHolderText}{DateTime.Now,placeHolderInt}");

            sb.AppendLine($"{"BoardSize",placeHolderText}{BoardSize,placeHolderInt}");
            if (NoOfSolutions > MaxNoOfSolutionsInOutput)
            { sb.AppendLine($"No. of Solutions Included{MaxNoOfSolutionsInOutput,placeHolderInt}"); }

            // Here is: NoOfSolutions <= MaxNoOfSolutionsInOutput
            else
            {
                sb.AppendLine(solutionMode == SolutionMode.All
                      ? $"{"No. of All Solutions",placeHolderText}{NoOfSolutions,placeHolderInt}"
                      : $"{"No. of Unique Solutions",placeHolderText}{NoOfSolutions,placeHolderInt}");
            }

            var timeInSec = Math.Round(ElapsedTimeInSec, decimalPlaces);
            sb.AppendLine($"{"Elapsed Time (s)",placeHolderText}{timeInSec,placeHolderInt}");

            return sb;
        }

        private string PrintFinalResults(SolutionMode solutionMode)
        {
            var result = new StringBuilder()
                        .Append(FormatSolutionTitle(solutionMode))
                        .Append(GetSummary(solutionMode))
                        .AppendLine();

            return result.ToString();
        }
        #endregion PrivateMembers
    }
}