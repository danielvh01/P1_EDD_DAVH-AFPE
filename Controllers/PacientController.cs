﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using P1_EDD_DAVH_AFPE.Models;
using P1_EDD_DAVH_AFPE.Models.Data;
using DataStructures;
namespace P1_EDD_DAVH_AFPE.Controllers
{
    public class PacientController : Controller
    {
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

        //
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
                if (Singleton.Instance.WaitingList.Get(i).schedule == "" && Singleton.Instance.verif == false)
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
                    Department = collection["Department"],
                    municipality = collection["municipality"],
                    priority = priorityAssign(pr, Convert.ToInt32(collection["age"])),
                    age = Convert.ToInt32(collection["age"]),
                    schedule = ""
                };               
                var newPacientAVL = new PacientModel
                {
                    Name = collection["Name"],
                    LastName = collection["LastName"],
                    DPI = Convert.ToInt32(collection["DPI"]),
                    priority = priorityAssign(pr, Convert.ToInt32(collection["age"]))
                };
                Singleton.Instance.HeapPacient.insertKey(newPacient, newPacient.priority);
                Singleton.Instance.Data.Add(newPacient, Singleton.Instance.keyGen(newPacient.DPI));
                for (int i = 0; i < Singleton.Instance.HeapPacient.Length(); i++)
                {
                    if (i == 0)
                    {
                        for (int j = 0; j < Singleton.Instance.index.Length; j++)
                        {
                            Singleton.Instance.index = new AVLTree<PacientModel>();
                            Singleton.Instance.WaitingList = new DoubleLinkedList<PacientModel>();
                        }
                    }
                    Singleton.Instance.index.Insert(Singleton.Instance.HeapPacient.heapArray.Get(i).value,Singleton.Instance.index.Root);
                    Singleton.Instance.WaitingList.InsertAtEnd(Singleton.Instance.HeapPacient.heapArray.Get(i).value);
                }
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

        public int priorityAssign(string pa, int age)
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
