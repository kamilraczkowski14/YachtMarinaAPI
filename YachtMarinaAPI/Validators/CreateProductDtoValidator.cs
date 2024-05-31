using FluentValidation;
using YachtMarinaAPI.DAL;
using YachtMarinaAPI.Dtos;

namespace YachtMarinaAPI.Validators
{
    public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductDtoValidator(ApplicationDbContext _context)
        {
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
            RuleFor(x => x.LoanPricePerDay).GreaterThanOrEqualTo(0);
            RuleFor(x => x.YearOfProduction).GreaterThanOrEqualTo(1950);
            RuleFor(x => x.QuantityInStock).GreaterThanOrEqualTo(0);
        }
    }
}
