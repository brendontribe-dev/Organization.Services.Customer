using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Services.Customer.Interfaces
{
    public interface IRepositoryService
    {
        //TODO: more meaningful responses than just a boolean, should have a result object to return errors instead of just hiding behind a "false".
        //Should let user know what may be wrong with their request so they can fix the opperation.
        Task<bool> Delete<T>(string whereClause) where T : class;
        //TODO: remove ExecuteAsync 
        Task<bool> ExecuteAsync<T>(string whereClause) where T : class;

        Task<T> SelectSingle<T>(string whereClause) where T : class;
        Task<IEnumerable<T>> SelectAll<T>(string whereClause) where T : class;

        //TODO: It is imperative that SelectAll be converted to an expression to reduce dependency on specific sql language / DB 
        //Calling classes should be able to interface with ANY repository and by requiring them to provide sql we prevent this modularity
        //This is hacky and unprofessional as is currently implemented
        //Should be => Task<IEnumerable<Guid>> SelectAll<T>(Expression<Func<T, bool>> predicate) where T : class;
        //used as await _repositoryService.SelectAll<Customer>(x => x.Status == CustomerStatus.Active);
        //A predicate is less vulnerable to sql injection
        //Is more generic
        //Requires less knowledge of SQL from supporting developers
        //WAY less prone to bugs
        //Can be more easily tested in isolation

        /*
         * TODO: implement
        Task<bool> Update<T>(IEnumerable<T> elements) where T : class;
        Task<bool> Update<T>(T element) where T : class;
        */
    }
}
