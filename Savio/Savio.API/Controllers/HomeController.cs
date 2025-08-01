﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Savio.API.Controllers
{
    [ApiController]
    [Route("/")]
    public class HomeController : ControllerBase
    {
        public ActionResult Index()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyName = assembly.GetName();
            var version = assemblyName.Version;

            var buildDate = DateTime.Now;

            string message = $"Savio API Service is Running\n" +
                             $"Assembly: {assemblyName.Name}\n" +
                             $"Version: {version}\n" +
                             $"Build Date: {buildDate}";

            return Content(message, "text/plain");
        }
    }
}
