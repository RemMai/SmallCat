using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SmallCat.Swagger.Filter;

internal class SmallCatSecurityRequirementsOperationFilter : IOperationFilter
{
    private readonly SecurityRequirementsOperationFilter<AuthorizeAttribute> _filter;

    public SmallCatSecurityRequirementsOperationFilter(bool includeUnauthorizedAndForbiddenResponses = true, string securitySchemaName = "oauth2")
    {
        IEnumerable<string> PolicySelector(IEnumerable<AuthorizeAttribute> authAttributes) => from a in authAttributes where !string.IsNullOrEmpty(a.Policy) select a.Policy;
        _filter = new SecurityRequirementsOperationFilter<AuthorizeAttribute>(PolicySelector, includeUnauthorizedAndForbiddenResponses, securitySchemaName);
    }


    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        _filter.Apply(operation, context);
    }
}