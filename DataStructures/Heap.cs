using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace DataStructures
{
    public class Heap<T> : IEnumerable<HeapNode<T>> where T : IComparable
    {
        #region Variables

        public DoubleLinkedList<HeapNode<T>> heapArray;
        int capacity;


        #endregion

        #region Methods
        public Heap(int L)
        {
            capacity = L;
            heapArray = new DoubleLinkedList<HeapNode<T>>();
        }

        public int Length()
        {
            return heapArray.Length;
        }

        public void Swap(int a, int b)
        {
            if (a > b)
            {
                HeapNode<T> temp = heapArray.Get(a);
                heapArray.Delete(a);
                heapArray.Insert(heapArray.Get(b), a);
                heapArray.Delete(b);
                heapArray.Insert(temp, b);
            }
            else
            {
                HeapNode<T> temp = heapArray.Get(b);
                heapArray.Delete(b);
                heapArray.Insert(heapArray.Get(a), b);
                heapArray.Delete(a);
                heapArray.Insert(temp, a);
            }
        }

        public int Parent(int index)
        {
            return (index - 1) / 2;
        }

        public int Left(int index)
        {
            return 2 * index + 1;
        }

        public int Right(int index)
        {
            return 2 * index + 2;
        }

        public bool insertKey(T value, string p)
        {
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

        public HeapNode<T> getMin()
        {
            return heapArray.Get(0);
        }

        public HeapNode<T> extractMin()
        {
            if (Length() <= 0)
            {
                return default;
            }
            else
            {
                HeapNode<T> result = heapArray.Get(0);
                heapArray.Delete(0);
                if (Length() > 0)
                {
                    MoveDown(0);
                }
                return result;
            }
        }
        public void Delete(T value)
        {
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

        public void Sort()
        {
            if (Length() > 0)
            {
                DoubleLinkedList<HeapNode<T>> sorted = new DoubleLinkedList<HeapNode<T>>();
                for (int i = 0; heapArray.Length > 0; i++)
                {
                    HeapNode<T> x = extractMin();
                    sorted.Insert(x, i);
                }
                for (int i = 0; sorted.Length > 0; i++)
                {
                    HeapNode<T> temp = sorted.Get(0);
                    sorted.Delete(0);
                    insertKey(temp.value, temp.priority);
                }
            }
        }

        public IEnumerable<HeapNode<T>> Search(Func<T, int> Comparer)
        {
            DoubleLinkedList<HeapNode<T>> result = new DoubleLinkedList<HeapNode<T>>();
            int cont = 0;
            for (int i = 0; i < heapArray.Length; i++)
            {
                if (Comparer.Invoke(heapArray.Get(i).value) == 0)
                {
                    result.Insert(heapArray.Get(i), cont++);
                }
            }
            return result;
        }

        public IEnumerator<HeapNode<T>> GetEnumerator()
        {

            Sort();
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