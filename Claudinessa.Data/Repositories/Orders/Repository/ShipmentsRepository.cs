using Claudinessa.Data.Repositories.Orders.Interface;
using Claudinessa.Model;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Claudinessa.Model.Shipment;

namespace Claudinessa.Data.Repositories.Orders.Repository
{
    public class ShipmentsRepository : IShipmentsRepository
    {
        private readonly MySqlConfig _connectionString;

        public ShipmentsRepository(MySqlConfig connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection DbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public async Task<int> CreateShipment(NShipment shipment)
        {
            var db = DbConnection();

            try
            {
                string sql =
                    @"INSERT INTO shipments (name, shipment_types_idtype)
                      VALUES (@Name, @TypeId);";

                var result = await db.ExecuteAsync(sql, shipment);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> CreateType(Shipment.Type type)
        {
            var db = DbConnection();

            try
            {
                string sql =
                    @"INSERT INTO shipment_types (name, value) 
                      VALUES (@Name, @Value);";

                var result = await db.ExecuteAsync(sql, type);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> UpdateType(Shipment.Type type)
        {
            var db = DbConnection();

            try
            {
                string sql =
                    @"UPDATE shipment_types
                     SET name = @Name,
                         value = @Value
                     WHERE idtype = @Id";

                var result = await db.ExecuteAsync(sql, type);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> UpdateShipment(NShipment shipment)
        {
            var db = DbConnection();

            try
            {
                string sql =
                    @"UPDATE shipments
                      SET name = @Name,
                          shipment_types_idtype = @TypeId
                      WHERE idshipment = @Id";

                var result = await db.ExecuteAsync(sql, shipment);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<LShipment>> GetShipments()
        {
            var db = DbConnection();

            try
            {
                string shipmentsSql =
                    @"SELECT idshipment as id, name, shipment_types_idtype as typeid FROM shipments;";
                string typesSql = @"SELECT idtype as id, name, value FROM shipment_types;";

                var shipments = await db.QueryAsync<NShipment>(shipmentsSql);
                var types = await db.QueryAsync<Shipment.Type>(typesSql);

                var shipmentsList = new List<LShipment>();
                foreach (var shipment in shipments)
                {
                    var findType = types.ToList().Find(s => s.Id == shipment.TypeId);

                    if (findType != null)
                    {
                        var s = new LShipment
                        {
                            Id = shipment.Id,
                            Name = shipment.Name,
                            type = findType
                        };

                        shipmentsList.Add(s);
                    }
                    else
                    {
                        throw new Exception("No se encontro el tipo de envio");
                    }
                }

                return shipmentsList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Shipment.Type>> GetTypes()
        {
            var db = DbConnection();

            try
            {
                var sql = @"SELECT idtype as id, name, value FROM shipment_types;";

                var types = await db.QueryAsync<Shipment.Type>(sql);

                return types;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> DeleteShipment(int IdShipment)
        {
            var db = DbConnection();

            try
            {
                var sql = @"DELETE FROM shipments WHERE idshipment = @IdShipment";

                int result = await db.ExecuteAsync(sql, new { IdShipment });

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> DeleteType(int IdType)
        {
            var db = DbConnection();

            try
            {
                var sql = @"DELETE FROM shipment_types WHERE idtype = @IdType";

                int result = await db.ExecuteAsync(sql, new { IdType });

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
