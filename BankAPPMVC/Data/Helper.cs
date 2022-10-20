using BankApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace BankApp.Data
{
    public class Helper
    {
        public static string RenderViewToString(Controller controller, string viewName, object model = null)
        {
            controller.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                IViewEngine viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
                ViewEngineResult viewResult = viewEngine.FindView(controller.ControllerContext, viewName, false);

                ViewContext viewContext = new ViewContext(
                    controller.ControllerContext,
                    viewResult.View,
                    controller.ViewData,
                    controller.TempData,
                    sw,
                    new HtmlHelperOptions()
                );
                viewResult.View.RenderAsync(viewContext);
                return sw.GetStringBuilder().ToString();
            }
        }


        public static void InitializeData(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<BankContext>();
             
            if (!context.Users.Any())
            {
                var users = new List<User>
                {
                   new User
                   {
                       userId = "User_1",// for test only  Guid.NewGuid().ToString("N").ToUpper(),
                       name = "Nader Ssam",
                       date_created = DateTime.Now
                   }
                };
                context.AddRange(users);
                context.SaveChangesAsync();
            }
        }
    }
}
