using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.Heuristics.Meta.GA
{
    public class CrossOver
    {
        public static IEnumerable<T> OnePoint<T>(IEnumerable<T> sol1, IEnumerable<T> sol2)
        {
            if (sol1 == null || sol2 == null) throw new Exception("None of the Arguments can be null");
            if (sol1.Count() != sol2.Count()) throw new Exception("Element Count in both Arguments must be the same");
            if (sol1.Count() <= 2) throw new Exception("For One Point CrossOver, Enumerable Length must be more than 2");
            int point = Convert.ToInt32(Math.Floor(Number.Rnd() * sol1.Count()));
            List<T> ret = new List<T>();
            for (int i = 0; i < point; i++)
            {
                ret.Add(sol1.ElementAt(i));
            }
            for (int i = point; i < sol2.Count(); i++)
            {
                ret.Add(sol2.ElementAt(i));
            }
            return ret.AsEnumerable();
        }

        public static IEnumerable<T> Random<T>(IEnumerable<T> sol1, IEnumerable<T> sol2)
        {
            if (sol1 == null || sol2 == null) throw new Exception("None of the Arguments can be null");
            if (sol1.Count() != sol2.Count()) throw new Exception("Element Count in both Arguments must be the same");
            List<T> ret = new List<T>();
            for (int i = 0; i < sol1.Count(); i++)
            {
                if (Number.Rnd() < 0.5) ret.Add(sol1.ElementAt(i));
                else ret.Add(sol2.ElementAt(i));
            }
            return ret.AsEnumerable();
        }
    }
}
