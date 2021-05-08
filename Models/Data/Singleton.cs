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

        public bool cuiValid(long dpi)
        {
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

        public void genWaitingList(string municipality)
        {
            HeapPacient = new Heap<PacientModel>(heapCapacity);
            foreach (var item in Singleton.Instance.Database.GetAllElementsOf(x => x.municipality == municipality))
            {
                if (!item.vaccinated)
                {
                    HeapPacient.insertKey(item);
                }
            }
        }
        public void Agregar(PacientModel newPacient)
        {
            long newkey = keyGen(newPacient.DPI);
            Database.Add(newPacient, newkey);
            Data.Add(newPacient, newkey);
            DpiTree.Root = DpiTree.Insert(new SearchCriteria<long> { value = newPacient.DPI, key = newPacient.DPI }, DpiTree.Root);
            NameTree.Root = NameTree.Insert(new SearchCriteria<string> { value = newPacient.Name, key = newPacient.DPI }, NameTree.Root);
            LastNameTree.Root = LastNameTree.Insert(new SearchCriteria<string> { value = newPacient.LastName, key = newPacient.DPI }, LastNameTree.Root);
            HeapPacient.insertKey(newPacient);
        }
        public void AddDataBase(PacientModel newPacient)
        {
            long newkey = keyGen(newPacient.DPI);
            Database.Add(newPacient, newkey);
        }
        private void vaciar()
        {
            Data = new HashTable<PacientModel, long>(hashCapacity);
            DpiTree = new AVLTree<SearchCriteria<long>>();
            NameTree = new AVLTree<SearchCriteria<string>>();
            LastNameTree = new AVLTree<SearchCriteria<string>>();
            VaccinatedList = new DoubleLinkedList<PacientModel>();
            HeapPacient = new Heap<PacientModel>(heapCapacity);
        }
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
                    HeapPacient.insertKey(item);
                }
            }
        }

        public long keyGen(long dpi)
        {
            return dpi % hashCapacity;
        }

        public void BuildData()
        {
            database = "";
            database += "heapCapacity:" + heapCapacity + "\n";
            database += "hashCapacity:" + hashCapacity + "\n";
            string ptnts = recorrido();
            if (ptnts.Length > 0)
            {
                ptnts = ptnts.Remove(ptnts.Length - 1);
            }
            database += "pacients:" + ptnts + ";";            
        }

        public string recorrido()
        {
            //Write all the content of the HashTable.
            string result = "";
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
