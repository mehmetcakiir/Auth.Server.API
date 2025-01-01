using AuthServer.Core.DTOs;
using FluentValidation;

namespace Auth.Server.API.Validations
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            //User Name alanı zorunlu
            RuleFor(x => x.UserName).NotEmpty().WithMessage("User Name is Required");

            //Email alanı zorunlu ve doğru formatta olması gerekiyor.
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is Required").EmailAddress().WithMessage("Incorrect Email Format");

            //Password alanı zorunlu
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is Required");
        }
    }
}
