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
        public bool verif = false;
        public int Cont;
        public int maxLength;
        public int maxPacient;
        public int schedule;
        public int heapCapacity;
        public int hashCapacity;
        private readonly static Singleton _instance = new Singleton();
        public DoubleLinkedList<string> priorities;
        public DoubleLinkedList<PacientModel> WaitingList;
        public DoubleLinkedList<PacientModel> VaccinatedList;
        public HashTable<PacientModel, int> Data;
        public AVLTree<PacientModel> index;
        public Heap<PacientModel> HeapPacient;
        private Singleton()
        {
            index = new AVLTree<PacientModel>();
            WaitingList = new DoubleLinkedList<PacientModel>();
            VaccinatedList = new DoubleLinkedList<PacientModel>();
            priorities = new DoubleLinkedList<string>();
            priorities.InsertAtEnd("Health staff");
            priorities.InsertAtEnd("Older than 70 years");
            priorities.InsertAtEnd("Older than 50 years");
            priorities.InsertAtEnd("Essential workers");
            priorities.InsertAtEnd("People between 18 and 50 years old");
            maxLength = 15;
            maxPacient = 3;
            schedule = 30;
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
