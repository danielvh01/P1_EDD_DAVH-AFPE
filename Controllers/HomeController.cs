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
        const string SessionDepartment = "_Department";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        //Get de la view Help
        public IActionResult Help()
        {
            return View();
        }
        //Get de la view Index
        public IActionResult Index()
        {
            return View();
        }
        //Get de la view Login
        public IActionResult Login()
        {
            //Manda todos los departamentos a la vista de Login
            return View(Singleton.Instance.municipalities.GetAllKeys());
        }
        //Get de la view LoginM
        public IActionResult LoginM(string department)
        {
            //Asigna la variable de sesion Department
            HttpContext.Session.SetString(SessionDepartment,department);
            //Manda a la vista todos los municipios del Departamento elegido por el usuario
            return View(Singleton.Instance.municipalities.GetAllContent(department));
        }
        
        public IActionResult Session(string municipality)
        {
            //Este método asigna la variable de sesión Municipality y realiza el inicio
            //de sesión para la carga de datos
            HttpContext.Session.SetString(SessionMunicipality, municipality);
            Singleton.Instance.Login(HttpContext.Session.GetString(SessionMunicipality));
            return RedirectToAction(nameof(Index), "Pacient");
        }
        //Get de la view Configuration
        public IActionResult Configuration()
        {
            //Este método aplica las configuraciones básicas del programa, si hay unas previamente indicadas
            //las cargará al programa, de lo contrario mostrará la vista para que el usuario pueda asignarlas
            if (System.IO.File.Exists("./Database.txt"))
            {
                var lectorlinea = new StreamReader("./Database.txt");
                string line = lectorlinea.ReadToEnd();
                string[] obj = line.Split("\n");
                //Recorre todo el archivo de texto y asigna las propiedades al programa
                for (int i = 0; i < obj.Length; i++)
                {
                    int spacer = obj[i].IndexOf(":");
                    //Asigna la propiedad heapCapacity
                    if (obj[i].Substring(0, spacer) == "heapCapacity")
                    {
                        Singleton.Instance.heapCapacity = Convert.ToInt32(obj[i].Substring(spacer + 1));
                        Singleton.Instance.HeapPacient = new Heap<PacientModel>(Singleton.Instance.heapCapacity);
                    }
                    //Asigna la propiedad hashCapacity
                    if (obj[i].Substring(0, spacer) == "hashCapacity")
                    {
                        Singleton.Instance.hashCapacity = Convert.ToInt32(obj[i].Substring(spacer + 1));
                        Singleton.Instance.Data = new HashTable<PacientModel, long>(Singleton.Instance.hashCapacity);
                    }
                    //Carga todos los datos guardados previamente
                    if (obj[i].Substring(0, spacer) == "pacients")
                    {
                        string[] tasks = obj[i].Substring(spacer + 1).Split(";");

                        for (int j = 0; j < tasks.Length; j++)
                        {
                            string[] obj2 = tasks[j].Split(",");
                            if (obj2.Length == 8)
                            {
                                //Crea los objetos de tipo PacientModel
                                var newPacient = new PacientModel
                                {
                                    Name = obj2[0],
                                    LastName = obj2[1],
                                    DPI = Convert.ToInt64(obj2[2]),
                                    Department = obj2[3],
                                    municipality = obj2[4],
                                    priority = obj2[5],  
                                    vaccinated = bool.Parse(obj2[7])
                                };
                                string d = obj2[6];
                                string[] date = d.Split("/");
                                newPacient.schedule = newPacient.schedule.AddYears(int.Parse(date[2].Substring(0, 4)) - 1);
                                newPacient.schedule = newPacient.schedule.AddMonths(int.Parse(date[1]) - 1);
                                newPacient.schedule = newPacient.schedule.AddDays(int.Parse(date[0]) - 1);
                                newPacient.schedule = newPacient.schedule.AddHours(int.Parse(date[2].Substring(5,2)));
                                int separator = date[2].IndexOf(":");
                                newPacient.schedule = newPacient.schedule.AddMinutes(int.Parse(date[2].Substring(separator + 1, 2)));
                                if (newPacient.schedule.CompareTo(new DateTime()) != 0)
                                {
                                    if (Singleton.Instance.startingDate.CompareTo(newPacient.schedule) < 0)
                                    {
                                        Singleton.Instance.startingDate = newPacient.schedule;
                                    }
                                }
                                //Carga el dato a el programa
                                Singleton.Instance.AddDataBase(newPacient);
                            }
                        }
                    }
                }
                lectorlinea.Close();

                return RedirectToAction(nameof(Login));
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Configuration(IFormCollection collection)
        {
            //Si el programa se ejecuta por primera vez, crea todas las estructuras y asigna la configuración
            //que el usuario indicó
            Singleton.Instance.HeapPacient = new Heap<PacientModel>(Singleton.Instance.heapCapacity);
            Singleton.Instance.Data = new HashTable<PacientModel, long>(Singleton.Instance.hashCapacity);
            //Crea el archivo donde se guardarán los datos para las seisones futuras
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
