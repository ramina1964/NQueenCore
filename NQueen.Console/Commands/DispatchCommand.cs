using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NQueen.Kernel;
using NQueen.Shared.Enums;
using NQueen.Shared.Interfaces;

namespace NQueen.ConsoleApp.Commands
{
    internal class DispatchCommands
    {
        public static int BoardSize { get; set; }
        public static DisplayMode DisplayMode { get; set; }
        public static SolutionMode SolutionMode { get; set; }
        internal static bool ProcessCommand(string feature, string value)
        {
            bool ret = false;
            feature = feature.Replace("  ", " ").TrimEnd();
            if (feature == "")
            {
                System.Environment.Exit(0);
            }

            feature = feature.ToUpper();
            if (feature == "RUN")
            {
                var solver = new Solver((sbyte)BoardSize);
                solver.DisplayMode = DisplayMode.Hide;
                solver.SolutionMode = SolutionMode;
                var res = solver.GetResults();
                if (solver.DisplayMode == DisplayMode.Hide)
                {
                    ConsoleUtils.WriteLineColored(ConsoleColor.Blue, $"Here is a summary from NQueen solver with boardsize {res.BoardSize}:\n");
                    ConsoleUtils.WriteLineColored(ConsoleColor.Gray, $"Number of solutions found: {res.NoOfSolutions}");
                    ConsoleUtils.WriteLineColored(ConsoleColor.Gray, $"Elapsed time in seconds: {res.ElapsedTimeInSec}");
                    var example = res.Solutions.First().Details;
                    ConsoleUtils.WriteLineColored(ConsoleColor.Blue, "\nExample output - first solution found:\n");
                    ConsoleUtils.WriteLineColored(ConsoleColor.Yellow, example);
                }
                else
                {
                    foreach (var item in res.Solutions)
                    {
                        Console.WriteLine(item.Details);
                    }
                }
                return true;
            }

            if (feature == "BOARDSIZE")
            {
                var ok = int.TryParse(value, out int size);
                if (ok)
                {
                    BoardSize = Convert.ToSByte(size);
                    return true;
                }
                else
                {
                    Console.WriteLine("Could not parse displaymode");
                    return false;
                }
            }

            if (feature == "DISPLAYMODE")
            {
                var svar = value[0].ToString().ToUpper() + value[1..];
                var ok = Enum.TryParse(svar, out DisplayMode displayMode);
                if (ok)
                {
                    DisplayMode = displayMode;
                    return true;
                }

                else
                {
                    Console.WriteLine("Could not parse displaymode");
                }
            }

            if (feature == "SOLUTIONMODE")
            {
                var svar = value[0].ToString().ToUpper() + value[1..];
                var ok = Enum.TryParse(svar, out SolutionMode solutionMode);
                if (ok)
                {
                    SolutionMode = solutionMode;
                    return true;
                }

                else
                {
                    Console.WriteLine("Could not parse solutionmode");
                }
            }

            return ret;
        }

        internal static void ShowErrorExit(string errorString)
        {

            ConsoleColor priorColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("ERROR: ");
            Console.ForegroundColor = priorColor;
            Console.WriteLine(errorString);
            Console.WriteLine();
            System.Environment.Exit(-1);

        }
    }
}
