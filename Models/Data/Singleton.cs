using System;
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
        //Integer variables
        public int Cont;
        public int hashCapacity;
        public int heapCapacity;
        public int schedule;
        //String Variables
        public string department;
        public string muni;
        //DATA STORAGE//
        public string database;
        //Data structures
        public DoubleLinkedList<string> priorities;
        public DoubleLinkedList<string> municipalities;
        public DoubleLinkedList<PacientModel> WaitingList;
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
        private string lastAppointment;
        #endregion
        private Singleton()
        {
            hashCapacity = 15;
            heapCapacity = 3;
            schedule = 30;
            department = "";
            muni = "";
            DpiTree = new AVLTree<SearchCriteria<int>>();
            NameTree = new AVLTree<SearchCriteria<string>>();
            LastNameTree = new AVLTree<SearchCriteria<string>>();
            WaitingList = new DoubleLinkedList<PacientModel>();
            VaccinatedList = new DoubleLinkedList<PacientModel>();
            priorities = new DoubleLinkedList<string>();
            #region Priotity insertions
            priorities.InsertAtEnd("Health staff");
            priorities.InsertAtEnd("Health Sciences Students");
            priorities.InsertAtEnd("Relief corps");
            priorities.InsertAtEnd("Interned or admitted in an elderly institution person");
            priorities.InsertAtEnd("Older than 70 years");
            priorities.InsertAtEnd("National Security Staff");
            priorities.InsertAtEnd("Education Staff");
            priorities.InsertAtEnd("Justice Staff");
            priorities.InsertAtEnd("Older between 50 and 69 years");
            priorities.InsertAtEnd("Essential workers");
            priorities.InsertAtEnd("People between 18 and 50 years old");
            #endregion
            municipalities = new DoubleLinkedList<string>();


        }
        public static Singleton Instance
        {
            get
            {
                return _instance;
            }
        }


        public int priorityAssign(string pa)
        {
            if (pa == "Health staff" || pa == "Health Sciences Students" ||
                pa == "Relief corps" || pa == "Interned or admitted in an elderly institution person")
            {
                return 1;
            }
            if (pa == "Older than 70 years")
            {
                return 2;
            }
            if (pa == "Older than 50 years" || pa == "National Security Staff"
                || pa == "Education Staff" || pa == "Justice Staff")
            {
                return 3;
            }
            if (pa == "Essential workers")
            {
                return 4;
            }
            if (pa == "People between 18 and 50 years old")
            {
                return 5;
            }
            else
            {
                return default;
            }
        }
        public void Agregar(PacientModel newPacient)
        {
            int newkey = keyGen(newPacient.DPI);
            Data.Add(newPacient, newkey);
            Database.Add(newPacient, newkey);
            DpiTree.Root = DpiTree.Insert(new SearchCriteria<int> { value = newPacient.DPI, key = newkey }, DpiTree.Root);
            NameTree.Root = NameTree.Insert(new SearchCriteria<string> { value = newPacient.Name, key = newkey }, NameTree.Root);
            LastNameTree.Root = LastNameTree.Insert(new SearchCriteria<string> { value = newPacient.LastName, key = newkey }, LastNameTree.Root);
        }
        public bool Login(string municipality)
        {

            return true;
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
            string tasks = recorrido();
            if (tasks.Length > 0)
            {
                tasks = tasks.Remove(tasks.Length - 1);
            }
            database += "pacients:" + tasks;
        }

        public string recorrido()
        {
            //Write all the content of the HashTable.
            string result = "";
            for (int i = 0; i < HeapPacient.Length(); i++)
            {
                int PacientParam = HeapPacient.heapArray.Get(i).value.DPI;
                var Pacient = Data.Get(x => x.Name.CompareTo(PacientParam), keyGen(PacientParam));
                result += Pacient.Name + ",";
                result += Pacient.LastName + ",";
                result += Pacient.DPI + ",";
                result += Pacient.Department + ",";
                result += Pacient.municipality + ",";
                result += Pacient.priority + ",";
                result += Pacient.age + ",";
                result += Pacient.schedule + ";";
            }
            return result;
        }
    }
}
