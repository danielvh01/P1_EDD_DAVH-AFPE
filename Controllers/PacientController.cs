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
            if(Singleton.Instance.startingDate == null)
            {
                Singleton.Instance.startingDate = collection["startingDate"];
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
            bool verif2 = false;
            int hour = 8;
            int min = 0;
            int year = 0;
            int month = 0;
            int day = 0;
            string datetime = "";
            string minhour = "";
            string dayMonth = "";
            if (Singleton.Instance.verifAppointmen)
            {
                int spacer1 = Singleton.Instance.startingDate.IndexOf(":");
                int spacer2 = Singleton.Instance.startingDate.IndexOf("-");
                if (spacer1 == 2 && hour > 9)
                {
                    hour = int.Parse(Singleton.Instance.startingDate.Substring(0, 2));
                }
                else
                {
                    hour = int.Parse(Singleton.Instance.startingDate.Substring(1, 1));
                }
                if (spacer2 == 6 && min > 9)
                {
                    min = int.Parse(Singleton.Instance.startingDate.Substring(spacer2 - 2, 2));
                }
                else 
                {
                    min = int.Parse(Singleton.Instance.startingDate.Substring(spacer2 - 1, 1));
                }
                year = int.Parse(Singleton.Instance.startingDate.Substring(spacer2 + 7, 4));
                if (month > 9)
                {
                    month = int.Parse(Singleton.Instance.startingDate.Substring(spacer2 + 4, 2));
                }
                else
                {
                    month = int.Parse(Singleton.Instance.startingDate.Substring(spacer2 + 5, 1));
                }
                if (day > 9)
                {
                    day = int.Parse(Singleton.Instance.startingDate.Substring(spacer2 + 2, 1));
                }
                else
                {
                    day = int.Parse(Singleton.Instance.startingDate.Substring(spacer2 + 1, 2));
                }
            }
            else
            {
                day = int.Parse(Singleton.Instance.startingDate.Substring(8, 2));
                month = int.Parse(Singleton.Instance.startingDate.Substring(5, 2));
                year = int.Parse(Singleton.Instance.startingDate.Substring(0, 4));
            }
            

            if (Singleton.Instance.HeapPacient.heapArray.Length > 0)
            {
                for (int a = 0; a < Singleton.Instance.HeapPacient.heapArray.Length; a++)
                {
                    datetime = hour + ":" + min + "-" + day + "/" + month + "/" + year;                                            
                    if (min < 10)
                    {
                        minhour = hour + ":" + "0" + min ;
                        verif2 = true;
                    }
                    if(hour < 10)
                    {                        
                        minhour = "+" + hour + ":" + min;
                        verif2 = true;
                    }                    
                    if (hour < 10 && min < 10)
                    {
                        minhour = "0" +hour + ":" + "0" + min;
                        verif2 = true;
                    }
                    if (day < 10)
                    {
                        dayMonth = "-" + "0"+  day + "/" + month + "/" + year;
                        verif2 = true;
                    }
                    if (month < 10)
                    {
                        dayMonth = "-" + day + "/" + "0"+ month + "/" + year;
                        verif2 = true;
                    }
                    if (day < 10 && month < 10)
                    {
                        dayMonth = "-" +"0"+ day + "/" + "0" + month + "/" + year;
                        verif2 = true;
                    }
                    if (verif2)
                    {
                        datetime = minhour + dayMonth;
                    }
                    if (a % Singleton.Instance.simmultaneous != 0 || a == 0)
                    {
                        if (Singleton.Instance.HeapPacient.heapArray.Get(a).value.schedule == "No asignado todavia")
                        {
                            Singleton.Instance.HeapPacient.heapArray.Get(a).value.schedule = datetime;
                            
                            verif = true;                            
                        }
                    }
                    else
                    {
                        if (Singleton.Instance.HeapPacient.heapArray.Get(a).value.schedule == "No asignado todavia")
                        {
                            if ((min + Singleton.Instance.schedule) < 60)
                            {
                                min += Singleton.Instance.schedule;
                            }
                            else
                            {
                                min += Singleton.Instance.schedule - 60;
                                hour++;
                            }
                            if (hour >= 17)
                            {
                                hour = 8;
                                day++;
                            }
                            if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10)
                            {
                                if (day >= 31)
                                {
                                    day = 1;
                                    month++;
                                }
                            }
                            if (month == 4 || month == 6 || month == 9 || month == 11)
                            {
                                if (day >= 30)
                                {
                                    day = 1;
                                    month++;
                                }
                            }
                            if (month == 2)
                            {
                                if (day >= 28)
                                {
                                    day = 1;
                                    month++;
                                }
                            }
                            if (month >= 12)
                            {
                                month = 1;
                                year++;
                            }
                            verif = true;
                        }
                        
                    }
                }
                Singleton.Instance.startingDate = datetime;
                Singleton.Instance.verifAppointmen = true;
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
                TempData["testmsg"] = "No hay ninguna persona en lista de espera.";
            }
            return RedirectToAction(nameof(Index));
        }

        //This method is called when a person didn't assist in the scheduled hour in order to assign a new one with different date.
        public ActionResult SReschedule(HeapNode<PacientModel>  pacient)
        {
            int spacer = Singleton.Instance.startingDate.IndexOf("-");
            int hour = 8;
            int min = 0;
            int year = int.Parse(Singleton.Instance.startingDate.Substring(spacer+8, 2))+1;
            int month = int.Parse(Singleton.Instance.startingDate.Substring(spacer+5, 2));
            int day = int.Parse(Singleton.Instance.startingDate.Substring(spacer+0, 4));  
            string date = date = hour + ":" + min + day + "/" + month + "/" + year;
            Singleton.Instance.HeapPacient.heapArray.Get(Singleton.Instance.HeapPacient.heapArray.GetPositionOf(pacient)).value.schedule = date;
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
            try
            {
                string pr = collection["priority"];
                var newPacient = new PacientModel
                {
                    Name = collection["Name"],
                    LastName = collection["LastName"],
                    DPI = Convert.ToInt32(collection["DPI"]),
                    Department = HttpContext.Session.GetString(SessionDepartment),
                    municipality = HttpContext.Session.GetString(SessionMunicipality),
                    priority = Singleton.Instance.priorityAssign(pr),                    
                    schedule = "No asignado todavia",
                    vaccinated = false
                };
                Singleton.Instance.Agregar(newPacient);
                Data();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
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
