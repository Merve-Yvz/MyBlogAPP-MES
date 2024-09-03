﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlogAPP.Services
{
    public class JwtAuthenticationAttribute : ActionFilterAttribute
    {
        private readonly Authentication _authentication;

        public JwtAuthenticationAttribute(Authentication authentication)
        {
            _authentication = authentication;
        }
        
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;
            var token = request.Cookies["jwt"];

            if (!string.IsNullOrEmpty(token))
            {
                var userName = _authentication.ValidateToken(token);
                if (string.IsNullOrEmpty(userName))
                {
                    context.Result = new UnauthorizedResult(); 
                }
            }
            else
            {
                context.Result = new UnauthorizedResult();
            }

            base.OnActionExecuting(context);
        }
    }
}
