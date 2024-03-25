using System;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace POC.API.Configurations
{
    ///<Summary>
    /// SETUP de linguagem 
    ///</Summary>
    public static class LocalizationConfiguration
    {
        ///<Summary>
        /// Adiciona linguagem padrão PT-BR
        ///</Summary>
        public static void UseLocalizationConfiguration(this IApplicationBuilder app)
        {
            var supportedCultures = new[]{
                new CultureInfo("pt-BR")
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("pt-BR"),
                SupportedCultures = supportedCultures,
                FallBackToParentCultures = false
            });

            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CreateSpecificCulture("pt-BR");
        }
    }
}

