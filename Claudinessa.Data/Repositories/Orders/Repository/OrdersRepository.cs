using Claudinessa.Data.Repositories.Orders.Interface;
using Claudinessa.Model;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Claudinessa.Data.Repositories.Orders.Repository
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly MySqlConfig _connectionString;

        public OrdersRepository(MySqlConfig connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection DbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public async Task<int> CreateOrder(Order order)
        {
            var db = DbConnection();
            try
            {
                //Calcular precio desde el front
                string orderSql =
                    @"INSERT INTO orders (price, shipment, client, phone, adress, house_number, instructions, complement, neighborhood, method, service, state)
                                   VALUES (@Price, @Shipment, @Client, @Phone, @Adress, @HouseNumber, @Instructions, @Complement, @Neighborhood, @Method, @Service, @State);
                                   SELECT LAST_INSERT_ID();";

                string userSql =
                    @"INSERT INTO users (client, phone, orders)
                                       VALUES (@Client, @Phone, 1)
                                       ON DUPLICATE KEY
                                       UPDATE orders = orders + 1;";

                int idOrder = await db.ExecuteScalarAsync<int>(orderSql, order);
                await db.ExecuteAsync(userSql, order);
                return idOrder;
            }
            catch
            {
                throw;
            }
            finally
            {
                db.Close();
            }
        }

        public async Task<bool> UpdateState(int IdOrder, int State)
        {
            var db = DbConnection();
            try
            {
                string sql =
                    @"UPDATE orders
                      SET state = @State
                      WHERE idorder = @IdOrder;";

                await db.ExecuteAsync(sql, new { State, IdOrder });

                return true;
            }
            catch
            {
                throw;
            }
            finally
            {
                db.Close();
            }
        }

        public async Task<bool> DeleteOrder(int IdOrder)
        {
            var db = DbConnection();
            try
            {
                string sql =
                    @"DELETE FROM orders
                               WHERE idorder = @IdOrder";

                await db.ExecuteAsync(sql, new { IdOrder });

                return true;
            }
            catch (MySqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Close();
            }
        }

        public async Task<IEnumerable<Order>> GetOrders()
        {
            var db = DbConnection();
            try
            {
                string sql =
                    @"SELECT idorder as id, price, shipment, state, method, service, 
                                      client, phone, adress, instructions, created, complement, neighborhood, house_number 
                               FROM orders;";

                return await db.QueryAsync<Order>(sql);
            }
            catch (MySqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Close();
            }
        }

        public async Task<Order> GetOrderById(int IdOrder)
        {
            var db = DbConnection();
            try
            {
                string sql =
                    @"SELECT idorder as id, price, shipment, state, method, service, 
                             client, phone, adress, instructions, created, complement, neighborhood, house_number 
                      FROM orders
                      WHERE idorder = @IdOrder;";

                return await db.QueryFirstOrDefaultAsync<Order>(sql, new { IdOrder });
            }
            catch (MySqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Close();
            }
        }
    }
}
