using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.Dto;

namespace TaskManagement.Application.Validators
{
    /// <summary>
    /// Валидатор запроса на обновление задачи.
    /// </summary>
    public sealed class UpdateTaskValidator : AbstractValidator<UpdateTaskRequest>
    {
        public UpdateTaskValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Название задачи не может быть пустым.")
                .MaximumLength(200).WithMessage("Название задачи не может превышать 200 символов.");

            RuleFor(x => x.Description)
                .MaximumLength(2000).WithMessage("Описание задачи не может превышать 2000 символов.");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Статус задачи обязателен.")
                .Must(BeValidStatus).WithMessage("Недопустимое значение статуса. Разрешены: New, InProgress, Completed.");
        }

        private static bool BeValidStatus(string status) =>
            Enum.TryParse<Domain.Entities.TaskStatus>(status, ignoreCase: true, out _);
    }
}
