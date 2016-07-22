using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace Extensions.Models
{
    public class Cache
    {
        public static dynamic Add(string key, dynamic value, long milliseconds = 18000000, CacheItemPriority priority = CacheItemPriority.Normal)
        {
            if (HttpContext.Current.Cache.Get(key) == null)
            {
                HttpContext.Current.Cache.Add(key, value, null, DateTime.Now.AddMilliseconds(Convert.ToDouble(milliseconds)), TimeSpan.Zero, priority, null);
            }
            else
            {
                HttpContext.Current.Cache.Remove(key);
                HttpContext.Current.Cache.Add(key, value, null, DateTime.Now.AddMilliseconds(Convert.ToDouble(milliseconds)), TimeSpan.Zero, priority, null);
            }
            return value;
        }

        public static void Remove(string key)
        {
            if (Contains(key))
            {
                HttpContext.Current.Cache.Remove(key);
            }
        }

        public static bool Contains(string key)
        {
            return HttpContext.Current.Cache.Get(key) != null;
        }

        public static T Get<T>(string key, Func<T> retfn = null)
        {
            if (!Contains(key) && retfn != null)
            {
                T rett = Add(key, retfn());
                return rett;
            }
            if (!Contains(key)) return default(T);
            T ret = (T)HttpContext.Current.Cache[key];
            return ret;
        }

        public static List<string> Keys()
        {
            List<string> ret = new List<string>();
            foreach(var c in HttpContext.Current.Cache)
            {
                ret.Add((string)((DictionaryEntry)c).Key);
            }
            return ret;
        }

        public static void Clear()
        {
            foreach (var key in Keys())
            {
                Remove(key);
            }
        }
    }
}
