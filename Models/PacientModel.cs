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

        //Comparador encargado de realizar el ordenamiento en base a la fecha, prioridad o dpi del PacientModel en base a las distintas condiciones
        //a lo largo del metodo

        public int CompareTo(object obj)
        {
            var comparer = ((PacientModel)obj);
            int x;
            if (schedule.CompareTo(new DateTime()) == 0)
            {
                //Valida si el pacient model que ingresa viene sin fecha
                if (comparer.schedule.CompareTo(new DateTime()) != 0)
                {
                    return 1;
                }
                else
                {
                    return priority.CompareTo(comparer.priority);
                }
            }
            //Si se compara con uno que si tiene, procede a ser superior
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
            //Si no tiene fecha , compara prioridades para realizar el ordenamiento
            if (x == 0)
            {
                x = priority.CompareTo(comparer.priority);
            }
            //Si las fechas son iguales y si las prioridades son iguales, procede a comparar por DPI como ultima instancia
            if (x == 0)
            {
                x = DPI.CompareTo(comparer.DPI);
            }
            return x;
        }
        //Comparar of Priority Parameter
        
        #endregion
    }
}
