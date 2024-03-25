using System;
using ThrottlR;

namespace POC.API.Configurations
{
    public static class ThrottleConfiguration
    {
        public static WebApplicationBuilder AddThrottleConfiguration(this WebApplicationBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder.Services.AddThrottlR(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy
                       .WithIpResolver()
                       .WithGeneralRule(TimeSpan.FromSeconds(1), 100)  
                       .WithGeneralRule(TimeSpan.FromMinutes(1), 2000) 
                       .WithGeneralRule(TimeSpan.FromHours(1), 50000); 
                });
            })
            .AddInMemoryCounterStore();

            return builder;
        }
    }
}

