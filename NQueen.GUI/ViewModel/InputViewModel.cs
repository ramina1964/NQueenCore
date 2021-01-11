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
                .WithMessage(q => Utility.ValueNullOrWhiteSpaceError)

                .Must(bst => sbyte.TryParse(bst, out sbyte value))
                .WithMessage(q => Utility.InvalidSByteError)

                .Must(bst => sbyte.Parse(bst) >= Utility.MinBoardSize)
                .WithMessage(q => Utility.BoardSizeTooSmallError);

            _ = RuleFor(q => q.BoardSizeText)
                .Must(bst => sbyte.TryParse(bst, out sbyte result) && result <= Utility.MaxBoardSizeForSingleCase)
                .When(q => q.SolutionMode == SolutionMode.Single)
                .WithMessage(q => Utility.BoardSizeTooLargeSingleCaseError);

            _ = RuleFor(q => q.BoardSizeText)
                .Must(bst => sbyte.TryParse(bst, out sbyte result) && result <= Utility.MaxBoardSizeForUniqueCase)
                .When(q => q.SolutionMode == SolutionMode.Unique)
                .WithMessage(q => Utility.BoardSizeTooLargeUniqueCaseError);

            _ = RuleFor(q => q.BoardSizeText)
                .Must(bst => sbyte.TryParse(bst, out sbyte result) && result <= Utility.MaxBoardSizeForAllCase)
                .When(q => q.SolutionMode == SolutionMode.All)
                .WithMessage(q => Utility.BoardSizeTooLargeAllCaseError);
        }
    }
}