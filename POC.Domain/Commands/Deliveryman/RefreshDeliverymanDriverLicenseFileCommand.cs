using System;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using POC.Domain.Models.Enumerations;
using MediatR;
using POC.Artifacts.Domain.Responses;

namespace POC.Domain.Commands.Deliveryman
{
    public class RefreshDeliverymanDriverLicenseFileCommand: IRequest<GenericCommandResult>
    {
        [Required]
        public IFormFile DriverLicenseFile { get; set; }

        [Required]
        public int DeliverymanId { get; set; }


        public class RefreshDeliverymanDriverLicenseFileCommandValidator : AbstractValidator<RefreshDeliverymanDriverLicenseFileCommand>
        {
            public RefreshDeliverymanDriverLicenseFileCommandValidator()
            {

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


                RuleFor(x => x.DeliverymanId)
                        .Must(x => x > 0)
                        .WithMessage("Não conseguimos lhe identificar. Realize o login novamente.");



            }
        }
    }
}

