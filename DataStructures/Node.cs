using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataStructures
{
    public class Node<T>
    {
        public Node<T> next = null;
        public Node<T> prev = null;
        public T value;
    }
}
