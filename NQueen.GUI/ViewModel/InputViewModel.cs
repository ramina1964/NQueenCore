using FluentValidation;
using NQueen.Shared;
using NQueen.Shared.Enums;

namespace NQueen.GUI.ViewModel
{
    public class InputViewModel : AbstractValidator<MainViewModel>
    {
        public InputViewModel() => ValidationRules();

        private void ValidationRules()
        {
            _ = RuleFor(vm => vm.BoardSizeText)
                .NotNull().NotEmpty()
                .WithMessage(_ => Utility.ValueNullOrWhiteSpaceMsg)
                .Must(bst => sbyte.TryParse(bst, out sbyte value))
                .WithMessage(_ => Utility.InvalidSByteError)
                .Must(bst => sbyte.Parse(bst) >= Utility.MinBoardSize)
                .WithMessage(_ => Utility.SizeTooSmallMsg);

            _ = RuleFor(vm => vm.BoardSizeText)
                .Must(bst => sbyte.TryParse(bst, out sbyte result) && result <= Utility.MaxBoardSizeForSingleSolution)
                .When(vm => vm.SolutionMode == SolutionMode.Single)
                .WithMessage(_ => Utility.SizeTooLargeForSingleSolutionMsg);

            _ = RuleFor(vm => vm.BoardSizeText)
                .Must(bst => sbyte.TryParse(bst, out sbyte result) && result <= Utility.MaxBoardSizeForUniqueSolutions)
                .When(vm => vm.SolutionMode == SolutionMode.Unique)
                .WithMessage(_ => Utility.SizeTooLargeForUniqueSolutionsMsg);

            _ = RuleFor(vm => vm.BoardSizeText)
                .Must(bst => sbyte.TryParse(bst, out sbyte result) && result <= Utility.MaxBoardSizeForAllSolutions)
                .When(vm => vm.SolutionMode == SolutionMode.All)
                .WithMessage(_ => Utility.SizeTooLargeForAllSolutionsMsg);
        }
    }
}
