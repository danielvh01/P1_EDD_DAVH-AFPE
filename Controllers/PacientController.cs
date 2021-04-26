using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using P1_EDD_DAVH_AFPE.Models;
using P1_EDD_DAVH_AFPE.Models.Data;

namespace P1_EDD_DAVH_AFPE.Controllers
{
    public class PacientController : Controller
    {
        // GET: PacientController
        public ActionResult Index()
        {
            return View();
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
                    Department = collection["Department"],
                    municipality = collection["municipality"],
                    priority = (collection["priority"]),                    
                };
                int P = priorityAssign(pr);                
                var newPacientAVL = new PacientModel
                {
                    Name = collection["Name"],
                    LastName = collection["LastName"],
                    DPI = Convert.ToInt32(collection["DPI"]),
                };
                Singleton.Instance.PriorityPacient.insertKey(newPacientAVL, P);
                Singleton.Instance.Data.Add(newPacient, Singleton.Instance.keyGen(newPacient.Name));
                for (int i = 0; i < Singleton.Instance.PriorityPacient.Length(); i++)
                {
                    Singleton.Instance.index.Insert(Singleton.Instance.PriorityPacient.heapArray.Get(i).value,Singleton.Instance.index.Root);
                }
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
        public ActionResult Delete(int id)
        {
            return View();
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

        public int priorityAssign(string pa)
        {
            if (pa == "Health staff")
            {
                return 1;
            }
            if (pa  == "Older than 70 years")
            {
                return 2;
            }
            if (pa  == "Older than 50 years")
            {
                return 3;
            }
            if (pa  == "Essential workers")
            {
                return 4;
            }
            if (pa  == "People between 18 and 50 years old")
            {
                return 5;
            }
            else
            {
                return default;
            }
        }
    }
}
