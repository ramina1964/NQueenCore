using FluentValidation;

namespace NQueen.GUI.ViewModel
{
    public class InputViewModel : AbstractValidator<SolverViewModel>
    {
        #region Constructors
        public InputViewModel() => ValidationRules();
        #endregion Constructors

        #region PrivateMethods
        private void ValidationRules()
        {
            RuleFor(q => q.BoardSizeText)
                .NotNull()
                //.WithMessage(q => string.Format(Resources.ValueNullOrWhiteSpaceError, nameof(q.BoardSize)))

                .NotEmpty()
                //.WithMessage(q => string.Format(Resources.ValueNullOrWhiteSpaceError, nameof(q.BoardSize)))

                .Must(bst => sbyte.TryParse(bst, out sbyte value))
                //.WithMessage(q => string.Format(Resources.InvalidIntError, nameof(q.BoardSize)))

                .Must(bst => sbyte.Parse(bst) >= 4);
            //.WithMessage(q => string.Format(Resources.BoardSizeTooSmallError, nameof(q.BoardSize), Settings.Default.MinBoardSize));

            //RuleFor(q => q.BoardSizeText)
            //	.Must(bst => sbyte.TryParse(bst, out sbyte result) && result <= Settings.Default.MaxBoardSizeForSingleCase)
            //	.When(q => q.SolutionMode == SolutionMode.Single)
            //	.WithMessage(q => string.Format(Resources.BoardSizeTooLargeSingleCaseError,
            //			nameof(q.BoardSize), nameof(Resources.SingleSolution), Settings.Default.MaxBoardSizeForSingleCase));

            //RuleFor(q => q.BoardSizeText)
            //	.Must(bst => sbyte.TryParse(bst, out sbyte result) && result <= Settings.Default.MaxBoardSizeForUniqueCase)
            //	.When(q => q.SolutionMode == SolutionMode.Unique)
            //	.WithMessage(q => string.Format(Resources.BoardSizeTooLargeUniqueCaseError,
            //			nameof(q.BoardSize), nameof(Resources.UniqueSolutions), Settings.Default.MaxBoardSizeForUniqueCase));

            //RuleFor(q => q.BoardSizeText)
            //	.Must(bst => sbyte.TryParse(bst, out sbyte result) && result <= Settings.Default.MaxBoardSizeForAllCase)
            //	.When(q => q.SolutionMode == SolutionMode.All)
            //	.WithMessage(q => string.Format(Resources.BoardSizeTooLargeAllCaseError,
            //			nameof(q.BoardSize), nameof(Resources.AllSolutions), Settings.Default.MaxBoardSizeForAllCase));
        }
        #endregion PrivateMethods
    }
}