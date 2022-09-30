using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SmartCat.Swagger.Filter;

internal class SmartCatSecurityRequirementsOperationFilter : IOperationFilter
{
    private readonly SecurityRequirementsOperationFilter<AuthorizeAttribute> filter;

    public SmartCatSecurityRequirementsOperationFilter(bool includeUnauthorizedAndForbiddenResponses = true, string securitySchemaName = "oauth2")
    {
        Func<IEnumerable<AuthorizeAttribute>, IEnumerable<string>> policySelector = (IEnumerable<AuthorizeAttribute> authAttributes) => from a in authAttributes
                                                                                                                                        where !string.IsNullOrEmpty(a.Policy)
                                                                                                                                        select a.Policy;
        filter = new SecurityRequirementsOperationFilter<AuthorizeAttribute>(policySelector, includeUnauthorizedAndForbiddenResponses, securitySchemaName);
    }


    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        filter.Apply(operation, context);
    }
}
