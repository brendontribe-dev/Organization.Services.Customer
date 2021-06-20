using Npgsql;
using Organization.Services.Customer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using System.Linq.Expressions;
using System.Linq;

namespace Organization.Services.Customer.Services
{
    public class RepositoryService : IRepositoryService
    {
        private readonly IDbConnection _connection;
        private readonly IDatabaseTranslator _translator;

        public RepositoryService(
            IDbConnection connection,
            IDatabaseTranslator translator)
        {
            _connection = connection;
            _translator = translator;
        }

        public async Task<bool> Delete<T>(string whereClause) where T : class
        {
            var sql = $"delete from {_translator.GetTable<T>()} {whereClause}";
            return await _connection.ExecuteAsync(sql) != 0;
        }

        public async Task<bool> ExecuteAsync<T>(string sql) where T : class
        {
            return await _connection.ExecuteAsync(sql) != 0;
        }

        public async Task<IEnumerable<T>> SelectAll<T>(string whereClause) where T : class
        {
            var sql = $"select * from {_translator.GetTable<T>()} {whereClause};";
            return await _connection.QueryAsync<T>(sql);
        }

        public async Task<T> SelectSingle<T>(string whereClause) where T : class
        {
            var sql = $"select * from {_translator.GetTable<T>()} {whereClause};";
            return (await _connection.QueryAsync<T>(sql)).First();
        }

        /*
         * TODO: Add Upsert
         * TODO: Add Update
         * TODO: Add Insert
         * TODO: Remove ExecuteAsync as makes less modular and more dependent on postgres
         * i.e. cant swap to another repository as easily
         * also less secure/more vulnerable to sql attacks
         * 
         * public Task<bool> Upsert<T>(IEnumerable<T> elements) where T : class
         * {
         *     throw new NotImplementedException();
         * }
         */
    }
}
