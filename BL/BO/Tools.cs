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
            Type type = typeof(T);
            foreach (var property in type.GetProperties())
            {
                if (property.GetValue(obj) is ValueType || property.GetValue(obj) is string)
                    str += property.Name + ": " + property.GetValue(obj)+"\n";
                else
                    str += ToStringProperty(property.GetValue(obj));
            }
            return str;
        }
        
    }
}
   
}

