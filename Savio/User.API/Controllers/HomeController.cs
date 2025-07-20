using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace User.API.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyName = assembly.GetName();
            var version = assemblyName.Version;

            var buildDate = DateTime.Now;

            string message = $"MoneyRecord API Service is Running\n" +
                             $"Assembly: {assemblyName.Name}\n" +
                             $"Version: {version}\n" +
                             $"Build Date: {buildDate}";

            return Content(message, "text/plain");
        }
    }
}
