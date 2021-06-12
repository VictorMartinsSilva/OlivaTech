using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OlivaTech.Site.Extension
{
    public class CustomAuthorization
    {
        public static bool ValidationClaimsUser(HttpContext context, string ClaimName, string ClaimValue)
        {
            return context.User.Identity.IsAuthenticated &&
                   context.User.Claims.Any(c => c.Type == ClaimName && c.Value.Contains(ClaimValue));
        }
    }

    public class RequiredClaimFilter : IAuthorizationFilter
    {
        private readonly Claim _claim;
        public RequiredClaimFilter(Claim claim)
        {
            _claim = claim;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!CustomAuthorization.ValidationClaimsUser(context.HttpContext, _claim.Type, _claim.Value))
            {
                context.Result = new ForbidResult();
            }
        }

    }

    public class ClaimsAuthorizeAttribute : TypeFilterAttribute
    {
        public ClaimsAuthorizeAttribute(string ClaimName, string ClaimValue) :
            base(typeof(RequiredClaimFilter))
        {
            Arguments = new object[] { new Claim(ClaimName, ClaimValue) };
        }
    }
}
