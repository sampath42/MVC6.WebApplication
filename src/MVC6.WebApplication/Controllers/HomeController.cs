using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Cors;

namespace MVC6.WebApplication.Controllers
{
    [EnableCors("AllowOrigin1")]
    public class HomeController : Controller
    {
        private IMemoryCache cache;

        public HomeController(IMemoryCache cache)
        {
            this.cache = cache;
        }

        [EnableCors("AllowOrigin1")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
