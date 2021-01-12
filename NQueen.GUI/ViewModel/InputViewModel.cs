using FluentValidation;
using NQueen.Shared;
using NQueen.Shared.Enums;

namespace NQueen.GUI.ViewModel
{
    public class InputViewModel : AbstractValidator<SolverViewModel>
    {
        public InputViewModel() => ValidationRules();

        private void ValidationRules()
        {
            _ = RuleFor(q => q.BoardSizeText)
                .NotNull().NotEmpty()
                .WithMessage(q => Utility.ValueNullOrWhiteSpaceMsg)

                .Must(bst => sbyte.TryParse(bst, out sbyte value))
                .WithMessage(q => Utility.InvalidSByteError)

                .Must(bst => sbyte.Parse(bst) >= Utility.MinBoardSize)
                .WithMessage(q => Utility.SizeTooSmallMsg);

            _ = RuleFor(q => q.BoardSizeText)
                .Must(bst => sbyte.TryParse(bst, out sbyte result) && result <= Utility.MaxBoardSizeForSingleSolution)
                .When(q => q.SolutionMode == SolutionMode.Single)
                .WithMessage(q => Utility.SizeTooLargeForSingleSolutionMsg);

            _ = RuleFor(q => q.BoardSizeText)
                .Must(bst => sbyte.TryParse(bst, out sbyte result) && result <= Utility.MaxBoardSizeForUniqueSolutions)
                .When(q => q.SolutionMode == SolutionMode.Unique)
                .WithMessage(q => Utility.SizeTooLargeForUniqueSolutionsMsg);

            _ = RuleFor(q => q.BoardSizeText)
                .Must(bst => sbyte.TryParse(bst, out sbyte result) && result <= Utility.MaxBoardSizeForAllSolutions)
                .When(q => q.SolutionMode == SolutionMode.All)
                .WithMessage(q => Utility.SizeTooLargeForAllSolutionsMsg);
        }
    }
}