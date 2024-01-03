using DataAccess.Dto.Request;
using FluentValidation;

namespace SecurityPunching.Validators
{
    public class SecPunchValidator:AbstractValidator<SecPunchReqDto>
    {
        public SecPunchValidator()
        {
            RuleFor(d => d.p_flag).NotNull().NotEmpty().WithMessage("Flag is required");

        }
       
    }
}
