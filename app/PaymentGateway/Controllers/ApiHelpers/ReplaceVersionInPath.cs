




using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PaymentGateway.Controllers.ApiHelpers {


public class ReplaceVersionsInPath : IDocumentFilter
{

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var existing = swaggerDoc.Paths
                .ToDictionary(
                    path => path.Key.Replace("{version}", swaggerDoc.Info.Version),
                    path => path.Value
                );

            var ps = new OpenApiPaths();
            foreach (var item in existing)
                ps.Add(item.Key, item.Value);

            swaggerDoc.Paths = ps;

        }
    }

}