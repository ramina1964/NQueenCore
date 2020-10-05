using FluentAssertions;
using NQueen.Common.Enum;
using NQueen.Model;
using NUnit.Framework;

namespace NQueen.Test
{
    public partial class NQueenSolverTests
    {
        public Solver Sut { get; set; }

        #region PublicTestMethods
        [TestCase(9, SolutionMode.Single), TestCase(12, SolutionMode.Single), TestCase(13, SolutionMode.Single)]
        [TestCase(6, SolutionMode.Unique), TestCase(7, SolutionMode.Unique), TestCase(8, SolutionMode.Unique)]
        [TestCase(9, SolutionMode.Unique), TestCase(10, SolutionMode.Unique), TestCase(11, SolutionMode.Unique)]
        [TestCase(12, SolutionMode.Unique), TestCase(13, SolutionMode.Unique), TestCase(14, SolutionMode.Unique)]
        [TestCase(6, SolutionMode.All), TestCase(7, SolutionMode.All), TestCase(8, SolutionMode.All)]
        [TestCase(9, SolutionMode.All), TestCase(10, SolutionMode.All), TestCase(11, SolutionMode.All)]
        [TestCase(12, SolutionMode.All), TestCase(13, SolutionMode.All), TestCase(14, SolutionMode.All)]
        public void Should_generate_correct_amount_of_solutions(sbyte boardSize, SolutionMode solutionMode)
        {
            // Arrange
            Sut = new Solver(boardSize);
            var expected = (solutionMode == SolutionMode.Single)
                ? GetNoOfSingleSol()
                : (solutionMode == SolutionMode.Unique)
                ? GetNoOfUniqueSols(boardSize)
                : GetNoOfAllSols(boardSize);

            // Act
            var simResults = Sut.GetSimulationResultsAsync(boardSize, solutionMode);
            var actual = simResults.Result.NoOfSolutions;

            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase(1, SolutionMode.Single), TestCase(2, SolutionMode.Single), TestCase(3, SolutionMode.Single)]
        [TestCase(4, SolutionMode.Single), TestCase(5, SolutionMode.Single), TestCase(6, SolutionMode.Single)]
        [TestCase(7, SolutionMode.Single), TestCase(8, SolutionMode.Single), TestCase(9, SolutionMode.Single)]
        [TestCase(18, SolutionMode.Single), TestCase(19, SolutionMode.Single), TestCase(20, SolutionMode.Single)]
        [TestCase(21, SolutionMode.Single), TestCase(22, SolutionMode.Single), TestCase(23, SolutionMode.Single)]
        [TestCase(24, SolutionMode.Single), TestCase(25, SolutionMode.Single), TestCase(26, SolutionMode.Single)]
        [TestCase(27, SolutionMode.Single), TestCase(28, SolutionMode.Single)]
        [TestCase(1, SolutionMode.Unique), TestCase(2, SolutionMode.Unique), TestCase(3, SolutionMode.Unique)]
        [TestCase(4, SolutionMode.Unique), TestCase(5, SolutionMode.Unique), TestCase(6, SolutionMode.Unique)]
        [TestCase(7, SolutionMode.Unique), TestCase(8, SolutionMode.Unique), TestCase(9, SolutionMode.Unique)]
        [TestCase(4, SolutionMode.All), TestCase(5, SolutionMode.All), TestCase(6, SolutionMode.All)]
        [TestCase(7, SolutionMode.All), TestCase(8, SolutionMode.All)]
        public void Should_generate_correct_list_of_solutions(sbyte boardSize, SolutionMode solutionMode)
        {
            // Arrange
            Sut = new Solver(boardSize);
            var expected = (solutionMode == SolutionMode.Single)
                ? GetSingleSol(boardSize)
                : (solutionMode == SolutionMode.Unique)
                ? GetUniqueSols(boardSize)
                : GetAllSols(boardSize);

            // Act
            var simResults = Sut.GetSimulationResultsAsync(boardSize, solutionMode);
            var actual = simResults.Result.Solutions;

            // Assert
            actual.Should().BeEquivalentTo(expected, options =>
                options.Excluding(s => s.Id)
                .Excluding(s => s.Name));
        }
        #endregion PublicTestMethods
    }
}