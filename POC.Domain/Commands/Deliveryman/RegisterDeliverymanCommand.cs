using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using POC.Artifacts.Domain.Responses;
using POC.Domain.Commands.Motorcycle;
using POC.Domain.Models.Entities;
using POC.Domain.Models.Enumerations;
using POC.Artifacts.Helpers;

namespace POC.Domain.Commands.Deliveryman
{
	public class RegisterDeliverymanCommand: IRequest<GenericCommandResult>
	{
        [Required]
        public string Name { get; set; }
        [Required]
        public string CNPJ { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public string DriverLicenseNumber { get; set; }
        [Required]
        public int DriveLicenseTypeId { get; set; }
        [Required]
        public IFormFile DriverLicenseFile { get; set; }
        [Required]
        public string Identifier { get; set; }
        [Required]
        public string Password { get; set; } = String.Empty;
        [Required]
        public string ConfirmPassword { get; set; } = String.Empty;
    }


    public class RegisterDeliverymanCommandValidator : AbstractValidator<RegisterDeliverymanCommand>
    {
        public RegisterDeliverymanCommandValidator()
        {
            RuleFor(x => x.Name)
                    .NotEmpty()
                    .WithMessage("Necessário informar seu nome completo para realizar o cadastro.")
                    .MaximumLength(255)
                    .WithMessage("Tamanho do nome inválido. O tamanho do do nome deve ter entre 1 e 255 caracteres.");

            RuleFor(x => x.CNPJ)
                 .Must(x => x.IsCNPJ())
                 .WithMessage("CNPJ informado não é válido.");

            RuleFor(x => x.BirthDate)
                    .NotNull()
                    .WithMessage("Data de nascimento informada não é válida.")
                    .Must(x => x.GetYearDiff() >= 18)
                    .WithMessage("Você precisa ter no mínimo 18 anos para se cadastrar em nossa plataforma.");

            RuleFor(x => x.DriverLicenseNumber)
                 .Length(11)
                 .WithMessage("O número da CNH deve conter 11 dígitos.")
                 .Must(x => x.IsDriverLicenseNumber())
                 .WithMessage("O número da CNH informada não é valido.");

            RuleFor(x => x.DriveLicenseTypeId)
                .Must(z =>
                {
                    DriverLicenseTypeEnum _driverLicenseType = DriverLicenseTypeEnum.From(z);
                    return _driverLicenseType != DriverLicenseTypeEnum.Unknown;
                })
                .WithMessage($"O tipo de CNH informado não é válido. Tipos disponíveis: {String.Join(", ",DriverLicenseTypeEnum.List().Select(v => v.Description))}.");

            RuleFor(x => x.DriverLicenseFile)
                .Must(h => h != null && h.Length > 0)
                .WithMessage("Não foi possível receber a imagem da sua CNH. Tente realizar o upload novamente.")
                .Must(z =>
                {
                    //Caso a validação acima já seja acionada, retornar para não enviar multiplas mensagens de erro.
                    if (z == null)
                        return true;
                    try
                    {
                        DriverLicenseNumberFileExtensionEnum _driverLicenseType = DriverLicenseNumberFileExtensionEnum.FromName(Path.GetExtension(z.FileName));
                        return _driverLicenseType != DriverLicenseNumberFileExtensionEnum.Unknown;
                    }
                    catch 
                    {
                        return false;
                    }
                   

                })
                .WithMessage($"A imagem da CNH enviada é inválida. Formatos disponíveis: {String.Join(", ", DriverLicenseNumberFileExtensionEnum.List().Select(v => v.Description))}.");


            RuleFor(x => x.Identifier)
                    .NotNull()
                    .WithMessage("Necessário preencher seu número identificador. O número identificador é apenas uma chave que você poderá definir.")
                    .MaximumLength(255)
                    .WithMessage("Tamanho do identificador da moto inválido. O tamanho do identificador da moto deve ter entre 1 e 255 caracteres.");

            RuleFor(x => x.Password)
                  .Must(x => x.IsValidPassword())
                  .WithMessage($@"Precisamos que você melhore sua senha.Sua senha precisa conter:- No mínimo 12 e no máximo 16 caracteres.- No mínimo uma letra maiúscula. - No mínimo um número.- No mínimo uma letra mínuscula.- No mínimo um caracter especial.");

            RuleFor(x => new { x.Password, x.ConfirmPassword })
                   .Must(z => z.Password == z.ConfirmPassword)
                   .WithMessage("Os campos de 'Senha' e 'Confirmação da senha' estão diferentes. Preencha o campo de 'Confirmação da senha' com a mesma senha digitada no campo anterior.");

          
        }

    }
}

