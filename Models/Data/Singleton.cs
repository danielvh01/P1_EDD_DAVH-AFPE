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
        public int maxLength;
        public int maxPacient;
        public int schedule;
        public int heapCapacity;
        public int hashCapacity;
        //String Variables
        public string muni = "";
        //Data structures
        public DoubleLinkedList<string> priorities;
        public HashTable<string,string> municipalities;
        public DoubleLinkedList<PacientModel> WaitingList;
        public DoubleLinkedList<PacientModel> VaccinatedList;
        public HashTable<PacientModel, int> Data;
        public AVLTree<PacientModel> PacientsTree;
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
            maxLength = 15;
            maxPacient = 3;
            schedule = 30;                    
            PacientsTree = new AVLTree<PacientModel>();
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
            municipalities = new HashTable<string, string>(22);
            #region Municipalities insertions
            StreamReader sr = new StreamReader("Municipios.txt");
            string result = sr.ReadToEnd();
            string[] lines = result.Split("\n");
            string depa = "";
            for(int i = 0; i < lines.Length; i++)
            {
                if(lines[1].StartsWith("-"))
                {
                    depa = lines[1].Remove(0,1);
                }
                else
                {
                    municipalities.Add(lines[1], depa);
                }
            }
            #endregion


        }
        public static Singleton Instance
        {
            get
            {
                return _instance;
            }
        }

        public int keyGen(int dpi)
        {
            return dpi % maxLength;
        }
    }
}
