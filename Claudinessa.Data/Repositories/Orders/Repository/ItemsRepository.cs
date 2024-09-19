using Claudinessa.Data.Repositories.Orders.Interface;
using Claudinessa.Model;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Claudinessa.Data.Repositories.Orders.Repositories
{
    public class ItemsRepository : IItemsRepository
    {
        private readonly MySqlConfig _connectionString;

        public ItemsRepository(MySqlConfig connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection DbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public async Task<Dictionary<int, IEnumerable<Item.Extra>>> CreateItems(
            IEnumerable<Item> products,
            int IdOrder
        )
        {
            var db = DbConnection();
            try
            {
                string sql =
                    @"UPDATE products 
                               SET purchases = purchases + @Amount 
                               WHERE idproduct = @IdProduct;
                               INSERT INTO items (name, img, price, comment, amount, order_idorder) 
                               VALUES (@Name, @Img, @Price, @Comment, @Amount, @IdOrder);
                               SELECT LAST_INSERT_ID();";

                var extrasById = new Dictionary<int, IEnumerable<Item.Extra>>();

                foreach (var item in products)
                {
                    item.IdOrder = IdOrder;
                    int idItem = await db.ExecuteScalarAsync<int>(sql, item);
                    extrasById[idItem] = item.Extras;
                }

                return extrasById;
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

        public async Task<bool> DeleteItem(int IdItem)
        {
            var db = DbConnection();
            try
            {
                string sql = @"DELETE FROM items WHERE iditem = @IdItem";

                await db.ExecuteAsync(sql, new { IdItem });

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

        public async Task<IEnumerable<Item>> GetItems(int IdOrder)
        {
            var db = DbConnection();
            try
            {
                string sql =
                    @"SELECT iditem as id, name, img, price, amount, comment, order_idorder as IdOrder 
                               FROM items WHERE order_idorder = @IdOrder";

                return await db.QueryAsync<Item>(sql, new { IdOrder });
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

        public async Task<bool> AddItemExtras(
            Dictionary<int, IEnumerable<Item.Extra>> extrasById,
            int IdOrder
        )
        {
            // productDictionary = {[idOrder] = Extras}

            var db = DbConnection();

            try
            {
                foreach (var (IdItem, extras) in extrasById)
                {
                    string sql =
                        @"INSERT INTO item_extras (name, amount, price, item_iditem, item_order_idorder)
                                VALUES (@Name, @Amount, @Price, @IdItem, @IdOrder)";

                    await db.ExecuteAsync(
                        sql,
                        extras.Select(e => new { IdOrder, IdItem, e.Price, e.Name, e.Amount })
                    );
                }

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

        public async Task<IEnumerable<Item.Extra>> GetItemExtras(int IdItem)
        {
            var db = DbConnection();
            try
            {
                string sql =
                    @"SELECT iditemextra as id, name, amount, price 
                               FROM item_extras WHERE item_iditem = @IdItem";

                return await db.QueryAsync<Item.Extra>(sql, new { IdItem });
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
