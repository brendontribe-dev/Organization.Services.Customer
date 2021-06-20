using Organization.Services.Customer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Organization.Services.Customer.Services
{
    public class DatabaseTranslator : IDatabaseTranslator
    {
        public string GetTable<T>() where T : class
        {
            return ConvertPOCOToPostgres(typeof(T).Name);
        }

        private string BuildColumns<T>()
        {
            var result = string.Empty;

            //Further info on issues
            //https://mattwarren.org/2016/12/14/Why-is-Reflection-slow/
            var props = typeof(T).GetProperties();
            foreach(PropertyInfo p in props)
            {
                if (string.IsNullOrWhiteSpace(result)) result += $"\"{p.Name}\"";
                else result += $", \"{ p.Name }\""; 
            }
            return result;
        }
        
        //public for test purposes
        public string ConvertPOCOToPostgres(string name)
        {
            var sb = new StringBuilder();
            sb.Append(char.ToLower(name[0]));

            foreach(char c in name.Skip(1))
            {
                if (char.IsUpper(c)) sb.Append($"_{char.ToLower(c)}");
                else sb.Append(c);
            }
            return sb.ToString();
        }
    }
}
