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
        public DoubleLinkedList<T> heapArray;

        public AVLTree<T> binaryHeap;
        int capacity;


        #endregion

        #region Methods
        public Heap(int L)
        {
            capacity = L;
            heapArray = new DoubleLinkedList<T>();

            binaryHeap = new AVLTree<T>();
        }

        public int Length()
        {
            return heapArray.Length;
        }

        public void Swap(int a, int b)
        {
            if (a > b)
            {
                T temp = heapArray.Get(a);
                heapArray.Delete(a);
                heapArray.Insert(heapArray.Get(b), a);
                heapArray.Delete(b);
                heapArray.Insert(temp, b);
            }
            else
            {
                T temp = heapArray.Get(b);
                heapArray.Delete(b);
                heapArray.Insert(heapArray.Get(a), b);
                heapArray.Delete(a);
                heapArray.Insert(temp, a);
            }
        }


        public int Left(int index)
        {
            return 2 * index + 1;
        }

        public int Right(int index)
        {
            return 2 * index + 2;
        }

        public bool insertKey(T value)
        {
            if (Length() == capacity)
            {
                return false;
            }
            binaryHeap.Root = binaryHeap.Insert(value, binaryHeap.Root);
            return true;
        }

        public T getMin()
        {
            FillHeapArray(binaryHeap.Root);
            return heapArray.Get(0);
        }

        public T extractMin()
        {
            if (Length() <= 0)
            {
                return default;
            }
            else
            {
                FillHeapArray(binaryHeap.Root);
                T result = heapArray.Get(0);
                heapArray.Delete(0);
                if (Length() > 0)
                {
                    MoveDown(0);
                }
                return result;
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