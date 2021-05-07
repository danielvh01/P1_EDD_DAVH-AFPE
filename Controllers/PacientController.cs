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
            Schedule();
            return RedirectToAction(nameof(SIndex));
        }

        //GET of waiting List
        public ActionResult SIndex()
        {            
            return View(Singleton.Instance.HeapPacient);
        }

        public ActionResult Simulation()
        {
            return View(Singleton.Instance.HeapPacient);
        }
        public ActionResult VaccinatedList()
        {
            return View(Singleton.Instance.VaccinatedList);
        }

        public ActionResult Percentage()
        {
            return View(Singleton.Instance.VaccinatedList);
        }

        // GET: PacientController/Details/5
        public ActionResult Details(int id)
        {
            PacientModel detailsPacient = null;
            return View();
        }

        

        //Method that is called in order to Schedule the appointments of pacients registered.
        public ActionResult Schedule()
        {
            bool verif = false;
            if (Singleton.Instance.HeapPacient.Length() > 0)
            {
                for(int i = 0; i < Singleton.Instance.HeapPacient.Length(); i++)
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
                    TempData["testmsg"] = "Calendarización generada correctamente.";
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
            return RedirectToAction(nameof(Index));
        }

        //This method is called when a person didn't assist in the scheduled hour in order to assign a new one with different date.
        public ActionResult SReschedule(HeapNode<PacientModel>  pacient)
        {
            //int spacer = Singleton.Instance.startingDate.IndexOf("-");
            //int hour = 8;
            //int min = 0;
            //int year = int.Parse(Singleton.Instance.startingDate.Substring(spacer+8, 2))+1;
            //int month = int.Parse(Singleton.Instance.startingDate.Substring(spacer+5, 2));
            //int day = int.Parse(Singleton.Instance.startingDate.Substring(spacer+0, 4));  
            //string date = date = hour + ":" + min + day + "/" + month + "/" + year;
            //Singleton.Instance.HeapPacient.heapArray.Get(Singleton.Instance.HeapPacient.heapArray.GetPositionOf(pacient)).value.schedule = date;
            return RedirectToAction(nameof(Simulation));
        }

        public ActionResult Vaccinated(HeapNode<PacientModel> pacient)
        {
            Singleton.Instance.HeapPacient.heapArray.Get(Singleton.Instance.HeapPacient.heapArray.GetPositionOf(pacient)).value.vaccinated = true;
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
            var newPacient = new PacientModel
            {
                Name = collection["Name"],
                LastName = collection["LastName"],
                DPI = Convert.ToInt64(collection["DPI"]),
                Department = HttpContext.Session.GetString(SessionDepartment),
                municipality = HttpContext.Session.GetString(SessionMunicipality),
                priority = Singleton.Instance.priorityAssign(pr),                    
                schedule = new DateTime(),
                vaccinated = false
            };
            Singleton.Instance.Agregar(newPacient);
            Data();
            return RedirectToAction(nameof(Index));
        }

        // GET: PacientController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PacientController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PacientController/Delete/5
        public ActionResult Delete(int dpi)
        {
            PacientModel pacient = new PacientModel();
            return View(pacient);
        }

        // POST: PacientController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
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
