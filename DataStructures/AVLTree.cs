using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataStructures
{
    public class AVLTree<T> where T : IComparable
    {
        #region Variables and instances
        //Punteros de nodos
        public AVLTreeNode<T> Root { get; set; }
        public AVLTreeNode<T> Work { get; set; }

        //Propiedades
        public int Length = 0;

        //Constructor
        public AVLTree()
        {
            Root = null;
            Work = null;
        }
        #endregion

        #region Methods
        //Metodo de insersión de un nuevo elemento
        public AVLTreeNode<T> Insert(T newvalue, AVLTreeNode<T> pNode)
        {
            //Revisa si el nodo en el que se esta vacío
            if (pNode == null)
            {
                Length++;
                return new AVLTreeNode<T>(newvalue);
            }
            //Si el elemento a insertar es menor al nodo actual, se corre al hizo izquierdo
            if (newvalue.CompareTo(pNode.value) < 0)
            {
                pNode.left = Insert(newvalue, pNode.left);
            }
            //Si el elemento a insertar es mayor al nodo actual, se corre al hizo derecho
            else if (newvalue.CompareTo(pNode.value) > 0)
            {
                pNode.right = Insert(newvalue, pNode.right);
            }
            else
            {
                //Si el elemento es repetido se ignora
                return pNode;
            }
            //Se calcula el peso y el factor de balance
            pNode.height = 1 + max(height(pNode.left), height(pNode.right));

            int balance = getBalance(pNode);

            //Si se debe aplicar balanceo, identifica que caso es y realiza las rotaciones necesarias
            //Caso izquierda izquierda
            if (balance > 1 && newvalue.CompareTo(pNode.left.value) < 0)
                return rightRotate(pNode);

            //Caso derecha derecha
            if (balance < -1 && newvalue.CompareTo(pNode.right.value) > 0)
                return leftRotate(pNode);

            //Caso izquierda derecha
            if (balance > 1 && newvalue.CompareTo(pNode.left.value) > 0)
            {
                pNode.left = leftRotate(pNode.left);
                return rightRotate(pNode);
            }

            //Caso derecha irzquierda
            if (balance < -1 && newvalue.CompareTo(pNode.right.value) < 0)
            {
                pNode.right = rightRotate(pNode.right);
                return leftRotate(pNode);
            }
            return pNode;

        }
        //Este método busca un elemento en base a una función
        public T Find(Func<T, int> comparer, AVLTreeNode<T> node)
        {
            //Si el elemento actual no es nulo
            if (node != null)
            {
                //Si el nodo actual es el que se busca, devuelve su valor
                if (comparer.Invoke(node.value) == 0)
                {
                    return node.value;
                }
                //Si el valor actual es menor al nodo actual, se mueve al hijo izquierdo
                if (comparer.Invoke(node.value) < 0)
                {
                    return Find(comparer, node.left);
                }
                //Si el valor actual es mayor al nodo actual, se mueve al hijo derecho
                else
                {
                    return Find(comparer, node.right);
                }
            }

            return default;
        }

        //Este método busca el nodo padre del elemento
        public AVLTreeNode<T> SearchParent(AVLTreeNode<T> node, AVLTreeNode<T> parent)
        {
            AVLTreeNode<T> temp = null;
            //Si el nodo es nulo no devuelve null
            if (node == null)
            {
                return null;
            }
            //Si el nodo actual tiene un hijo izquierdo valida si este es el que se busca
            if (parent.left != null)
            {
                if (parent.left.value.CompareTo(node.value) == 0)
                {
                    //Si se cumple, el nodo actual es el padre
                    return parent;
                }
            }
            //Si el nodo actual tiene un hijo derecho valida si este es el que se busca
            if (parent.right != null)
            {
                if (parent.right.value.CompareTo(node.value) == 0)
                {
                    //Si se cumple, el nodo actual es el padre
                    return parent;
                }
            }
            //Si ninguno de los hijos es igual al que se busca, entonces se compara el valor con el nodo actual
            if (node.value.CompareTo(parent.value) < 0 && parent.left != null)
            {
                //Si es menor repite el proceso con el hijo izquierdo
                temp = SearchParent(node, parent.left);
            }
            if (node.value.CompareTo(parent.value) > 0 && parent.right != null)
            {
                //Si es mayor repite el proceso con el hijo derecho
                temp = SearchParent(node, parent.right);
            }
            return temp;
        }
        //Este método elimina un elemento del árbol
        public AVLTreeNode<T> Delete(AVLTreeNode<T> node, T value)
        {
            //Revisa si el nodo en el que se esta vacío
            if (node == null)
            {
                return node;
            }
            //Si el nodo a eliminar es menor al nodo actual, se mueve al hijo izquierdo
            if (value.CompareTo(node.value) < 0)
            {
                node.left = Delete(node.left, value);
            }
            //Si el nodo a eliminar es mayor al nodo actual, se mueve al hijo derecho
            else if (value.CompareTo(node.value) > 0)
            {
                node.right = Delete(node.right, value);
            }
            //Si el nodo a eliminar es igual al nodo actual revisará el caso de eliminación
            else
            {
                //Elimina para los casos tanto para nodo hoja como para nodo de 1 solo hijo
                if (node.left == null) 
                {
                    return node.right;
                }
                else if (node.right == null)
                {
                    return node.left;
                }
                //Si el nodo a eliminar tiene 2 hijos se elimina y se sustituye por el menor de los mayores
                else
                {
                    node.value = FindMinimum(node.right).value;
                    node.right = Delete(node.right, node.value);
                }
                Length--;
            }
            //Se calcula el peso y el factor de balance
            node.height = max(height(node.left), height(node.right)) + 1;

            int balance = getBalance(node);
            //Si se debe aplicar balanceo, identifica que caso es y realiza las rotaciones necesarias
            //Caso izquierda izquierda
            if (balance > 1 && getBalance(node.left) <= 0)
            {
                return rightRotate(node);
            }
            //Caso izquierda derecha
            if (balance > 1 && getBalance(node.left) > 0) 
            {
                node.left = leftRotate(node.left);
                return rightRotate(node);
            }
            //Caso derecha derecha
            if (balance < -1 && getBalance(node.right) >= 0)
            {
                return leftRotate(node);
            }
            //Caso derecha izquierda
            if (balance < -1 && getBalance(node.right) < 0)
            {
                node.right = rightRotate(node.right);
                return leftRotate(node);
            }
            return node;
        }
        //Metodo que busca el nodo más pequeño de un subárbol
        public AVLTreeNode<T> FindMinimum(AVLTreeNode<T> node)
        {
            //Verifica si el elemento no esta vacío
            if (node == null)
            {
                return default;
            }
            Work = node;
            AVLTreeNode<T> minimum = Work;
            //Obtiene el elemento mas a la izquierda, es decir el menor, y lo devuelve
            while (Work.left != null)
            {
                Work = Work.left;
                minimum = Work;
            }
            return minimum;

        }
        //Método que devuelve el mayor valor entre dos enteros
        int max(int a, int b)
        {
            return (a > b) ? a : b;
        }
        //Obtiene el peso de un sub-árbol
        int height(AVLTreeNode<T> N)
        {
            if (N == null)
                return 0;

            return N.height;
        }
        //Obtiene el factor de balance de un nodo entre sus dos sub-árboles
        int getBalance(AVLTreeNode<T> N)
        {
            if (N == null)
            {
                return 0;
            }

            return height(N.left) - height(N.right);
        }

        //Método que realiza una rotación a la derecha
        AVLTreeNode<T> rightRotate(AVLTreeNode<T> y)
        {
            //Guarda los nodos que se moverán
            AVLTreeNode<T> x = y.left;
            AVLTreeNode<T> z = x.right;
            //Sustituye los hijos para que se realice la rotación
            x.right = y;
            y.left = z;
            //Actualiza el peso de los nodos
            y.height = max(height(y.left), height(y.right)) + 1;
            x.height = max(height(x.left), height(x.right)) + 1;

            return x;
        }
        //Método que realiza una rotación a la izquierda
        AVLTreeNode<T> leftRotate(AVLTreeNode<T> x)
        {
            //Guarda los nodos que se moverán
            AVLTreeNode<T> y = x.right;
            AVLTreeNode<T> z = y.left;
            //Sustituye los hijos para que se realice la rotación
            y.left = x;
            x.right = z;
            //Actualiza el peso de los nodos
            x.height = max(height(x.left), height(x.right)) + 1;
            y.height = max(height(y.left), height(y.right)) + 1;

            return y;
        }
        #endregion
    }
}

