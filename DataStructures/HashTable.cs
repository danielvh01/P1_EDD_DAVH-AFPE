using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataStructures
{
    public class HashTable<T, K> where T : IComparable where K : IComparable
    {
        #region Variables
        //Propiedades
        public int Length;
        int maxKeys;
        //Punteros
        HashNode<T, K> start;
        HashNode<T, K> end;
        #endregion

        #region Methods
        //Constructor
        public HashTable(int L)
        {
            start = null;
            end = null;
            Length = 0;
            maxKeys = L;
        }
        //Método que verifica si existe una cable
        public bool existsKey(K key)
        {
            //Revisa solo si la tabla hash no esta vacía, si lo esta devuelve false
            if(Length > 0)
            {
                HashNode<T, K> temp = start;
                //Recorre todas las llaves hasta encontrar la que se busca, si la encuentra retorna true
                while (temp != null)
                {
                    if (temp.key.CompareTo(key) == 0)
                    {
                        return true;
                    }
                    else
                    {
                        temp = temp.next;
                    }
                }
                //Si no la encuentra retorna false
                return false;
            }
            else
            {
                return false;
            }
        }
        //Inserta un nuevo elemento con cierta clave
        public void Add(T value, K key)
        {
            HashNode<T, K> newnode = new HashNode<T, K>();
            //Inserta para el caso de ser el primer elemento en insertarse
            if (Length == 0)
            {
                newnode.key = key;
                newnode.value.InsertAtEnd(value);
                start = newnode;
                end = newnode; 
            }
            else {
                //Si no es el primer elemento, verifíca si la clave es nueva y en caso de serlo realiza la insersion para ese caso
                if(!existsKey(key))
                {
                    newnode.key = key;
                    newnode.value.InsertAtEnd(value);
                    end.next = newnode;
                    newnode.prev = end;
                    end = end.next;
                }
                //Si la clave ya existe, busca la llave ya creada y realiza la insersión con caso de colisión
                else
                {
                    HashNode<T, K> temp = start;
                    while (temp.key.CompareTo(key) != 0)
                    {
                        temp = temp.next;
                    }
                    //En este caso la colisión se resuelve con acumulación
                    temp.value.InsertAtEnd(value);
                }
            }
            Length++;
        }
        //Devuelve un elemento en base a una funcion
        public T Get( Func<T,int> comparer, K key)
        {
            if (existsKey(key))
            {
                HashNode<T, K> temp = start;
                while (temp.key.CompareTo(key) != 0)
                {
                    temp = temp.next;
                }
                return temp.value.Find(comparer);
            }
            else
            {
                return default;
            }
        }
        //Elimina un elemento en concreto
        public T Delete(T value, K key)
        {
            //Verifica si existe la llave del elemento, de lo contrario no elimina ningun elemento
            if (existsKey(key))
            {
                HashNode<T, K> temp = start;
                //Sondea entre las llaves hasta buscar la llave que se busca
                while (temp.key.CompareTo(key) != 0)
                {
                    temp = temp.next;
                }
                //En caso de encontrar la llave, se verifica si el elemento se encuentra en la llave obtenida
                int idx = temp.value.GetPositionOf(value);
                T val = temp.value.Find(value);
                //Si se encuentra el elemento, se elimina
                if (idx >= 0)
                {
                    temp.value.Delete(idx);
                }
                //Si la llave del elemento se encuentra vacia despues de la eliminacion, se borra también de la tabla
                if(temp.value.Length == 0)
                {
                    DeleteKey(key);
                }
                Length--;
                return val;
            }
            else
            {
                return default;
            }
        }
        //Elimina una llave en concreto
        private void DeleteKey (K key)
        {
            //Verifica si la tabla no esta vacía y si existe la llave que se quiere eliminar
            if (Length > 0 && existsKey(key))
            {
                //Caso de eliminacion si la llave es la única que existe
                if (Length == 1)
                {
                    start = null;
                    end = null;
                }
                //Casos de eliminación cuando hay más de un solo elemento
                else
                {
                    //Caso de elimnar primer llave
                    if (start.key.CompareTo(key) == 0)
                    {
                        start = start.next;
                        start.prev = null;
                    }
                    //Caso de eliminar última llave
                    else if (end.key.CompareTo(key) == 0)
                    {
                        end = end.prev;
                        end.next = null;
                    }
                    //Caso de eliminar un nodo intermedio
                    else
                    {
                        HashNode<T, K> pretemp = start;
                        HashNode<T, K> temp = start.next;
                        //Sondea hasta encontrar la llave que se busca
                        while (temp.key.CompareTo(key) != 0)
                        {
                            pretemp = temp;
                            temp = temp.next;
                        }
                        //Reasigna los punteros
                        pretemp.next = temp.next;
                        if (temp.next != null)
                        {
                            temp.next.prev = pretemp;
                        }
                    }
                }
            }
        }
        //Obtiene la cantidad de elementos en una clave en concreto
        public int getLenghtOf(K key)
        {
            var node = start;
            //Sondea entre las llaves hasta encontrar la que se busca
            while(node != null && node.key.CompareTo(key)!= 0)
            {
                node = node.next;
            }
            //Si la llave no esta vacía, devuelve la cantidad de elementos en esa llave
            if(node != null)
            {
                return node.value.Length;
            }
            else
            {
                return -1;
            }
        }
        //Devuelve todas las llaves existentes en la tabla
        public IEnumerable<K> GetAllKeys()
        {
            DoubleLinkedList<K> result = new DoubleLinkedList<K>();
            var node = start;
            //Agrega a una lista todas las claves que encuentre
            while(node != null)
            {
                result.InsertAtEnd(node.key);
                node = node.next;
            }
            return result;
        }
        //Devuelve todos los elementos de una clave en concreto
        public IEnumerable<T> GetAllContent(K key)
        {
            if (existsKey(key))
            {
                var node = start;
                //Sondea entre las llaves hasta encontrar la que se busca
                while(node.key.CompareTo(key) != 0 && node != null)
                {
                    node = node.next;
                }
                //Si la llave no esta vacía, devuelve todos los elementos
                if(node != null)
                {
                    return node.value;
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
        //Devuelve todos los elementos de la tabla que cumplen con una función
        public IEnumerable<T> GetAllElementsOf(Func<T, bool> Comparer)
        {
            DoubleLinkedList<T> result = new DoubleLinkedList<T>();
            var node = start;
            //Sondea entre todas las llaves
            while (node != null)
            {
                //Agrega a la lista todos los elementos que cumplen con la función de la clave en la que se encuentra
                for (int i = 0; i < node.value.Length; i++)
                {
                    T val = node.value.Get(i);
                    if (Comparer.Invoke(val))
                    {
                        result.InsertAtEnd(val);
                    }
                }
                node = node.next;
            }
            return result;
        }
        //Devuelve todos los elementos de la tabla
        public IEnumerable<T> GetAllElements()
        {
            DoubleLinkedList<T> result = new DoubleLinkedList<T>();
            var node = start;
            //Sondea entre todas las llaves
            while (node != null)
            {
                //Agrega a la lista todos los elementos de la clave en la que se encuentra
                for (int i = 0; i < node.value.Length; i++)
                {
                    T val = node.value.Get(i);
                    result.InsertAtEnd(val);
                }
                node = node.next;
            }
            return result;
        }
        #endregion
    }
}
