using deliverySystem.Authentication;
using deliverySystem.App_Start;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.Http;
using System.Web.Routing;


namespace deliverySystem
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register); // for API
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies["Cookie1"];
            if (authCookie != null)
            {
                //FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                //var serializeModel = JsonConvert.DeserializeObject<CustomSerializeModel>(authTicket.UserData);
                //CustomPrincipal principal = new CustomPrincipal(authTicket.Name)
                //{
                //    UserId = serializeModel.UserId,
                //    Roles = new string[] { serializeModel.Role }
                //};

                var serializeModel = JsonConvert.DeserializeObject<CustomSerializeModel>(authCookie.Value);
                CustomPrincipal principal = new CustomPrincipal(serializeModel.Email)
                {
                    Email = serializeModel.Email,
                    Roles = new string[] { serializeModel.Role },
                    UserId = serializeModel.UserId
                };

                HttpContext.Current.User = principal;
            }
        }
    }
}
