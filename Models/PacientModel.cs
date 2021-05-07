using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace P1_EDD_DAVH_AFPE.Models
{
    public class PacientModel : IComparable
    {
        #region GETS/SETS
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public long DPI { get; set; }
        [Required]
        public string Department{ get; set; }
        [Required]
        public string municipality { get; set; }
        [Required]
        public string priority { get; set; }

        [Required]
        public DateTime schedule { get; set; }

        public bool vaccinated { get; set; }

        #endregion

        #region METHOD

        //Comparer of DPI parameter
        public int CompareTo(object obj)
        {
            var comparer = ((PacientModel)obj);
            int x;
            if (schedule.CompareTo(new DateTime()) == 0)
            {
                if (comparer.schedule.CompareTo(new DateTime()) != 0)
                {
                    return 1;
                }
                else
                {
                    return priority.CompareTo(comparer.priority);
                }
            }
            else if (comparer.schedule.CompareTo(new DateTime()) == 0)
            {
                if (schedule.CompareTo(new DateTime()) != 0)
                {
                    return -1;
                }
                else
                {
                    return priority.CompareTo(comparer.priority);
                }
            }
            x = schedule.CompareTo(comparer.schedule);
            if(x == 0)
            {
                x = priority.CompareTo(comparer.priority);
            }
            return x;
        }
        //Comparar of Priority Parameter
        public int ComparePriority(object obj)
        {
            var comparer = ((PacientModel)obj).priority;
            return comparer.CompareTo(priority);
        }
        #endregion
    }
}
