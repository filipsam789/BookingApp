using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace WebApplication16.Models
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class UserTypeAuthorizationAttribute : AuthorizeAttribute
    {
        private readonly Type _requiredUserType;

        public UserTypeAuthorizationAttribute(Type requiredUserType)
        {
            _requiredUserType = requiredUserType ?? throw new ArgumentNullException(nameof(requiredUserType));
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var user = httpContext.User as ClaimsPrincipal;

            if (user != null && user.Identity.IsAuthenticated)
            {
                // Check if the user's type matches the required type
                var userTypeClaim = user.FindFirst(ClaimTypes.UserData);

                if (userTypeClaim != null)
                {
                    var userType = Type.GetType(userTypeClaim.Value);
                    return userType != null && _requiredUserType.IsAssignableFrom(userType);
                }
            }

            return false;
        }
    }
}