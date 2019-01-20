using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using dbBus.AspNet.Models;
using dbBus.Core;

namespace dbBus.AspNet.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBus bus;

        public HomeController(IBus bus)
        {
            this.bus = bus;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SendMessage()
        {
            this.bus.Publish(new MyMessage());
            return this.RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
