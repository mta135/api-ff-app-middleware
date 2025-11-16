using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FFAppMiddleware.API.SwaggerSchemaFilter
{
    public class NullableDefaultSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema.Type == "array")
            {
                schema.Default = new OpenApiArray();
                //schema.Nullable = false; // Forțăm să nu fie nullable
            }
            else if (schema.Nullable)
            {
                schema.Default = new OpenApiNull();
            }
        }
    }
}
