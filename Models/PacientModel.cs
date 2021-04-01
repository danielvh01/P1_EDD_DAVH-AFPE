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

        #endregion

        #region METHOD
        public int CompareTo(object obj)
        {
            var comparer = ((PacientModel)obj).Name;
            return comparer.CompareTo(Name);
        }
        #endregion
    }
}
