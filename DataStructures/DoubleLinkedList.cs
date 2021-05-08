using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace DataStructures
{
    public class DoubleLinkedList<T> : IEnumerable<T> where T : IComparable
    {
        #region Variables
        //Pointers
        public Node<T> First;
        Node<T> End;
        //Properties
        public int Length = 0;
        #endregion

        #region Methods
        public void InsertAtStart(T value)
        {
            //Se declara un nodo vacio y asigna el valor del tipo de dato generico entrante
            Node<T> node = new Node<T>();
            node.value = value;
            //Si la cantidad de elementos en la lista es cero, el nodo a insertar se vuelve tanto el inicio como el final de la misma
            if (Length == 0)
            {
                First = node;
                End = node;
            }
            //De no ser cero la longitud, el primero de la lista toma a ser el segundo de la misma y el nodo a ingresar toma la primera posicion
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
            //Se declara un nodo vacio y asigna el valor del tipo de dato generico entrante
            Node<T> node = new Node<T>();
            node.value = value;
            //Si la cantidad de elementos en la lista es cero, el nodo a insertar se vuelve tanto el inicio como el final de la misma
            if (Length == 0)
            {
                First = node;
                End = node;
            }
            //De no ser cero la longitud, el ultimo de la lista toma a ser el penultimo de la misma y el nodo a ingresar toma la ultima posicion
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
            //Se declara un nodo vacio y asigna el valor del tipo de dato generico entrante
            Node<T> newNode = new Node<T>();
            newNode.value = value;
            //Si la cantidad de elementos en la lista es cero, el nodo a insertar se vuelve tanto el inicio como el final de la misma
            if (Length == 0)
            {
                First = newNode;
                End = newNode;
                Length++;
            }            
            else
            {
                //De lo contrario, si la posicion a insertar deseada es cero, el valor entrante sera insertado al inicio
                if (Position == 0)
                {
                    InsertAtStart(value);
                }
                //Si la posicion a insertar deseada es la ultima de la lista, procede a invocar el metodo de insertar al final de la misma.
                else if (Position >= Length - 1)
                {
                    InsertAtEnd(value);
                }
                //Si la posicion a insertar deseada no es la del inicio ni tampoco la del final, se realiza un bucle que mientras el contador sea menor a la posicion
                //deseada - 1 , el nodo temporal toma el valor del siguiente
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
            //Verifica que la cantidad de nodos en la lista sea mayor a cero
            if (Length > 0)
            {
                //Si la cantidad es igual a 1, tanto el principio como el final de la lista se asignan con valor nulo.
                if (Length == 1)
                {
                    First = null;
                    End = null;
                }
                //Asigna el nodo siguiente del primero como primero y elimina el que era primero originalmente
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
            //Verifica que la cantidad de nodos en la lista sea mayor a cero
            if (Length > 0)
            {
                //Si la cantidad es igual a 1, tanto el principio como el final de la lista se asignan con valor nulo.
                if (Length == 1)
                {
                    First = null;
                    End = null;
                }
                //Asigna el nodo anterior del ultimo como ultimo y elimina el que era ultimo en la lista originalmente
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
            //Verifica que la cantidad de nodos en la lista sea mayor a cero
            if (Length > 0)
            {
                //Si la cantidad es igual a 1, tanto el principio como el final de la lista se asignan con valor nulo.
                if (Length == 1)
                {
                    First = null;
                    End = null;
                    Length--;
                }
                else
                {
                    //si la posicion a eliminar deseada es cero, se invoca al metodo de eliminar al inicio para eliminar el nodo inicial
                    if (position == 0)
                    {
                        DeteleAtStart();
                    }
                    //si la posicion a eliminar deseada es la ultima, se invoca al metodo de eliminar al ultimo de la lista para eliminar el ultimo nodo
                    else if (position >= Length - 1)
                    {
                        DeleteAtEnd();
                    }
                    //Si la posicion a eliminar deseada no es la del inicio ni tampoco la del final, se realiza un bucle que mientras el contador sea menor a la posicion
                    //deseada - 1 , el nodo temporal toma el valor del siguiente
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
                        if (node.next != null)
                        {
                            node.next.prev = prev;
                        }
                        Length--;
                    }
                }
            }
        }

        //Obtiene el primer dato generico que encuentre en la lista
        T GetFirst()
        {
            //verifica que la cantidad sea mayor a cero para devolver el primer dato generico que encuentre en la lista
            if (Length > 0)
            {
                return First.value;
            }
            //De lo contrario retornara un null
            else
            {
                return default;
            }
        }

        //Obtiene el ultimo dato generico que encuentre en la lista
        T GetEnd()
        {
            //verifica que la cantidad sea mayor a cero para devolver el ultimo dato generico que encuentre en la lista
            if (Length > 0)
            {
                return End.value;
            }
            else
            {
                return default;
            }
        }


        //retorna el dato generico en base a la posicion deseada
        public T Get(int position)
        {
            //si la cantidad es mayor a cero procede a evaluar las siguientes condiciones
            if (Length > 0)
            {
                //si la posicion desea es la inicial de la lista, invoca el metodo GetFirst() para retornar el primero dato generico que encuentre en la lista
                if (position == 0)
                {
                    return GetFirst();
                }
                //si la posicion desea es la ultima de la lista, invoca el metodo GetEnd() para retornar el ultima dato generico que encuentre en la lista
                else if (position >= Length - 1)
                {
                    return GetEnd();
                }
                //si la posicion deseada no es la inicial ni la ultima, procede a encontrar el valor hasta llegar a la posicion actual
                else
                {
                    Node<T> node = First;
                    int cont = 0;
                    while (node != null && cont < position)
                    {
                        node = node.next;
                        cont++;
                    }
                    // si al llegar a la posicion deseada el nodo es distinto a nulo, retorna el valor deseado
                    if (node != null)
                    {
                        return node.value;
                    }
                    //De lo contrario volvera un valor nulo
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

        //retorna la posicion del dato enviado como parametro, realizando comparaciones del mismo con el del nodo en cierta posicion
        //Si el valor del nodo es distinto a null e igual al valor del deseado, retorna la posicion en base al contador que el while fue aumentando
        //De ser lo contrario, retornara un -1 indicando que no fue posible encontrar el valor deseado en una posicion de la lista
        public int GetPositionOf(T value)
        {
            Node<T> temp = First;
            int cont = 0;
            while (temp != null && temp.value.CompareTo(value) != 0)
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

        //Se encarga de retornar el valor del nodo realizando comparaciones hasta que el nodo temporal sea nulo. Si el siguiente del temporal es distinto a nulo y
        //el valor es el mismo que el deseado, retorna el mismo
        //De lo contrario retornara un valor nulo
        public T Find(T value)
        {
            Node<T> temp = First;
            while (temp != null && temp.value.CompareTo(value) != 0)
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

        //T debe ser IComparable        
        //Mandara el nodo en el que esta y el deseado
        //Saldra de bucle hasta encontrar un nodo que posea el mismo valor T.
        //Si el nodo es distinto a un valor nulo, devolvera el valor del nodo
        public T Find(Func<T, int> comparer)
        {
            Node<T> temp = First;
            while (temp != null && comparer.Invoke(temp.value) != 0)
            {
                temp = temp.next;
            }
            if (temp != null)
            {
                return temp.value;
            }
            else
            {
                return default;
            }
        }
        
        //Metodo encargado de devolver un nodo y su siguiente hasta que un nodo posea valor de nulo.
        //Metodo usado para llenar los index en base a los nodos contenidos en la lista.
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
