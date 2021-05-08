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
        //Heap
        public DoubleLinkedList<HeapNode<T>> heapArray;
        //Capacidad máxima del heap
        int capacity;


        #endregion

        #region Methods
        //Constructor
        public Heap(int L)
        {
            capacity = L;
            heapArray = new DoubleLinkedList<HeapNode<T>>();
        }
        //Metodo que obtiene el tamaño del heap
        public int Length()
        {
            return heapArray.Length;
        }
        //Intercambia dos elementos en el heap
        public void Swap(int a, int b)
        {
            //Intercambio cuando a es mayor
            if (a > b)
            {
                HeapNode<T> temp = heapArray.Get(a);
                heapArray.Delete(a);
                heapArray.Insert(heapArray.Get(b), a);
                heapArray.Delete(b);
                heapArray.Insert(temp, b);
            }
            //Intercambio cuando b es mayor
            else
            {
                HeapNode<T> temp = heapArray.Get(b);
                heapArray.Delete(b);
                heapArray.Insert(heapArray.Get(a), b);
                heapArray.Delete(a);
                heapArray.Insert(temp, a);
            }
        }
        //Obtiene el Nodo padre
        public int Parent(int index)
        {
            return (index - 1) / 2;
        }
        //Obtiene el hijo izquierdo
        public int Left(int index)
        {
            return 2 * index + 1;
        }
        //Obtiene el hijo derecho
        public int Right(int index)
        {
            return 2 * index + 2;
        }

        //Inserta un nuevo elemento al heap
        public bool insertKey(T value, string p)
        {
            //Si no ha llegado a su máxima capacidad, inserta el elemento
            if (Length() == capacity)
            {
                return false;
            }
            int i = Length();
            heapArray.Insert(new HeapNode<T>(value, p), i);

            while (i > 0 && heapArray.Get(i).CompareTo(heapArray.Get(Parent(i))) > 0)
            {
                Swap(i, Parent(i));
                i = Parent(i);
            }
            return true;
        }
        //Obtiene el elemento más pequeño del heap
        public T getMin()
        {
            return heapArray.Get(0).value;
        }
        //Extrae el elemento más pequeño del heap
        public HeapNode<T> extractMin()
        {
            //Si el heap no esta vació realiza la eliminación
            if (Length() <= 0)
            {
                return default;
            }
            else
            {
                //Elimina el primer elemento
                HeapNode<T> result = heapArray.Get(0);
                heapArray.Delete(0);
                if (Length() > 0)
                {
                    MoveDown(0);
                }
                return result;
            }
        }
        //Borra un elemento en específico
        public void Delete(T value)
        {
            //Elimina si la heap no esta vacía
            if (Length() > 0)
            {
                DoubleLinkedList<HeapNode<T>> result = new DoubleLinkedList<HeapNode<T>>();
                for (int i = 0; heapArray.Length > 0; i++)
                {
                    HeapNode<T> x = extractMin();
                    result.Insert(x, i);
                }
                for (int i = 0; result.Length > 0; i++)
                {
                    HeapNode<T> temp = result.Get(0);
                    result.Delete(0);
                    if (temp.value.CompareTo(value) != 0)
                    {
                        insertKey(temp.value, temp.priority);
                    }
                }
            }
        }

        public void MoveDown(int position)
        {
            int lchild = Left(position);
            int rchild = Right(position);
            int largest;
            if ((lchild < Length()) && (heapArray.Get(position).CompareTo(heapArray.Get(lchild)) < 0))
            {
                largest = lchild;
            }
            else
            {
                largest = position;
            }
            if ((rchild < Length()) && (heapArray.Get(largest).CompareTo(heapArray.Get(rchild)) < 0))
            {
                largest = rchild;
            }
            if (largest != position)
            {
                Swap(position, largest);
                MoveDown(largest);
            }
        }

        //Devuelve todos los elementos del heap
        public IEnumerator<T> GetEnumerator()
        {
            var node = heapArray.First;
            while (node != null)
            {
                yield return node.value.value;
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