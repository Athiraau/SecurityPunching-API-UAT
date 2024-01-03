using DataAccess.Dto.Request;
using FluentValidation;


namespace SecurityPunching.Validators
{
    public class UpdateImageValidator : AbstractValidator<SecPunchPostDto>
    {
        public UpdateImageValidator()
        {
            //RuleFor(d => d.secCode).NotNull().GreaterThan(0).WithMessage("Security Code must be greater than 0");
            RuleFor(d => d.secPhoto).NotNull().NotEmpty().WithMessage("Photo string64 is required");
           // RuleFor(d => d.secCode).Must(EmpcodeLength).WithMessage("Security Code Length must be equal or less than 9");
        }

        private bool EmpcodeLength(int secCode)
        {
            if (Convert.ToString(secCode).Length > 9)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
