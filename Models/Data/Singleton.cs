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
        public Tree<PacientModel> index;
        public Hashtable Data;
        private Singleton()
        {
            index = new Tree<PacientModel>();
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
