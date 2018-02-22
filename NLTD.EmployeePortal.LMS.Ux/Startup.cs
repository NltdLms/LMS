using Hangfire;
using Hangfire.Dashboard;
using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

[assembly: OwinStartup(typeof(NLTD.EmployeePortal.LMS.Ux.Startup))]
namespace NLTD.EmployeePortal.LMS.Ux
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configuration
                .UseSqlServerStorage(ConfigurationManager.ConnectionStrings["LmsdbConn"].ConnectionString);

            //Fire and Forget


           // app.UseHangfireDashboard();

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                AuthorizationFilters = new[] { new HangfireAuthorizationFilter() }
            });

            app.UseHangfireServer();
        }
    }

    public class HangfireAuthorizationFilter : IAuthorizationFilter
    {
        public bool Authorize(IDictionary<string, object> owinEnvironment)
        {
            var context = new OwinContext(owinEnvironment);
            return context.Authentication.User.Identity.IsAuthenticated; //&& context.Authentication.User.IsInRole("Employee");
        }
    }
}