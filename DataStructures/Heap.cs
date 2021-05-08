using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace DataStructures
{
    public class Heap<T> : IEnumerable<T> where T : IComparable
    {
        #region Variables
        //Heap en forma de arreglo (forma alternativa del array)
        public DoubleLinkedList<T> heapArray;
        //Heap en forma de arbol binario (forma principal del array)
        public AVLTree<T> binaryHeap;
        //Capacidad máxima del heap
        int capacity;


        #endregion

        #region Methods
        //Constructor
        public Heap(int L)
        {
            capacity = L;
            heapArray = new DoubleLinkedList<T>();

            binaryHeap = new AVLTree<T>();
        }
        //Metodo que obtiene el tamaño del heap
        public int Length()
        {
            return binaryHeap.Length;
        }

        //Inserta un nuevo elemento al heap
        public bool insertKey(T value)
        {
            //Si no ha llegado a su máxima capacidad, inserta el elemento
            if (Length() == capacity)
            {
                return false;
            }
            binaryHeap.Root = binaryHeap.Insert(value, binaryHeap.Root);
            return true;
        }
        //Obtiene el elemento más pequeño del heap
        public T getMin()
        {
            FillHeapArray(binaryHeap.Root);
            return heapArray.Get(0);
        }
        //Extrae el elemento más pequeño del heap
        public T extractMin()
        {
            //Si el heap no esta vació realiza la eliminación
            if (Length() <= 0)
            {
                return default;
            }
            else
            {
                FillHeapArray(binaryHeap.Root);
                T result = heapArray.Get(0);
                binaryHeap.Delete(binaryHeap.Root, result);
                return result;
            }
        }

        //Genera el heap en forma de arreglo en base a la forma de árbol binario
        void FillHeapArray(AVLTreeNode<T> node)
        {
            if (node == null)
            {
                return;
            }
            if (node.left != null)
            {
                FillHeapArray(node.left);
            }
            heapArray.InsertAtEnd(node.value);
            if (node.right != null)
            {
                FillHeapArray(node.right);
            }
        }

        //Devuelve todos los elementos del heap
        public IEnumerator<T> GetEnumerator()
        {
            FillHeapArray(binaryHeap.Root);
            var node = heapArray.First;
            while (node != null)
            {
                yield return node.value;
                node = node.next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}