using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using P1_EDD_DAVH_AFPE.Models;
using P1_EDD_DAVH_AFPE.Models.Data;
using DataStructures;
using System.IO;
using Microsoft.AspNetCore.Hosting;
namespace P1_EDD_DAVH_AFPE.Controllers

{
    public class PacientController : Controller
    {
        string session;
        private readonly IHostingEnvironment hostingEnvironment;

        //Asignacion de claves de sesión
        const string SessionMunicipality = "_Municipality";
        const string SessionDepartment = "_Department";

        public PacientController(IHostingEnvironment hostingEnvironment)
        {
            session = "Database.txt";
            this.hostingEnvironment = hostingEnvironment;
        }
        //Get de la vista Index
        public ActionResult Index()
        {
            //Manda a la vista los datos de la session
            ViewBag.Department = HttpContext.Session.GetString(SessionDepartment);
            ViewBag.Municipality = HttpContext.Session.GetString(SessionMunicipality);
            return View();
        }

        //Get de la view SMenu
        [HttpGet]
        public ActionResult SMenu()
        {
            return View();
        }
        //Post de la view SMenu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SMenu(IFormCollection collection)
        {
            //Asignacion de la configuración para la calendarización
            Singleton.Instance.simmultaneous = int.Parse(collection["simmultaneous"]);
            Singleton.Instance.schedule = int.Parse(collection["schedule"]);
            //Verifica si ya hay una fecha de inicio en la calendarización previa, si no la, asigna la indicada por el usuario
            if(Singleton.Instance.startingDate.CompareTo(new DateTime()) == 0)
            {
                string d = collection["startingDate"];
                string[] date = d.Split("-");
                Singleton.Instance.startingDate = Singleton.Instance.startingDate.AddYears(int.Parse(date[0]) - 1);
                Singleton.Instance.startingDate = Singleton.Instance.startingDate.AddMonths(int.Parse(date[1]) - 1);
                Singleton.Instance.startingDate = Singleton.Instance.startingDate.AddDays(int.Parse(date[2]) - 1);
                Singleton.Instance.startingDate = Singleton.Instance.startingDate.AddHours(8);
            }
            //Se comienza a generar la calendarización a todos los registros sin asignar fecha,
            //finalmente devuleve un mensaje con el resultado del proceso
            bool verif = false;
            if (Singleton.Instance.HeapPacient.Length() > 0)
            {
                for (int i = 0; i < Singleton.Instance.HeapPacient.Length(); i++)
                {
                    if (Singleton.Instance.HeapPacient.heapArray.Get(i).value.schedule.CompareTo(new DateTime()) == 0)
                    {
                        Singleton.Instance.HeapPacient.heapArray.Get(i).value.schedule = Singleton.Instance.NextDate(i);
                        verif = true;
                    }
                }
                Data();
                if (verif)
                {
                    TempData["testmsg"] = "Calendarizacion generada correctamente.";
                }
                else
                {
                    TempData["testmsg"] = "Todas las personas de la lista de espera ya tenian un horario establecido";
                }
            }
            else
            {
                TempData["testmsg"] = "No hay personas en la lista de espera";
            }
            return RedirectToAction(nameof(Simulation));
        }

        //Get de la view SIndex
        public ActionResult SIndex()
        {
            //Genera la lista de espera con los registros de el municipio indicado y la manda a la vista
            Singleton.Instance.genWaitingList(HttpContext.Session.GetString(SessionMunicipality));
            return View(Singleton.Instance.HeapPacient);
        }

        //Get de la view Simulation
        public ActionResult Simulation()
        {
            //Genera la lista de espera con los registros de el municipio indicado y la manda a la vista
            Singleton.Instance.genWaitingList(HttpContext.Session.GetString(SessionMunicipality));
            return View(Singleton.Instance.HeapPacient);
        }
        //Get de la view VaccinatedList
        public ActionResult VaccinatedList()
        {
            //Manda a la vista la lista de vacunados
            return View(Singleton.Instance.VaccinatedList);
        }
        //Get de la view Percentage
        public ActionResult Percentage()
        {
            //Si existen registros calcula el porcentaje de vacunados para mandarla a la vista
            if (Singleton.Instance.Data.Length > 0)
            {
                double p = double.Parse(Singleton.Instance.VaccinatedList.Length.ToString()) / double.Parse(Singleton.Instance.Data.Length.ToString()) * 100;
                ViewBag.Percentage = p.ToString("N2");
            }
            return View();
        }
        //Get de la view List
        public ActionResult List()
        {
            //Manda a la vista todos los registros
            return View(Singleton.Instance.Data.GetAllElements());
        }
        //Post de la view List
        [HttpPost]
        public ActionResult List(IFormCollection collection)
        {
            //Este método realiza la búsqueda de un registro, determinado en primer lugar en base a que propiedad realizará
            //la búsqueda, posterior pasará a realizar la búsqueda del elemento, mostrará un mensaje en caso de no encontrarlo
            if (collection["criteria"] == "Dpi")
            {
                var criteria = Singleton.Instance.DpiTree.Find(x => x.value.CompareTo(long.Parse(collection["Search"])), Singleton.Instance.DpiTree.Root);
                if (criteria != null)
                {
                    return View("SearchResult", Singleton.Instance.Data.Get(x => x.DPI.CompareTo(criteria.value), Singleton.Instance.keyGen(criteria.key)));
                }
                else
                {
                    TempData["testmsg"] = "No se encontro ninguna coincidencia";
                    return RedirectToAction(nameof(List));
                }
            }
            else if(collection["criteria"] == "Name")
            {
                var criteria = Singleton.Instance.NameTree.Find(x => x.value.CompareTo(collection["Search"]), Singleton.Instance.NameTree.Root);
                if (criteria != null)
                {
                    return View("SearchResult", Singleton.Instance.Data.Get(x => x.Name.CompareTo(criteria.value), Singleton.Instance.keyGen(criteria.key)));
                }
                else
                {
                    TempData["testmsg"] = "No se encontro ninguna coincidencia";
                    return RedirectToAction(nameof(List));
                }
            }
            else if(collection["criteria"] == "LastName")
            {
                var criteria = Singleton.Instance.LastNameTree.Find(x => x.value.CompareTo(collection["Search"]), Singleton.Instance.LastNameTree.Root);
                if (criteria != null)
                {
                    return View("SearchResult", Singleton.Instance.Data.Get(x => x.LastName.CompareTo(criteria.value), Singleton.Instance.keyGen(criteria.key)));
                }
                else
                {
                    TempData["testmsg"] = "No se encontro ninguna coincidencia";
                    return RedirectToAction(nameof(List));
                }
            }
            else
            {
                //En caso de no elegir un criterio de búsqueda (Caso supuestamente imposible) regresará la vista con todos los elementos de nuevo
                return View(Singleton.Instance.Data.GetAllElements());
            }
        }

        

        //Método que marca que una persona no asistió a su cita
        public ActionResult NotAssist()
        {
            //Obtiene el primer elemento en la lista, el cual es quien se marcará como ausente y muestra la vista de confirmación
            var result = Singleton.Instance.HeapPacient.getMin();
            return View(result);
        }

        //Método que reasigna la fecha de vacunación de un registro
        public ActionResult SReschedule(PacientModel  pacient)
        {
            var x = Singleton.Instance.HeapPacient.extractMin().value;
            x.schedule = Singleton.Instance.NextDate(Singleton.Instance.HeapPacient.Length() - 1);
            Singleton.Instance.HeapPacient.insertKey(x,x.priority);
            return RedirectToAction(nameof(Simulation));
        }

        //Get de la view Create
        public ActionResult Create()
        {
            //Manda a la vista los datos de la session
            ViewBag.depa = HttpContext.Session.GetString(SessionDepartment);
            ViewBag.muni = HttpContext.Session.GetString(SessionMunicipality);
            return View();
        }
        //Post de la view Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            //Crea un nuevo Registro y le asigna una prioridad según los datos indicados, también
            //valida si el dpi indicado es válido, de lo contrario mostrará un mensaje de error
            string pr = collection["priority"];
            int age = int.Parse(collection["age"]);
            long dpi = Convert.ToInt64(collection["DPI"]);
            if(Singleton.Instance.cuiValid(dpi))
            {
                var newPacient = new PacientModel
                {
                    Name = collection["Name"],
                    LastName = collection["LastName"],
                    DPI = dpi,
                    Department = HttpContext.Session.GetString(SessionDepartment),
                    municipality = HttpContext.Session.GetString(SessionMunicipality),
                    schedule = new DateTime(),
                    vaccinated = false
                };
                if (pr == "Area de salud")
                {
                    //Si es del area de salud pasará a otra vista que pregunta mas datos para determinar la prioridad
                    return RedirectToAction(nameof(WorkArea), newPacient);
                }
                else if (pr == "Trabajador de funeraria o de institucion del adulto mayor" || pr == "Cuerpo de socorro")
                {
                    newPacient.priority = "1d";
                }
                else if (pr == "Persona internada en hogar o institucion del adulto mayor")
                {
                    newPacient.priority = "1e";
                }
                else
                {
                    if (collection["q3"] == "No" && age < 70)
                    {
                        switch (pr)
                        {
                            case "Otros":
                                if (age < 40)
                                {
                                    newPacient.priority = "4b";
                                }
                                else if (age < 50)
                                {
                                    newPacient.priority = "4a";
                                }
                                break;
                            case "Area educativa":
                                newPacient.priority = "3c";
                                break;
                            case "Area de justicia":
                                newPacient.priority = "3d";
                                break;
                            case "Entidad de servivios esenciales":
                                newPacient.priority = "3b";
                                break;
                            case "Area de seguridad nacional":
                                newPacient.priority = "3a";
                                break;
                        }
                    }
                    else
                    {
                        newPacient.priority = "2a";
                    }
                }
                Singleton.Instance.Agregar(newPacient);
                Data();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["testmsg"] = "DPI no valido, intentelo de nuevo";
                return View();
            }
        }
        //Get de la view WorkArea
        public ActionResult WorkArea(PacientModel pacient)
        {
            //Manda los valores del nuevo registro
            return View(pacient);
        }
        //Post de la view WorkArea
        [HttpPost]
        public ActionResult WorkArea(IFormCollection collection)
        {
            //Obtiene los datos indicados por el usuario, asigna una prioridad en base a estos y lo añade a la lista de espera
            long dpi = long.Parse(collection["dpi"]);
            var pacient = new PacientModel
            {
                Name = collection["name"],
                LastName = collection["lastname"],
                DPI = dpi,
                Department = collection["department"],
                municipality = collection["municipality"],
                schedule = new DateTime(),
                vaccinated = false,
            };
            string q1 = collection["q1"];
            string q2 = collection["q2"];
            string q3 = collection["q3"];
            if(q1 == "Si")
            {
                pacient.priority = "1a";
            }
            else if(q2 == "Si")
            {
                pacient.priority = "1c";
            }
            else if (q3 == "Si")
            {
                pacient.priority = "1f";
            }
            else
            {
                pacient.priority = "1b";
            }
            Singleton.Instance.Agregar(pacient);
            Data();
            return RedirectToAction(nameof(Index));
        }

        //Get de la view Delete
        public ActionResult Delete()
        {
            //Obtiene el primer elemento en la lista, el cual es quien se marcará como vacunado y muestra la vista de confirmación
            var result = Singleton.Instance.HeapPacient.getMin();
            return View("VaccinatedCheck", result);
        }
        //Método que marca como vacunado un registro
        public ActionResult Vaccinated()
        {
            var x = Singleton.Instance.HeapPacient.extractMin().value;
            x.vaccinated = true;
            Singleton.Instance.VaccinatedList.InsertAtEnd(x);
            Data();
            return RedirectToAction(nameof(Simulation));
        }

        //Metodo que actualiza el archivo de texto con los datos actuales en el programa
        public ActionResult Data()
        {
            Singleton.Instance.BuildData();
            StreamWriter file = new StreamWriter(session, false);
            file.Write(Singleton.Instance.database);
            file.Close();
            return RedirectToAction(nameof(Index));
        }
    }
}
