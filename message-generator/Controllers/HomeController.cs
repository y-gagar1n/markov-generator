using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MarkovGen.Models;

namespace MarkovGen.Controllers
{
    public class HomeController : Controller
    {
        private static Generator generator;

        static HomeController()
        {
            generator = new Generator();
            generator.Init();
        }

        public ActionResult Index()
        {
            var example = generator.Generate();
            return Content(example);
        }

    }
}
