using ReventTask.Domain.DataTransferObjects.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReventTask.Domain.DataTransferObjects.DtoValidators
{
    public class MailRequestDtoValidator : AbstractValidator<MailRequestDto>
    {
            public MailRequestDtoValidator()
            {
                RuleFor(x => x.RecepientEmail)
                .NotNull()
                .NotEmpty()
                .EmailAddress()
                ;
                RuleFor(x => x.Subject)
                .NotNull()
                .MaximumLength(200)
                .NotEmpty()
                ;
                RuleFor(x => x.Body)
                .NotNull()
                ;
            }
        }
    }

