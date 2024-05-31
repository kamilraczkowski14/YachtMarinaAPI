using FluentValidation;
using YachtMarinaAPI.DAL;
using YachtMarinaAPI.Dtos;

namespace YachtMarinaAPI.Validators
{
    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator(ApplicationDbContext _context)
        {
            RuleFor(x => x.Email).EmailAddress();

            RuleFor(x => x.Email).Custom((value, context) =>
            {
                var existingEmail = _context.Users.Any(u => u.Email == value);

                if (existingEmail)
                {
                    context.AddFailure("Email", "Ten adres e-mail jest juz zajety");
                }
            });

            RuleFor(x => x.Username).MinimumLength(3);

            RuleFor(x => x.Username).Custom((value, context) =>
            {
                var existingName = _context.Users.Any(u => u.Username == value);

                if (existingName)
                {
                    context.AddFailure("Username", "Ta nazwa użytkownika już jest zajęta");
                }
            });

            RuleFor(x => x.NewPassword).MinimumLength(6);

            RuleFor(x => x.FirstName).MinimumLength(3);

            RuleFor(x => x.LastName).MinimumLength(3);

        }
    }
}
