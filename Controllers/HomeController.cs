using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using P1_EDD_DAVH_AFPE.Models;
using P1_EDD_DAVH_AFPE.Models.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using DataStructures;

namespace P1_EDD_DAVH_AFPE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Configuration(IFormCollection collection)
        {
            Singleton.Instance.HeapPacient = new Heap<PacientModel>(Singleton.Instance.maxPacient);
            Singleton.Instance.Data = new HashTable<PacientModel, int>(Singleton.Instance.maxLength);
            return RedirectToAction(nameof(Index),"Pacient");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
