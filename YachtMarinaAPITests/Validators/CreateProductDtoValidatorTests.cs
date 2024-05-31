using FluentValidation.TestHelper;
using Xunit;
using YachtMarinaAPI.Dtos;
using YachtMarinaAPI.Validators;

namespace YachtMarinaAPI.Tests.Validators
{
    public class CreateProductDtoValidatorTests
    {
        [Fact]
        public void CreateProductDtoValidator_ValidDto_ShouldNotHaveValidationError()
        {
            var validator = new CreateProductDtoValidator(null); 
            var dto = new CreateProductDto
            {
                Price = 100000,
                LoanPricePerDay = 500,
                YearOfProduction = 2022,
                QuantityInStock = -2
            };

            var result = validator.TestValidate(dto);

            result.ShouldNotHaveValidationErrorFor(x => x.Price);
            result.ShouldNotHaveValidationErrorFor(x => x.LoanPricePerDay);
            result.ShouldNotHaveValidationErrorFor(x => x.YearOfProduction);
            result.ShouldHaveValidationErrorFor(x => x.QuantityInStock);
        }
    }
}



