using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    public static class BooleanExtensions
    {
        public class BooleanValue<T>
        {
            public bool status { get; set; }
            public T Yes { get; set; }
            public T No { get; set; }

            public BooleanValue() {

            }

            public BooleanValue(bool status, T value)
            {
                this.status = status;
                if (status) this.Yes = value;
                if (!status) this.No = value;
            }
        }
        public static BooleanValue<T> If<T> (this bool status, T value)
        {
            if (status)
            {
                BooleanValue<T> ret = new BooleanValue<T>(status, value);
                return ret;
            }
            else
            {
                return new BooleanValue<T>(status, value);
            }
        }

        public static BooleanValue<T> If<T>(this BooleanValue<T> bv, T value)
        {
            if (bv.status)
            {
                BooleanValue<T> ret = new BooleanValue<T>(bv.status, value);
                return ret;
            }
            else
            {
                return bv;
            }
        }

        public static BooleanValue<T> Else<T>(this bool status, T value)
        {
            if (!status)
            {
                BooleanValue<T> ret = new BooleanValue<T>(status, value);
                return ret;
            }
            else
            {
                return new BooleanValue<T>(status, value);
            }
        }

        public static BooleanValue<T> Else<T>(this BooleanValue<T> bv, T value)
        {
            if (!bv.status)
            {
                BooleanValue<T> ret = new BooleanValue<T>(bv.status, value);
                return ret;
            }
            else return bv;
        }

        public static T Resolve<T> (this BooleanValue<T> bv)
        {
            if (bv.status) return bv.Yes;
            else if (!bv.status) return bv.No;
            else return System.Activator.CreateInstance<T>();
        }
    }
}
