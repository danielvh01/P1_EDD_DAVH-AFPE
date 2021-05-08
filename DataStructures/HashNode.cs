using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataStructures
{
    public class HashNode<T, K> where T : IComparable where K : IComparable
    {
        #region Variables
        //Propiedades
        public DoubleLinkedList<T> value;
        public K key { get; set; }
        //Punteros
        public HashNode<T, K> next;
        public HashNode<T, K> prev;
        #endregion

        #region Methods
        //Constructor
        public HashNode()
        {
            value = new DoubleLinkedList<T>();
            key = default;
        }
        #endregion

        
    }
}
