namespace ReventTask.Domain;
public partial class InboundLogDtoValidator : AbstractValidator<InboundLogDto>
{
    public InboundLogDtoValidator()
    {
       RuleFor(x => x.RequestSystem)
       .NotNull()
       .NotEmpty()
       .MaximumLength(30)
       ;
       RuleFor(x => x.APICalled)
       .NotNull()
       .NotEmpty()
       .MaximumLength(30)
       ;
       RuleFor(x => x.APIMethod)
       .NotNull()
       .NotEmpty()
       .MaximumLength(30)
       ;
       RuleFor(x => x.LogDate)
       .NotNull()
       .NotEmpty()
       ;
       RuleFor(x => x.ImpactUniqueIdentifier)
       .NotNull()
       .NotEmpty()
       .MaximumLength(30)
       ;
       RuleFor(x => x.ImpactUniqueidentifierValue)
       .NotNull()
       .NotEmpty()
       .MaximumLength(30)
       ;
       RuleFor(x => x.AlternateUniqueIdentifier)
       .NotNull()
       .NotEmpty()
       .MaximumLength(30)
       ;
       RuleFor(x => x.AlternateUniqueidentifierValue)
       .NotNull()
       .NotEmpty()
       .MaximumLength(30)
       ;
       RuleFor(x => x.RequestDetails)
       .NotNull()
       .NotEmpty()
       .MaximumLength(100)
       ;
       RuleFor(x => x.RequestDateTime)
       .NotNull()
       .NotEmpty()
       ;
       RuleFor(x => x.ResponseDetails)
       .NotNull()
       .NotEmpty()
       .MaximumLength(100)
       ;
       RuleFor(x => x.ResponseDateTime)
       .NotNull()
       .NotEmpty()
       ;
       RuleFor(x => x.OutboundLogs)
       ;
       RuleFor(x => x.ExceptionDetails)
       .NotNull()
       .NotEmpty()
       .MaximumLength(5000)
       ;
       
    }
}
