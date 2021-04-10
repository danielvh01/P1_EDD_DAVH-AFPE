using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P1_EDD_DAVH_AFPE.Models.Data
{
    public class Node<T>
    {
        public Node<T> next = null;
        public Node<T> prev = null;
        public T value;
    }
}
