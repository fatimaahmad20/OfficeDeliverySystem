
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace deliverySystem.Authentication
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected virtual IPrincipal CurrentUser
        {
            get { return HttpContext.Current.User; }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return (CurrentUser == null || CurrentUser.IsInRole(Roles)) && CurrentUser != null;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            RedirectToRouteResult routeData;

            if (CurrentUser == null)
            {
                routeData = new RedirectToRouteResult
                    (new System.Web.Routing.RouteValueDictionary
                    (new
                    {
                        controller = "Auth",
                        action = "Login",
                    }
                    ));
            }
            else
            {
                routeData = new RedirectToRouteResult
                (new System.Web.Routing.RouteValueDictionary
                 (new
                 {
                     controller = "Error",
                     action = "AccessDenied"
                 }
                 ));
            }

            filterContext.Result = routeData;
        }
    }
}