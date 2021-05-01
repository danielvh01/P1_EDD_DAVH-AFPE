using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using P1_EDD_DAVH_AFPE.Models;
using P1_EDD_DAVH_AFPE.Models.Data;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using DataStructures;
using System;

namespace P1_EDD_DAVH_AFPE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        const string SessionMunicipality = "_Municipality";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        //Assign the actual login to identify the data 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(IFormCollection collection)
        {
            if(Singleton.Instance.municipalities.Find(collection["search"]) != null)
            {
                HttpContext.Session.SetString(SessionMunicipality, collection["search"]);
                return RedirectToAction(nameof(Index), "Pacient");
            }
            else
            {
                return View();
            }
        }

        public IActionResult Configuration()
        {
            if (System.IO.File.Exists("./Database.txt"))
            {
                var lectorlinea = new StreamReader("./Database.txt");
                string line = lectorlinea.ReadToEnd();
                string[] obj = line.Split("\n");
                for (int i = 0; i < obj.Length; i++)
                {
                    int spacer = obj[i].IndexOf(":");
                    if (obj[i].Substring(0, spacer) == "heapCapacity")
                    {
                        Singleton.Instance.heapCapacity = Convert.ToInt32(obj[i].Substring(spacer + 1));
                        Singleton.Instance.HeapPacient = new Heap<PacientModel>(Singleton.Instance.heapCapacity);
                    }
                    if (obj[i].Substring(0, spacer) == "hashCapacity")
                    {
                        Singleton.Instance.hashCapacity = Convert.ToInt32(obj[i].Substring(spacer + 1));
                        Singleton.Instance.Data = new HashTable<PacientModel, int>(Singleton.Instance.hashCapacity);
                    }
                    if (obj[i].Substring(0, spacer) == "pacients")
                    {
                        string[] tasks = obj[i].Substring(spacer + 1).Split(";");

                        for (int j = 0; j < tasks.Length; j++)
                        {
                            string[] obj2 = tasks[j].Split(",");
                            if (obj2.Length == 6)
                            {
                                var newPacient = new PacientModel
                                {
                                    Name = obj2[0],
                                    LastName = obj2[1],
                                    DPI = Convert.ToInt32(obj2[2]),
                                    Department = obj2[4],
                                    municipality = obj2[3],
                                    priority = obj2[5],
                                    age = Convert.ToInt32(obj2[6]),
                                    schedule = obj2[7]
                                };
                                Singleton.Instance.HeapPacient.insertKey(newPacient, newPacient.priority);
                                Singleton.Instance.Data.Add(newPacient, Singleton.Instance.keyGen(newPacient.DPI));
                                Singleton.Instance.Agregar(newPacient);
                            }
                        }
                    }
                }
                lectorlinea.Close();

                return View();
            }
            else
            {
                return RedirectToAction(nameof(Configuration));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Configuration(IFormCollection collection)
        {
            Singleton.Instance.HeapPacient = new Heap<PacientModel>(Singleton.Instance.heapCapacity);
            Singleton.Instance.Data = new HashTable<PacientModel, int>(Singleton.Instance.hashCapacity);
            FileStream fs = new FileStream("./Database.txt", FileMode.Create, FileAccess.Write);
            Singleton.Instance.BuildData();
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(Singleton.Instance.database);
            sw.Close();
            fs.Close();
            return RedirectToAction(nameof(Login));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
