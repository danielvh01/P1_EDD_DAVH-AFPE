using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataStructures
{
    public class HeapNode<T> : IComparable where T : IComparable
    {
        public string priority;
        public T value { get; set; }

        public int height;

        public HeapNode(T newvalue, string p)
        {
            value = newvalue;
            height = 1;
            priority = p;
        }

        public int CompareTo(Object obj)
        {
            var comparer = ((HeapNode<T>)obj).value;
            return comparer.CompareTo(value);
        }

    }
}
