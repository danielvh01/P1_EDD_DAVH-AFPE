﻿using Microsoft.AspNetCore.Mvc;
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
using System.IO;

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
        public IActionResult Login()
        {
            return RedirectToAction(nameof(Login), "Pacient");
        }
        //Assign the actual login to identify the data 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(IFormCollection collection)
        {
            Singleton.Instance.muni = collection["municipality"];
            return RedirectToAction(nameof(Login), "Pacient");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Configuration(IFormCollection collection)
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
                        Singleton.Instance.maxPacient = Convert.ToInt32(obj[i].Substring(spacer + 1));
                        Singleton.Instance.HeapPacient = new Heap<PacientModel>(Singleton.Instance.maxPacient);
                    }
                    if (obj[i].Substring(0, spacer) == "hashCapacity")
                    {
                        Singleton.Instance.maxLength = Convert.ToInt32(obj[i].Substring(spacer + 1));
                        Singleton.Instance.Data = new HashTable<PacientModel, int>(Singleton.Instance.maxLength);
                    }
                    if (obj[i].Substring(0, spacer) == "tasks")
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
                                    priority = Convert.ToInt32(obj2[5]),
                                    age = Convert.ToInt32(obj2[6]),
                                    schedule = obj2[7]
                                };
                                Singleton.Instance.HeapPacient.insertKey(newPacient,Singleton.Instance.keyGen(newPacient.DPI));
                                Singleton.Instance.Data.Add(newPacient, Singleton.Instance.keyGen(newPacient.DPI));
                                for (int a = 0; a < Singleton.Instance.HeapPacient.Length(); a++)
                                {
                                    if (a == 0)
                                    {
                                        Singleton.Instance.PacientsTree = new AVLTree<PacientModel>();
                                        Singleton.Instance.WaitingList = new DoubleLinkedList<PacientModel>();
                                    }
                                    Singleton.Instance.PacientsTree.Insert(Singleton.Instance.HeapPacient.heapArray.Get(a).value, Singleton.Instance.PacientsTree.Root);
                                    Singleton.Instance.WaitingList.InsertAtEnd(Singleton.Instance.HeapPacient.heapArray.Get(a).value);
                                }
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
            Singleton.Instance.HeapPacient = new Heap<PacientModel>(Singleton.Instance.maxPacient);
            Singleton.Instance.Data = new HashTable<PacientModel, int>(Singleton.Instance.maxLength);
            return RedirectToAction(nameof(Login));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
