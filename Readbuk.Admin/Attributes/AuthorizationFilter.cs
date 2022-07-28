using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Readbuk.Admin.Attributes
{
    public class AuthorizationFilter : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            string token = filterContext.HttpContext.Session.GetString("token");

            if (string.IsNullOrEmpty(token))
                filterContext.Result = new RedirectToRouteResult( new RouteValueDictionary{{ "controller", "Auth" }, { "action", "Index" }});
        }

    }
}

