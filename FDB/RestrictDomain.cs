
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;




public class RestrictDomainAttribute : Attribute, IAuthorizationFilter
{
    public IEnumerable<string> AllowedHosts { get; }

    public RestrictDomainAttribute(params string[] allowedHosts) => AllowedHosts = allowedHosts;

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        //  Get host from the request and check if it's in the enumeration of allowed host


        if (!AllowedHosts.Contains("https://feedback-webapp.azurewebsites.net", StringComparer.OrdinalIgnoreCase))
        {
            //  Request came from an authorized host, return bad request
            context.Result = new BadRequestObjectResult("Host is not allowed");
        }
    }
}