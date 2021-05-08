using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P1_EDD_DAVH_AFPE.Models
{
    public class SearchCriteria<T> : IComparable where T : IComparable
    {
        public T value;
        public long key;

        public int CompareTo(object obj)
        {
            var comparer = ((SearchCriteria<T>)obj).value;
            return comparer.CompareTo(value);
        }
    }
}
