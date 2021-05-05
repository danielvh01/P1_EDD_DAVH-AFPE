﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using DataStructures;


namespace P1_EDD_DAVH_AFPE.Models.Data
{
    public sealed class Singleton
    {
        #region Public Variables
        //Boolean variables
        public bool verif = false;
        public bool SPacient;
        //Integer variable
        public int simmultaneous;
        public int Cont;
        public int hashCapacity;
        public int heapCapacity;
        public int schedule;
        //String Variables
        public string department;
        public string muni;
        public string startingDate;
        public string lastAppointment;
        //DATA STORAGE//
        public string database;
        //Data structures
        public DoubleLinkedList<string> priorities;
        public HashTable<string,string> municipalities;
        public DoubleLinkedList<PacientModel> VaccinatedList;
        public HashTable<PacientModel, int> Data;
        public AVLTree<SearchCriteria<int>> DpiTree;
        public AVLTree<SearchCriteria<string>> NameTree;
        public AVLTree<SearchCriteria<string>> LastNameTree;
        public Heap<PacientModel> HeapPacient;
        #endregion
        #region Private Variables
        private HashTable<PacientModel, int> Database;
        private readonly static Singleton _instance = new Singleton();
        private Heap<PacientModel> HeapDatabase;
        #endregion
        private Singleton()
        {
            hashCapacity = 15;
            heapCapacity = 3;
            schedule = 30;
            department = "";
            muni = "";
            Database = new HashTable<PacientModel, int>(hashCapacity);
            DpiTree = new AVLTree<SearchCriteria<int>>();
            NameTree = new AVLTree<SearchCriteria<string>>();
            LastNameTree = new AVLTree<SearchCriteria<string>>();
            VaccinatedList = new DoubleLinkedList<PacientModel>();
            priorities = new DoubleLinkedList<string>();
            HeapPacient = new Heap<PacientModel>(heapCapacity);
            #region Priotity insertions
            priorities.InsertAtEnd("Trabajador de establecimientos de salud asistencial que atienden pacientes con COVID-19");//1
            priorities.InsertAtEnd("Trabajador de establecimiento de salud asistencial");
            priorities.InsertAtEnd("Estudiantes de ciencias de la salud y afines");
            priorities.InsertAtEnd("Cuerpo de socorro,trabajador de funeraria o personal que labora en instituciones de adultos mayores");
            priorities.InsertAtEnd("Persona internada en hogar o institucion de adulto mayor");
            priorities.InsertAtEnd("Trabajadores del sector salud (administrativos)");
            priorities.InsertAtEnd("Adulto mayor de 70 o más años");
            priorities.InsertAtEnd("Adulto de 50 a 69 años");
            priorities.InsertAtEnd("Trabajador del sector de seguridad nacional");
            priorities.InsertAtEnd("Trabajadores registrados en las municipalidades y entidades que prestan servicios esenciales");
            priorities.InsertAtEnd("Trabajador del sector educativo");
            priorities.InsertAtEnd("Trabajadores sector justicia, (jueces, personal en tribunales)");
            priorities.InsertAtEnd("Adultos de 40 a 49 años");
            priorities.InsertAtEnd("Adultos de 18 a 39 años");            
            #endregion
            municipalities = new HashTable<string, string>(22);
            StreamReader sr = new StreamReader("Municipios.txt");
            string result = sr.ReadToEnd();
            string[] lines = result.Split("\r\n");
            string d = "";
            for (int i = 0; i < lines.Length; i++)
            {
                if(lines[i].StartsWith("-"))
                {
                    d = lines[i].Replace("-", "");
                }
                else
                {
                    municipalities.Add(lines[i], d);
                }
            }

        }
        public static Singleton Instance
        {
            get
            {
                return _instance;
            }
        }


        public string priorityAssign(string pa)
        {
            if (pa == "Trabajador de establecimientos de salud asistencial que atienden pacientes con COVID-19")
            {
                return "1a";
            }
            if (pa == "Trabajador de establecimiento de salud asistencial")
            {
                return "1b";
            }
            if (pa == "Estudiantes de ciencias de la salud y afines")
            {
                return "1c";
            }
            if (pa == "Cuerpo de socorro,trabajador de funeraria o personal que labora en instituciones de adultos mayores")
            {
                return "1d";
            }
            if (pa == "Persona internada en hogar o institucion de adulto mayor")
            {
                return "1e";
            }
            if (pa == "Trabajadores del sector salud (administrativos)")
            {
                return "1f";
            }
            if (pa == "Adulto mayor de 70 o más años")
            {
                return "2a"; 
            }
            if (pa == "Adulto de 50 a 69 años")
            {
                return "2b";
            }
            if (pa == "Trabajador del sector de seguridad nacional")
            {
                return "3a";
            }
            if (pa == "Trabajadores registrados en las municipalidades y entidades que prestan servicios esenciales")
            {
                return "3b";
            }
            if (pa == "Trabajador del sector educativo")
            {
                return "3c";
            }
            if (pa == "Trabajadores sector justicia, (jueces, personal en tribunales)")
            {
                return "3d";
            }
            if (pa == "Adultos de 40 a 49 años")
            {
                return "4a";
            }
            if (pa == "Adultos de 18 a 39 años")
            {
                return "4b";
            }
            else
            {
                return "";
            }
        }
        public void Agregar(PacientModel newPacient)
        {
            int newkey = keyGen(newPacient.DPI);
            Database.Add(newPacient, newkey);
            Data.Add(newPacient, newkey);
            DpiTree.Root = DpiTree.Insert(new SearchCriteria<int> { value = newPacient.DPI, key = newPacient.DPI }, DpiTree.Root);
            NameTree.Root = NameTree.Insert(new SearchCriteria<string> { value = newPacient.Name, key = newPacient.DPI }, NameTree.Root);
            LastNameTree.Root = LastNameTree.Insert(new SearchCriteria<string> { value = newPacient.LastName, key = newPacient.DPI }, LastNameTree.Root);
            HeapPacient.insertKey(newPacient, newPacient.priority);
        }
        public void AddDataBase(PacientModel newPacient)
        {
            int newkey = keyGen(newPacient.DPI);
            Database.Add(newPacient, newkey);
        }
        public void Login(string municipality)
        {
            foreach(var item in Singleton.Instance.Database.GetAllElementsOf(x => x.municipality == municipality))
            {
                int newkey = keyGen(item.DPI);
                Data.Add(item, newkey);
                DpiTree.Root = DpiTree.Insert(new SearchCriteria<int> { value = item.DPI, key = newkey }, DpiTree.Root);
                NameTree.Root = NameTree.Insert(new SearchCriteria<string> { value = item.Name, key = newkey }, NameTree.Root);
                LastNameTree.Root = LastNameTree.Insert(new SearchCriteria<string> { value = item.LastName, key = newkey }, LastNameTree.Root);
            }
        }

        public int keyGen(int dpi)
        {
            return dpi % hashCapacity;
        }

        public void BuildData()
        {
            database = "";
            database += "heapCapacity:" + heapCapacity + "\n";
            database += "hashCapacity:" + hashCapacity + "\n";
            string ptnts = recorrido();
            if (ptnts.Length > 0)
            {
                ptnts = ptnts.Remove(ptnts.Length - 1);
            }
            database += "pacients:" + ptnts;
        }

        public string recorrido()
        {
            //Write all the content of the HashTable.
            string result = "";
            for (int i = 0; i < HeapPacient.Length(); i++)
            {
                int PacientParam = HeapPacient.heapArray.Get(i).value.DPI;
                var Pacient = Database.Get(x => x.Name.CompareTo(PacientParam), keyGen(PacientParam));
                result += Pacient.Name + ",";
                result += Pacient.LastName + ",";
                result += Pacient.DPI + ",";
                result += Pacient.Department + ",";
                result += Pacient.municipality + ",";
                result += Pacient.priority + ",";                
                result += Pacient.schedule + ";";
            }
            return result;
        }
    }
}
