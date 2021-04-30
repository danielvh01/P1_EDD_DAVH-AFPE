using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
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
        //Data structures
        public DoubleLinkedList<string> priorities;
        public DoubleLinkedList<string> municipalities;
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
            priorities.InsertAtEnd("Older than 70 years");
            priorities.InsertAtEnd("Older than 50 years");
            priorities.InsertAtEnd("Essential workers");
            priorities.InsertAtEnd("People between 18 and 50 years old");
            #endregion
            municipalities = new DoubleLinkedList<string>();
            #region Municipalities insertions
            
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
