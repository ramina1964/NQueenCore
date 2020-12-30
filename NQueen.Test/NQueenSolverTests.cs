using FluentAssertions;
using NQueen.Model;
using NUnit.Framework;
using NQueen.Shared;

namespace NQueen.Test
{
    public class NQueenSolverTests : TestBase
    {
        [TestCase(2, SolutionMode.Single), TestCase(3, SolutionMode.Single)]
        [TestCase(2, SolutionMode.Unique), TestCase(3, SolutionMode.Unique)]
        [TestCase(2, SolutionMode.All), TestCase(3, SolutionMode.All)]
        public void Should_generate_an_empty_list_of_solutions(sbyte boardSize, SolutionMode solutionMode)
        {
            // Arrange
            Sut = new Solver(boardSize);
            ExpectedSolutions = GetExpectedSolutions(boardSize, solutionMode);

            // Act
            ActualSolutions = GetActualSolutions(boardSize, solutionMode, DisplayMode.Hide);

            // Assert
            Assert.AreEqual(ExpectedSolutions.Count, ActualSolutions.Count);
            ActualSolutions.Should().Equals(ExpectedSolutions);
        }

        [TestCase(1, SolutionMode.Single), TestCase(1, SolutionMode.Unique), TestCase(1, SolutionMode.All)]
        [TestCase(4, SolutionMode.Single), TestCase(5, SolutionMode.Single), TestCase(6, SolutionMode.Single)]
        [TestCase(7, SolutionMode.Single), TestCase(8, SolutionMode.Single), TestCase(9, SolutionMode.Single)]
        [TestCase(10, SolutionMode.Single), TestCase(11, SolutionMode.Single), TestCase(12, SolutionMode.Single)]
        [TestCase(13, SolutionMode.Single), TestCase(18, SolutionMode.Single), TestCase(19, SolutionMode.Single)]
        [TestCase(20, SolutionMode.Single), TestCase(21, SolutionMode.Single), TestCase(22, SolutionMode.Single)]
        [TestCase(23, SolutionMode.Single), TestCase(24, SolutionMode.Single), TestCase(25, SolutionMode.Single)]
        [TestCase(26, SolutionMode.Single), TestCase(27, SolutionMode.Single), TestCase(28, SolutionMode.Single)]
        public void Should_generate_a_single_solution(sbyte boardSize, SolutionMode solutionMode)
        {
            // Arrange
            Sut = new Solver(boardSize);
            ExpectedSolutions = GetExpectedSolutions(boardSize, solutionMode);

            // Act
            ActualSolutions = GetActualSolutions(boardSize, solutionMode, DisplayMode.Hide);

            // Assert
            Assert.AreEqual(ExpectedSolutions.Count, ActualSolutions.Count);
            ActualSolutions.Should().Equals(ExpectedSolutions);
        }

        [TestCase(4, SolutionMode.Unique), TestCase(5, SolutionMode.Unique), TestCase(6, SolutionMode.Unique)]
        [TestCase(7, SolutionMode.Unique), TestCase(8, SolutionMode.Unique), TestCase(9, SolutionMode.Unique)]
        public void Should_generate_correct_unique_solutions(sbyte boardSize, SolutionMode solutionMode)
        {
            // Arrange
            Sut = new Solver(boardSize);
            ExpectedSolutions = GetExpectedSolutions(boardSize, solutionMode);

            // Act
            ActualSolutions = GetActualSolutions(boardSize, solutionMode, DisplayMode.Hide);

            // Assert
            Assert.AreEqual(ExpectedSolutions.Count, ActualSolutions.Count);
            ActualSolutions.Should().Equals(ExpectedSolutions);
        }

        [TestCase(1, SolutionMode.All), TestCase(4, SolutionMode.All), TestCase(5, SolutionMode.All)]
        [TestCase(6, SolutionMode.All), TestCase(7, SolutionMode.All), TestCase(8, SolutionMode.All)]
        public void Should_generate_cortrect_all_solutions(sbyte boardSize, SolutionMode solutionMode)
        {
            // Arrange
            Sut = new Solver(boardSize);
            ExpectedSolutions = GetExpectedSolutions(boardSize, solutionMode);

            // Act
            ActualSolutions = GetActualSolutions(boardSize, solutionMode, DisplayMode.Hide);

            // Assert
            Assert.AreEqual(ExpectedSolutions.Count, ActualSolutions.Count);
            ActualSolutions.Should().Equals(ExpectedSolutions);
        }
    }
}
