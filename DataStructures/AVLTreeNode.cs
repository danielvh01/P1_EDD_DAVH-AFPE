using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures
{
    public class AVLTreeNode<T> where T : IComparable
    {
        public AVLTreeNode<T> parent { get; set; }
        public AVLTreeNode<T> left { get; set; }
        public AVLTreeNode<T> right { get; set; }
        public T value { get; set; }

        public int height;

        public AVLTreeNode(T newvalue)
        {
            value = newvalue;
            left = null;
            right = null;
            parent = null;
            height = 1;
        }
    }
}
