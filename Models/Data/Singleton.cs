using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using DataStructures;


namespace P1_EDD_DAVH_AFPE.Models.Data
{
    public sealed class Singleton
    {
        #region Public Variables
        //Boolean variables
        public bool verif = false;
        public bool SPacient;
        public bool verifAppointmen = false;
        //Integer variable
        public int simmultaneous;
        public int Cont;
        public int hashCapacity;
        public int heapCapacity;
        public int schedule;
        //String Variables
        public string department;
        public string muni;
        //DateTime Variables;
        public DateTime startingDate;
        //DATA STORAGE//
        public string database;
        //Data structures
        public DoubleLinkedList<string> priorities;
        public HashTable<string,string> municipalities;
        public DoubleLinkedList<PacientModel> VaccinatedList;
        public HashTable<PacientModel, long> Data;
        public AVLTree<SearchCriteria<long>> DpiTree;
        public AVLTree<SearchCriteria<string>> NameTree;
        public AVLTree<SearchCriteria<string>> LastNameTree;
        public Heap<PacientModel> HeapPacient;
        #endregion
        #region Private Variables
        private HashTable<PacientModel, long> Database;
        private readonly static Singleton _instance = new Singleton();
        private Heap<PacientModel> HeapDatabase;
        #endregion

        //Inicializacion de variables y estructura de datos
        private Singleton()
        {
            hashCapacity = 15;
            heapCapacity = 15;
            schedule = 30;
            department = "";
            muni = "";
            startingDate = new DateTime();
            Database = new HashTable<PacientModel, long>(hashCapacity);
            priorities = new DoubleLinkedList<string>();
            vaciar();
            #region Priotity insertions
            //Inicializacion de la lista contenedora de areas de trabajo para generar posteriormente las prioridades
            priorities.InsertAtEnd("Area de salud");//1            
            priorities.InsertAtEnd("Area de justicia");
            priorities.InsertAtEnd("Area educativa");
            priorities.InsertAtEnd("Area de seguridad nacional");
            priorities.InsertAtEnd("Cuerpo de socorro");
            priorities.InsertAtEnd("Entidad de servivios esenciales");                   
            priorities.InsertAtEnd("Otros");
            priorities.InsertAtEnd("Persona internada en hogar o institucion del adulto mayor");
            priorities.InsertAtEnd("Trabajador de funeraria o de institucion del adulto mayor");
            #endregion
            //Se guarda en una Tabla Hash la informacion de los municipios contenida en un documento de texto para luego ser usada en la vista "LoginM"
            municipalities = new HashTable<string, string>(22);
            StreamReader sr = new StreamReader("Municipios.txt");
            string result = sr.ReadToEnd();
            string[] lines = result.Split("\r\n");
            string d = "";
            for (int i = 0; i < lines.Length; i++)
            {
                if(lines[i].StartsWith("-"))
                {
                    d = lines[i].Replace("-", "");
                }
                else
                {
                    municipalities.Add(lines[i], d);
                }
            }

        }

        public static Singleton Instance
        {
            get
            {
                return _instance;
            }
        }

        //Metodo que se encarga de validad el dpi / partida de nacimiento ingresado por el usuario
        public bool cuiValid(long dpi)
        {
            //Se realiza la extraccion de datos del DPI usando el metodo substring de Strings
            string cui = dpi.ToString();
            int department = int.Parse(cui.Substring(9,2));
            int municipality = int.Parse(cui.Substring(11, 2));
            if(department >= 1 && department <= 22)
            {
                int i = 0;
                switch (department)
                {
                    case 1:
                        i = municipalities.getLenghtOf("Guatemala");
                        break;
                    case 2:
                        i = municipalities.getLenghtOf("El Progreso");
                        break;
                    case 3:
                        i = municipalities.getLenghtOf("Sacatepéquez");
                        break;
                    case 4:
                        i = municipalities.getLenghtOf("Chimaltenango");
                        break;
                    case 5:
                        i = municipalities.getLenghtOf("Escuintla");
                        break;
                    case 6:
                        i = municipalities.getLenghtOf("Santa Rosa");
                        break;
                    case 7:
                        i = municipalities.getLenghtOf("Sololá");
                        break;
                    case 8:
                        i = municipalities.getLenghtOf("Totonicapán");
                        break;
                    case 9:
                        i = municipalities.getLenghtOf("Quetzaltenango");
                        break;
                    case 10:
                        i = municipalities.getLenghtOf("Suchitepéquez");
                        break;
                    case 11:
                        i = municipalities.getLenghtOf("Retalhuleu");
                        break;
                    case 12:
                        i = municipalities.getLenghtOf("San Marcos");
                        break;
                    case 13:
                        i = municipalities.getLenghtOf("Huehuetenango");
                        break;
                    case 14:
                        i = municipalities.getLenghtOf("Quiché");
                        break;
                    case 15:
                        i = municipalities.getLenghtOf("Baja Verapaz");
                        break;
                    case 16:
                        i = municipalities.getLenghtOf("Alta Verapaz");
                        break;
                    case 17:
                        i = municipalities.getLenghtOf("Petén");
                        break;
                    case 18:
                        i = municipalities.getLenghtOf("Izabal");
                        break;
                    case 19:
                        i = municipalities.getLenghtOf("Zacapa");
                        break;
                    case 20:
                        i = municipalities.getLenghtOf("Chiquimula");
                        break;
                    case 21:
                        i = municipalities.getLenghtOf("Jalapa");
                        break;
                    case 22:
                        i = municipalities.getLenghtOf("Jutiapa");
                        break;
                }
                return municipality >= 1 && municipality <= i;
            }
            else
            {
                return false;
            }
        }

        //Obtiene datos en base al municipio que fue iniciado sesion , los obtiene de la tabla Hash Data y verifica que 
        //el paciente no ha sido vacunado para agregarlo a la lista de espera cada vez que se mande a llamar la vista.
        public void genWaitingList(string municipality)
        {
            HeapPacient = new Heap<PacientModel>(heapCapacity);
            foreach (var item in Singleton.Instance.Database.GetAllElementsOf(x => x.municipality == municipality))
            {
                if (!item.vaccinated)
                {
                    HeapPacient.insertKey(item, item.priority);
                }
            }
        }

        //Es el metodo que se encarga de insertar a los pacientes en las estructuras de datos : Tabla Hash, Arboles de busqueda AVL (nombre,apellido, dpi)
        
        public void Agregar(PacientModel newPacient)
        {
            //genera una llave de tipo long la cual se usara como indice en la insercion a la tabla hash
            long newkey = keyGen(newPacient.DPI);

            //se inserta en la Tabla Hash que contiene los datos de todos los municipios y departamentos
            Database.Add(newPacient, newkey);

            //se inserta en la tabla hash que nada mas se utiliza por cada inicio de sesion
            Data.Add(newPacient, newkey);

            //se realiza la insercion de un PacientModel nada mas con la informacion de DPI
            DpiTree.Root = DpiTree.Insert(new SearchCriteria<long> { value = newPacient.DPI, key = newPacient.DPI }, DpiTree.Root);

            //se realiza la insercion de un PacientModel nada mas con la informacion de Nombre
            NameTree.Root = NameTree.Insert(new SearchCriteria<string> { value = newPacient.Name, key = newPacient.DPI }, NameTree.Root);

            //se realiza la insercion de un PacientModel nada mas con la informacion de Apellido
            LastNameTree.Root = LastNameTree.Insert(new SearchCriteria<string> { value = newPacient.LastName, key = newPacient.DPI }, LastNameTree.Root);

            //Se inserta el pacientModel al heap para que luego la informacion sea capturada en las demas interfaces de la aplicacion
            HeapPacient.insertKey(newPacient, newPacient.priority);
        }

        //Metodo que es llamado en el HomeController para obtener todos los datos en un archivo de texto que contiene todos los pacientes y su respectiva informacion
        public void AddDataBase(PacientModel newPacient)
        {
            long newkey = keyGen(newPacient.DPI);
            Database.Add(newPacient, newkey);
        }

        //Metodo que se encarga de vacias las estructuras de datos utilizadas a lo largo de un inicio de sesion con un municipio
        //Cada vez que el usuario decida cambiar de ubicacion el centro de vacunacion, se vacian las estructuras de datos para que estas se puedan volver a llenar con los datos
        //del municipio posterior elegido.
        private void vaciar()
        {
            Data = new HashTable<PacientModel, long>(hashCapacity);
            DpiTree = new AVLTree<SearchCriteria<long>>();
            NameTree = new AVLTree<SearchCriteria<string>>();
            LastNameTree = new AVLTree<SearchCriteria<string>>();
            VaccinatedList = new DoubleLinkedList<PacientModel>();
            HeapPacient = new Heap<PacientModel>(heapCapacity);
        }

        //Obtiene la fecha de la proxima cita de vacunacion
        public DateTime NextDate(int index)
        {
            if ((index % simmultaneous) == 0)
            {
                startingDate = startingDate.AddMinutes(schedule);
                if(startingDate.Hour > 17)
                {
                    startingDate = startingDate.AddHours(15);
                }
            }
            return startingDate;
        }

        //Login es el metodo encargado de obtener los datos de la base de datos principal. Si el municipio del elemento obtenido coincide con el municipio el cual el usuario ha iniciado sesion
        //Se procede a la agregacion del resto de estructura de datos que se usaran para el respectivo municipio.
        public void Login(string municipality)
        {
            vaciar();
            foreach(var item in Singleton.Instance.Database.GetAllElementsOf(x => x.municipality == municipality))
            {
                long newkey = keyGen(item.DPI);
                Data.Add(item, newkey);
                DpiTree.Root = DpiTree.Insert(new SearchCriteria<long> { value = item.DPI, key = newkey }, DpiTree.Root);
                NameTree.Root = NameTree.Insert(new SearchCriteria<string> { value = item.Name, key = newkey }, NameTree.Root);
                LastNameTree.Root = LastNameTree.Insert(new SearchCriteria<string> { value = item.LastName, key = newkey }, LastNameTree.Root);
                if (item.vaccinated)
                {
                    VaccinatedList.InsertAtEnd(item);
                }
                else{
                    HeapPacient.insertKey(item, item.priority);
                }
            }
        }

        //Metodo encargado de generar las llaves que se le asignan a un pacientModel a la hora de registrarse , recibiendo como parametro el DPI del paciente
        //de tipo Long y devolviendo un Long mod capacidad de la tabla hash configurada al inicio de la ejecucion de la aplicacion.
        public long keyGen(long dpi)
        {
            return dpi % hashCapacity;
        }

        //Metodo encargado de recolectar la informacion de toda la aplicacion para luego concatenar la misma en un string.
        public void BuildData()
        {
            //reinicia la el string de base de datos para volverla a rellenar con la informacion actual
            database = "";
            //obtiene el valor de la capacidad del heap 
            database += "heapCapacity:" + heapCapacity + "\n";
            //obtiene el valor de la capacidad de la tabla hash
            database += "hashCapacity:" + hashCapacity + "\n";
            //invoca el metodo recorrido() que obtiene la informacion de todos los pacientes ingresados a lo largo del uso de la aplicacion
            string ptnts = recorrido();
            if (ptnts.Length > 0)
            {
                ptnts = ptnts.Remove(ptnts.Length - 1);
            }
            database += "pacients:" + ptnts + ";";            
        }

        // obtiene la informacion de todos los pacientes ingresados a lo largo del uso de la aplicacion agregados a la estructura de datos "Tabla Hash - > DataBase"
        public string recorrido()
        {
            //Write all the content of the HashTable.
            string result = "";
            //Por cada paciente situado en la base de datos, utiliza el metodo GetAllElements (IEnumerable)de la Tabla hash para recopilar la informacion y mandarla 
            //al metodo BuilData()
            foreach(var Pacient in Database.GetAllElements())
            {
                result += Pacient.Name + ",";
                result += Pacient.LastName + ",";
                result += Pacient.DPI + ",";
                result += Pacient.Department + ",";
                result += Pacient.municipality + ",";
                result += Pacient.priority + ",";
                result += Pacient.schedule + ",";
                result += Pacient.vaccinated + ";";
            }
            return result;
        }
    }
}
