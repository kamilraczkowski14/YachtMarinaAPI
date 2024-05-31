using FluentValidation;
using YachtMarinaAPI.DAL;
using YachtMarinaAPI.Dtos;

namespace YachtMarinaAPI.Validators
{
    public class CreateYachtDtoValidator : AbstractValidator<CreateYachtDto>
    {
        public CreateYachtDtoValidator(ApplicationDbContext _context)
        {
            RuleFor(x => x.Length).NotEmpty().GreaterThan(0);
            RuleFor(x => x.YearOfProduction).NotEmpty().GreaterThan(0);
        }
    }
}
