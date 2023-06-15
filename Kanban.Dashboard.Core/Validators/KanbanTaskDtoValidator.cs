using FluentValidation;
using Kanban.Dashboard.Core.Dtos;

namespace Kanban.Dashboard.Core.Validators
{
    public class KanbanTaskDtoValidator : AbstractValidator<KanbanTaskDto>
    {
        public KanbanTaskDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .Length(1, 200).WithMessage("Title must be between 1 and 200 characters.");

            RuleFor(x => x.Description)
                .Length(0, 4096).WithMessage("Description must be less than or equal to 4096 characters.");

            RuleFor(x => x.Status)
                .Length(1, 100).WithMessage("Status is required.");
        }
    }
}
