using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public static class Tools
    {
        public static string ToStringProperty<T>(this T obj)
        {
            string str = "";
            if (obj == null) { return str; }
            Type type = obj.GetType();
            foreach (var property in type.GetProperties())
            {
                var value = property.GetValue(obj);
                if (value != null && value is IEnumerable<object>)
                {
                    str += property.Name + ": ";
                    foreach (var property2 in value as IEnumerable<object>)
                        str += property2.ToString();
                }
                else
                    str += property.Name + ": " +value + "\n";
            }
            return str;
        }

    }
}



