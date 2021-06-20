using System;
using System.Collections.Generic;
using System.Text;

namespace Organization.Services.Customer.Interfaces
{
    public interface IDatabaseTranslator
    {
        public string GetTable<T>() where T : class;
    }
}
