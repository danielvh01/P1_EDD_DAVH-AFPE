using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P1_EDD_DAVH_AFPE.Models.Data
{
    public class HashNode<T, K> where T : IComparable where K : IComparable
    {
        #region Variables
        public DoubleLinkedList<T> value;
        public K key { get; set; }

        public HashNode<T, K> next;
        public HashNode<T, K> prev;
        #endregion

        #region Methods
        public HashNode()
        {
            value = new DoubleLinkedList<T>();
            key = default;
        }
        #endregion


    }
}
