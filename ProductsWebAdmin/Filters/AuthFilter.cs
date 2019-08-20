using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace ProductsWebAdmin.Filters
{
    public class AuthFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var authFailed = new RedirectToActionResult("Index", "Auth", null);
            if (!context.HttpContext.Request.Cookies.TryGetValue("token", out string token))
            {
                context.Result = authFailed;
            }
            else
            {
                if (string.IsNullOrEmpty(token))
                {
                    context.Result = authFailed;
                }

                // verify token
            }
        }
    }
}
