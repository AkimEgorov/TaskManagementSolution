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
    /// Валидатор запроса на создание задачи.
    /// </summary>
    public sealed class CreateTaskValidator : AbstractValidator<CreateTaskRequest>
    {
        public CreateTaskValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Название задачи не может быть пустым.")
                .MaximumLength(200).WithMessage("Название задачи не может превышать 200 символов.");

            RuleFor(x => x.Description)
                .MaximumLength(2000).WithMessage("Описание задачи не может превышать 2000 символов.");
        }
    }
}
