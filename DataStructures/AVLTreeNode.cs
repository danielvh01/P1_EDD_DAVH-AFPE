using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures
{
    public class AVLTreeNode<T> where T : IComparable
    {
        #region Varibales and Instances
        //Punteros
        public AVLTreeNode<T> parent { get; set; }
        public AVLTreeNode<T> left { get; set; }
        public AVLTreeNode<T> right { get; set; }
        //Propiedades
        public T value { get; set; }

        public int height;
        //Constructor
        public AVLTreeNode(T newvalue)
        {
            value = newvalue;
            left = null;
            right = null;
            parent = null;
            height = 1;
        }
        #endregion
    }
}
