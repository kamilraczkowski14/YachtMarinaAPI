using FluentValidation;
using YachtMarinaAPI.DAL;
using YachtMarinaAPI.Dtos;

namespace YachtMarinaAPI.Validators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator(ApplicationDbContext _context)
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();

            RuleFor(x => x.Username).MinimumLength(3);

            RuleFor(x => x.Password).MinimumLength(6);

            RuleFor(x => x.ConfirmPassword).Equal(e => e.Password);

        }
    }

}
