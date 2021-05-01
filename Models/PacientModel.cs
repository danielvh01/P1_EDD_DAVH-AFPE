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
        public int DPI { get; set; }
        [Required]
        public string Department{ get; set; }
        [Required]
        public string municipality { get; set; }
        [Required]
        public string priority { get; set; }
        [Required]
        public int age { get; set; }
        [Required]
        public string schedule { get; set; }

        #endregion

        #region METHOD

        //Comparer of DPI parameter
        public int CompareTo(object obj)
        {
            var comparer = ((PacientModel)obj).DPI;
            return comparer.CompareTo(DPI);
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
