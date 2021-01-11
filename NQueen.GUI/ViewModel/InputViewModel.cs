using FluentValidation;
using NQueen.Shared;
using NQueen.Shared.Enums;
using NQueen.Shared.Properties;

namespace NQueen.GUI.ViewModel
{
    public class InputViewModel : AbstractValidator<SolverViewModel>
    {
        public InputViewModel() => ValidationRules();

        private void ValidationRules()
        {
            _ = RuleFor(q => q.BoardSizeText)
                .NotNull().NotEmpty()
                .WithMessage(q => string.Format(Resources.ValueNullOrWhiteSpaceError, nameof(q.BoardSize)))

                .Must(bst => sbyte.TryParse(bst, out sbyte value))
                .WithMessage(q => string.Format(Resources.InvalidIntError, nameof(q.BoardSize)))

                .Must(bst => sbyte.Parse(bst) >= Utility.MinBoardSize)
                .WithMessage(q => string.Format(Resources.BoardSizeTooSmallError, nameof(q.BoardSize), Utility.MinBoardSize));

            _ = RuleFor(q => q.BoardSizeText)
                .Must(bst => sbyte.TryParse(bst, out sbyte result) && result <= Utility.MaxBoardSizeForSingleCase)
                .When(q => q.SolutionMode == SolutionMode.Single)
                .WithMessage(q => string.Format(Resources.BoardSizeTooLargeSingleCaseError,
                        nameof(q.BoardSize), nameof(Resources.SingleSolution), Utility.MaxBoardSizeForSingleCase));

            _ = RuleFor(q => q.BoardSizeText)
                .Must(bst => sbyte.TryParse(bst, out sbyte result) && result <= Utility.MaxBoardSizeForUniqueCase)
                .When(q => q.SolutionMode == SolutionMode.Unique)
                .WithMessage(q => string.Format(Resources.BoardSizeTooLargeUniqueCaseError,
                        nameof(q.BoardSize), nameof(Resources.UniqueSolutions), Utility.MaxBoardSizeForUniqueCase));

            _ = RuleFor(q => q.BoardSizeText)
                .Must(bst => sbyte.TryParse(bst, out sbyte result) && result <= Utility.MaxBoardSizeForAllCase)
                .When(q => q.SolutionMode == SolutionMode.All)
                .WithMessage(q => string.Format(Resources.BoardSizeTooLargeAllCaseError,
                        nameof(q.BoardSize), nameof(Resources.AllSolutions), Utility.MaxBoardSizeForAllCase));
        }
    }
}