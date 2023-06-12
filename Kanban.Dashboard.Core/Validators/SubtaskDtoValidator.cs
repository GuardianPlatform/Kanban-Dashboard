/*using FluentValidation;
using Kanban.Dashboard.Core.Dtos;

namespace Kanban.Dashboard.Core.Validators
{
    public class SubtaskDtoValidator : AbstractValidator<SubtaskDto>
    {
        public SubtaskDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .Length(1, 200).WithMessage("Title must be between 1 and 200 characters.");

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.");
        }
    }
}
*/