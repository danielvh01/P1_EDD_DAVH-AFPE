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

        const string SessionMunicipality = "_Municipality";
        const string SessionDepartment = "_Department";

        public PacientController(IHostingEnvironment hostingEnvironment)
        {
            session = "Database.txt";
            this.hostingEnvironment = hostingEnvironment;
        }
        // GET: PacientController
        public ActionResult Index()
        {
            ViewBag.Department = HttpContext.Session.GetString(SessionDepartment);
            ViewBag.Municipality = HttpContext.Session.GetString(SessionMunicipality);
            return View();
        }

        //GET of Simulation parameters
        [HttpGet]
        public ActionResult SMenu()
        {
            return View();
        }
        // POST: PacientController/SMenu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SMenu(IFormCollection collection)
        {
            Singleton.Instance.simmultaneous = int.Parse(collection["simmultaneous"]);
            Singleton.Instance.schedule = int.Parse(collection["schedule"]);
            if(Singleton.Instance.startingDate.CompareTo(new DateTime()) == 0)
            {
                string d = collection["startingDate"];
                string[] date = d.Split("-");
                Singleton.Instance.startingDate = Singleton.Instance.startingDate.AddYears(int.Parse(date[0]) - 1);
                Singleton.Instance.startingDate = Singleton.Instance.startingDate.AddMonths(int.Parse(date[1]) - 1);
                Singleton.Instance.startingDate = Singleton.Instance.startingDate.AddDays(int.Parse(date[2]) - 1);
                Singleton.Instance.startingDate = Singleton.Instance.startingDate.AddHours(8);
            }
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

        //GET of waiting List
        public ActionResult SIndex()
        {
            Singleton.Instance.genWaitingList(HttpContext.Session.GetString(SessionMunicipality));
            return View(Singleton.Instance.HeapPacient);
        }
        

        public ActionResult Simulation()
        {
            Singleton.Instance.genWaitingList(HttpContext.Session.GetString(SessionMunicipality));
            return View(Singleton.Instance.HeapPacient);
        }
        public ActionResult VaccinatedList()
        {
            return View(Singleton.Instance.VaccinatedList);
        }

        public ActionResult Percentage()
        {
            if (Singleton.Instance.Data.Length > 0)
            {
                double p = double.Parse(Singleton.Instance.VaccinatedList.Length.ToString()) / double.Parse(Singleton.Instance.Data.Length.ToString()) * 100;
                ViewBag.Percentage = p.ToString("N2");
            }
            return View();
        }

        // GET: PacientController/Details/5
        public ActionResult Details(int id)
        {
            PacientModel detailsPacient = null;
            return View();
        }

        public ActionResult List()
        {
            return View(Singleton.Instance.Data.GetAllElements());
        }
        [HttpPost]
        public ActionResult List(IFormCollection collection)
        {
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
                return View(Singleton.Instance.Data.GetAllElements());
            }
        }

        


        public ActionResult NotAssist()
        {
            var result = Singleton.Instance.HeapPacient.getMin().value;
            return View(result);
        }

        //This method is called when a person didn't assist in the scheduled hour in order to assign a new one with different date.
        public ActionResult SReschedule(PacientModel  pacient)
        {
            var x = Singleton.Instance.HeapPacient.extractMin().value;
            x.schedule = Singleton.Instance.NextDate(Singleton.Instance.HeapPacient.Length() - 1);
            Singleton.Instance.HeapPacient.insertKey(x, x.priority);
            return RedirectToAction(nameof(Simulation));
        }        

        // GET: PacientController/Create
        public ActionResult Create()
        {
            ViewBag.depa = HttpContext.Session.GetString(SessionDepartment);
            ViewBag.muni = HttpContext.Session.GetString(SessionMunicipality);
            return View();
        }
        // POST: PacientController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {

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
        public ActionResult WorkArea(PacientModel pacient)
        {
            return View(pacient);
        }

        [HttpPost]
        public ActionResult WorkArea(IFormCollection collection)
        {
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

        // GET: PacientController/Delete/5
        public ActionResult Delete()
        {
            var result = Singleton.Instance.HeapPacient.getMin();
            return View("VaccinatedCheck", result.value);
        }

        public ActionResult Vaccinated()
        {
            var x = Singleton.Instance.HeapPacient.extractMin().value;
            x.vaccinated = true;
            Singleton.Instance.VaccinatedList.InsertAtEnd(x);
            Data();
            return RedirectToAction(nameof(Simulation));
        }


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
