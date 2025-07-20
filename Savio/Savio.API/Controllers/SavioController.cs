using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Savio.API.Controllers
{
    public class SavioController : Controller
    {
        // GET: Savio
        public ActionResult Index()
        {
            return View();
        }
    }
}