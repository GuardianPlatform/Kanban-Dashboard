using FluentValidation;
using Kanban.Dashboard.Core.Dtos;

namespace Kanban.Dashboard.Core.Validators
{
    public class BoardDtoValidator : AbstractValidator<BoardDto>
    {
        public BoardDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(1, 100).WithMessage("Name must be between 1 and 100 characters.");
        }
    }
}
