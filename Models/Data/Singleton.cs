using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P1_EDD_DAVH_AFPE.Models.Data
{
    public sealed class Singleton
    {
        private readonly static Singleton _instance = new Singleton();
        public Tree<PacientModel> index;

        private Singleton()
        {
            index = new Tree<PacientModel>();
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
