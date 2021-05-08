using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataStructures
{
    public class Node<T>
    {
        //Apuntadores a nodos
        public Node<T> next = null;
        public Node<T> prev = null;
        //Valor que contiene cada nodo declarado
        public T value;
    }
}
