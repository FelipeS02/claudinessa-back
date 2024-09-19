using Claudinessa.Data.Repositories.Products.Interface;
using Claudinessa.Model;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Claudinessa.Data.Repositories.Products.Repository
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly MySqlConfig _connectionString;

        public ProductsRepository(MySqlConfig connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection DbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public async Task<IEnumerable<string>> GetProductsImages()
        {
            var db = DbConnection();

            try
            {
                string sql = @"SELECT img FROM products;";

                return await db.QueryAsync<string>(sql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> CreateProduct(NProduct product)
        {
            var db = DbConnection();

            try
            {
                string sql =
                    @"INSERT INTO products 
                            (name, img, description, isavailable, isondiscount, hasoptions, categories_idcategory) 
                            VALUES (@Name, @Img, @Description, @IsAvailable, @IsOnDiscount, @HasOptions, @Category);
                            SELECT LAST_INSERT_ID();";

                int productId = await db.ExecuteScalarAsync<int>(sql, product);

                string optionSql =
                    @"INSERT INTO options (name, price, offprice, isdefault, products_idproduct) 
                                     VALUES (@Name, @Price, @OffPrice, @IsDefault, @ProductId);";

                foreach (Option option in product.Options)
                {
                    option.ProductId = productId;
                }

                await db.ExecuteAsync(optionSql, product.Options);

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

        public async Task<bool> UpdateProduct(NProduct product)
        {
            var db = DbConnection();
            try
            {
                string productSql =
                    @"UPDATE products
                      SET name = @Name,
                                          img = @Img,
                                          description = @Description,
                                          isavailable = @IsAvailable,
                                          isondiscount = @IsOnDiscount,
                                          hasoptions = @HasOptions
                                      WHERE idproduct = @Id;";

                string deleteOptionsSql = @"DELETE FROM options WHERE products_idproduct = @Id";

                string optionsSql =
                    @"INSERT INTO options (name, price, offprice, isdefault, products_idproduct)
                      VALUES (@Name, @Price, @OffPrice, @IsDefault, @ProductId);";

                await db.ExecuteAsync(deleteOptionsSql, new { Id = product.Id });

                await db.ExecuteAsync(productSql, product);

                await db.ExecuteAsync(optionsSql, product.Options);

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

        public async Task<bool> DeleteProduct(int IdProduct)
        {
            var db = DbConnection();
            try
            {
                var sql =
                    @"DELETE FROM products 
                        WHERE idproduct = @IdProduct;";

                await db.ExecuteAsync(sql, new { IdProduct });

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

        public async Task<Product> GetProduct(int IdProduct)
        {
            var db = DbConnection();
            try
            {
                var productSql =
                    @"SELECT idproduct as id, name, img, description, isondiscount, isavailable, hasoptions 
                            FROM products 
                            WHERE idproduct = @IdProduct;";

                var optionsSql =
                    @"SELECT idoption as id, name, price, offprice, isdefault, products_idproduct as productid 
                                   FROM options WHERE products_idproduct = @IdProduct";

                var extrasSql = new
                {
                    getCategories = @"SELECT ec.idcategory as id, ec.name, ec.min, ec.max, ec.isquantifiable, ec.isoptional
                                   FROM extras_categories as ec
                                   INNER JOIN products_has_extras_categories as r_p_ec
                                   ON r_p_ec.extras_categories_idcategory = ec.idcategory
                                   WHERE r_p_ec.products_idproduct = @IdProduct;",
                    getExtras = @"SELECT idextra as id, name, price, isavailable
                                  FROM extras
                                  WHERE extras_categories_idcategory = @Id",
                };

                // Obtengo la informacion del producto (Sin extras ni opciones de precio)

                var product = await db.QueryFirstAsync<LProduct>(productSql, new { IdProduct });

                // Agrego las opciones de precio
                product.Options = await db.QueryAsync<Option>(optionsSql, new { IdProduct });

                // Obtengo las categorias sin opciones
                var categories = await db.QueryAsync<LExtrasCategory>(
                    extrasSql.getCategories,
                    new { IdProduct }
                );

                foreach (var c in categories)
                {
                    // Obtengo las opciones de cada categoria
                    c.Extras = await db.QueryAsync<Extra>(extrasSql.getExtras, new { c.Id });
                }

                product.Extras = categories;

                return product;
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
