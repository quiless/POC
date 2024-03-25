using System;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace POC.API.Configurations
{
    public class FileFormOperation : IOperationFilter
    {
        [AttributeUsage(AttributeTargets.Method)]
        public class FileContent : Attribute
        {

        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {


            var uploadFileMediaType = new OpenApiMediaType()
            {
                Schema = new OpenApiSchema()
                {
                    Type = "object",
                    Properties =
                {
                    ["DriverLicenseFile"] = new OpenApiSchema()
                    {
                        Description = "CNH do motorista",
                        Type = "file",
                        Format = "formData"
                    }
                },
                    Required = new HashSet<string>() { "DriverLicenseFile" }
                }
            };

            operation.RequestBody = new OpenApiRequestBody
            {
                Content = { ["multipart/form-data"] = uploadFileMediaType }
            };
        }
    }
}

