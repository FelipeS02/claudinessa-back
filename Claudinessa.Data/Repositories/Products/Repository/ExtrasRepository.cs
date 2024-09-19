using Claudinessa.Data.Repositories.Products.Interface;
using Claudinessa.Model;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using static Claudinessa.Model.Item;

namespace Claudinessa.Data.Repositories.Products.Repository
{
    public class ExtrasRepository : IExtrasRepository
    {
        private readonly MySqlConfig _connectionString;

        public ExtrasRepository(MySqlConfig connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection DbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }
        public async Task<bool> CreateExtra(NExtra extra)
        {
            var db = DbConnection();
            try
            {
                string sql = @"INSERT INTO extras (name, price, isavailable, extras_categories_idcategory) 
                               VALUES (@Name, @Price, @IsAvailable, @Category);";

                return await db.ExecuteAsync(sql, extra) > 0;
            }
            catch (MySqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally { db.Close(); }
        }
        public async Task<bool> UpdateExtra(NExtra extra)
        {
            var db = DbConnection();

            try
            {
                string sql = @"UPDATE extras (name, price, isavailable, extras_categories_idcategory) 
                        VALUES (@Name, @Price, @IsAvailable, @Category) 
                        WHERE idextra = @Id;";

                return await db.ExecuteAsync(sql, extra) > 0;
            }
            catch (MySqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally { db.Close(); }

        }
        public async Task<bool> DeleteExtra(int IdExtra)
        {
            var db = DbConnection();

            try
            {
                string sql = @"DELETE FROM extras WHERE idextra = @IdExtra;";

                return await db.ExecuteAsync(sql, new { IdExtra }) > 0;
            }
            catch (MySqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally { db.Close(); }
        }
    }
}
