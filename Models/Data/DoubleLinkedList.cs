using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace P1_EDD_DAVH_AFPE.Models.Data
{
    public class DoubleLinkedList<T> : IEnumerable<T> where T : IComparable
    {
        #region Variables
        Node<T> First;
        Node<T> End;
        public int Length = 0;
        #endregion

        #region Methods
        public void InsertAtStart(T value)
        {
            Node<T> node = new Node<T>();
            node.value = value;
            if (Length == 0)
            {
                First = node;
                End = node;
            }
            else
            {
                node.next = First;
                First.prev = node;
                First = node;
            }
            Length++;
        }

        public void InsertAtEnd(T value)
        {
            Node<T> node = new Node<T>();
            node.value = value;
            if (Length == 0)
            {
                First = node;
                End = node;
            }
            else
            {
                End.next = node;
                node.prev = End;
                End = node;
            }
            Length++;
        }

        public void Insert(T value, int Position)
        {
            Node<T> newNode = new Node<T>();
            newNode.value = value;
            if (Length == 0)
            {
                First = newNode;
                End = newNode;
            }
            else
            {
                if (Position == 0)
                {
                    InsertAtStart(value);
                }
                else if (Position >= Length - 1)
                {
                    InsertAtEnd(value);
                }
                else
                {
                    Node<T> pretemp = First;
                    int cont = 0;
                    while (cont < Position - 1)
                    {
                        pretemp = pretemp.next;
                        cont++;
                    }
                    newNode.next = pretemp.next;
                    pretemp.next.prev = newNode;
                    pretemp.next = newNode;
                    newNode.prev = pretemp;
                    Length++;
                }
            }
        }

        void DeteleAtStart()
        {
            if (Length > 0)
            {
                if (Length == 1)
                {
                    First = null;
                    End = null;
                }
                else
                {
                    First = First.next;
                    First.prev = null;
                }
                Length--;
            }
        }

        void DeleteAtEnd()
        {
            if (Length > 0)
            {
                if (Length == 1)
                {
                    First = null;
                    End = null;
                }
                else
                {
                    End = End.prev;
                    End.next = null;
                }
                Length--;
            }
        }

        public void Delete(int position)
        {
            if (Length > 0)
            {
                if (Length == 1)
                {
                    First = null;
                    End = null;
                    Length--;
                }
                else
                {
                    if (position == 0)
                    {
                        DeteleAtStart();
                    }
                    else if (position >= Length - 1)
                    {
                        DeleteAtEnd();
                    }
                    else
                    {
                        Node<T> prev = First;
                        Node<T> node = First.next;
                        int cont = 0;
                        while (cont < position - 1)
                        {
                            prev = node;
                            node = node.next;
                            cont++;
                        }
                        prev.next = node.next;
                        node.next.prev = prev;
                        node = null;
                        Length--;
                    }
                }
            }
        }

        T GetFirst()
        {
            if (Length > 0)
            {
                return First.value;
            }
            else
            {
                return default;
            }
        }

        T GetEnd()
        {
            if (Length > 0)
            {
                Node<T> node = First;
                while (node.next != null)
                {
                    node = node.next;
                }
                return node.value;
            }
            else
            {
                return default;
            }
        }

        public T Get(int position)
        {
            if (Length > 0)
            {
                if (position == 0)
                {
                    return GetFirst();
                }
                else if (position >= Length)
                {
                    return GetEnd();
                }
                else
                {
                    Node<T> node = First;
                    int cont = 0;
                    while (node != null && cont < position - 1)
                    {
                        node = node.next;
                        cont++;
                    }
                    if (node != null)
                    {
                        return node.value;
                    }
                    else
                    {
                        return default;
                    }
                }
            }
            else
            {
                return default;
            }
        }

        public int GetPositionOf(T value)
        {
            Node<T> temp = First;
            int cont = 0;
            while (temp != null && temp.value.CompareTo(value) < 0)
            {
                temp = temp.next;
                cont++;
            }
            if (temp != null)
            {
                if (temp.value.CompareTo(value) == 0)
                {
                    return cont;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        }

        public T Find(T value)
        {
            Node<T> temp = First;
            while (temp != null && temp.value.CompareTo(value) < 0)
            {
                temp = temp.next;
            }
            if (temp != null)
            {
                if (temp.value.CompareTo(value) == 0)
                {
                    return temp.value;
                }
                else
                {
                    return default;
                }
            }
            else
            {
                return default;
            }
        }
        public IEnumerator<T> GetEnumerator()
        {
            var node = First;
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
