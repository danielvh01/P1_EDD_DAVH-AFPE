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
        public PacientController(IHostingEnvironment hostingEnvironment)
        {
            session = "Database.txt";
            this.hostingEnvironment = hostingEnvironment;
        }
        // GET: PacientController
        public ActionResult Index()
        {
            //
            return View();
        }
        //GET of waiting List
        public ActionResult SIndex()
        {
            return View(Singleton.Instance.WaitingList);
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

        // GET: PacientController/Create
        public ActionResult Create()
        {
            return View();
        }

        //Method that is called in order to Schedule the appointments of pacients registered.
        public ActionResult Schedule()
        {            
            bool check = false;
            Random r = new Random();            
            string date;
            int day = r.Next(1, 28);
            int month = r.Next(1, 12);
            int year = 2021;
            for (int a = 1; a < Singleton.Instance.WaitingList.Length +1; a++)
            {
                if (Singleton.Instance.WaitingList.Length > 1)
                {                    
                    if (Singleton.Instance.WaitingList.Get(a).schedule == Singleton.Instance.WaitingList.Get(a - 1).schedule)
                    {
                        Singleton.Instance.Cont++;
                    }
                    if (Singleton.Instance.Cont == 3)
                    {
                        Singleton.Instance.verif = true;
                    }
                }                
            }

            for (int i = 0; i < Singleton.Instance.WaitingList.Length; i++)
            {
                if (Singleton.Instance.WaitingList.Get(i).schedule == "Not scheduled yet" && Singleton.Instance.verif == false)
                {
                    if (Singleton.Instance.Cont > 4)
                    {
                        date = day + "/" + month + "/" + year;
                        Singleton.Instance.WaitingList.Get(i).schedule = date;
                        Singleton.Instance.Cont++;
                    }
                    else
                    {
                        day = r.Next(1, 28);
                        month = r.Next(1, 12);
                        date = day + "/" + month + "/" + year;
                        Singleton.Instance.WaitingList.Get(i).schedule = date;
                        Singleton.Instance.Cont++;
                    }
                    check = true;
                }
            }
            if (check)
            {
                TempData["testmsg"] = "Calendarization generated.";
            }
            else
            {
                TempData["testmsg"] = "All in the waiting list had been scheduled already or there is nobody in it.";
            }
            return RedirectToAction(nameof(Index));
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
                    Department = Singleton.Instance.department,
                    municipality = Singleton.Instance.muni,
                    priority = Singleton.Instance.priorityAssign(pr),
                    age = Convert.ToInt32(collection["age"]),
                    schedule = "Not scheduled yet"
                };
                Singleton.Instance.HeapPacient.insertKey(newPacient, newPacient.priority);
                Singleton.Instance.Data.Add(newPacient, Singleton.Instance.keyGen(newPacient.DPI));
                Singleton.Instance.NameTree.Root = Singleton.Instance.NameTree.Insert(newPacient.Name,Singleton.Instance.NameTree.Root);
                Singleton.Instance.WaitingList.InsertAtEnd(Singleton.Instance.HeapPacient.heapArray.Get(i).value);
                //vCunar->eliminar de lista espera y heap
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
            PacientModel pacient = Singleton.Instance.WaitingList.Get(Singleton.Instance.WaitingList.GetPositionOf(new PacientModel {DPI = dpi}));
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
