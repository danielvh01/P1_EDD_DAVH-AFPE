using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace P1_EDD_DAVH_AFPE.Models.Data
{
    public sealed class Singleton
    {
        public int maxLength;
        public int maxPacient;
        public int schedule;
        private readonly static Singleton _instance = new Singleton();
        public DoubleLinkedList<string> priorities;
        public Hashtable Data;
        private Singleton()
        {
            priorities = new DoubleLinkedList<string>();
            priorities.InsertAtEnd("Health personnel");
            priorities.InsertAtEnd("Older than 70 years");
            priorities.InsertAtEnd("Older than 50 years");
            priorities.InsertAtEnd("Essential workers");
            priorities.InsertAtEnd("People between 18 and 50 years old");
            Data = new Hashtable();
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
    }
}
