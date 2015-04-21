using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using message_generator.Models;

namespace message_generator.Controllers
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
            //var generator = new Generator();
            //generator.Init();
            var example = generator.Generate();
            return Content(example);
            
        }

    }
}
