using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SmartCat.Swagger.Filter;

internal class SmartCatSecurityRequirementsOperationFilter : IOperationFilter
{
    private readonly SecurityRequirementsOperationFilter<AuthorizeAttribute> _filter;

    public SmartCatSecurityRequirementsOperationFilter(bool includeUnauthorizedAndForbiddenResponses = true, string securitySchemaName = "oauth2")
    {
        IEnumerable<string> PolicySelector(IEnumerable<AuthorizeAttribute> authAttributes) => from a in authAttributes where !string.IsNullOrEmpty(a.Policy) select a.Policy;
        _filter = new SecurityRequirementsOperationFilter<AuthorizeAttribute>(PolicySelector, includeUnauthorizedAndForbiddenResponses, securitySchemaName);
    }


    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        _filter.Apply(operation, context);
    }
}