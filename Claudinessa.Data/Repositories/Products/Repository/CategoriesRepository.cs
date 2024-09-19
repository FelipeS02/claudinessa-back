using Claudinessa.Data.Repositories.Products.Interface;
using Claudinessa.Model;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Claudinessa.Data.Repositories.Products.Repository
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly MySqlConfig _connectionString;

        public CategoriesRepository(MySqlConfig connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection DbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        protected static Dictionary<string, string> Types { get; } =
            new Dictionary<string, string> { { "PRODUCT", "PRODUCT" }, { "EXTRA", "EXTRA" } };

        public async Task<IEnumerable<LProductCategory>> GetProductsCategories()
        {
            var db = DbConnection();
            try
            {
                string categoriesSql = @"SELECT idcategory as id, name from products_categories";

                string productsSql =
                    @"SELECT idproduct as id, name, img, description, isondiscount, isavailable, hasoptions 
                                       FROM products WHERE categories_idcategory = @IdCategory";

                string optionsSql =
                    @"SELECT idoption as id, name, price, offprice, isdefault, products_idproduct as productid FROM options 
                                      WHERE products_idproduct = @IdProduct";

                var categories = await db.QueryAsync<LProductCategory>(categoriesSql);

                if (categories.Count() > 0)
                {
                    foreach (var c in categories)
                    {
                        c.Products = await db.QueryAsync<LProduct>(
                            productsSql,
                            new { IdCategory = c.Id }
                        );

                        if (c.Products.Count() > 0)
                        {
                            foreach (var p in c.Products)
                            {
                                p.Options = await db.QueryAsync<Option>(
                                    optionsSql,
                                    new { IdProduct = p.Id }
                                );
                            }
                        }
                    }
                }

                return categories;
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

        public async Task<IEnumerable<LExtrasCategory>> GetExtrasCategories()
        {
            var db = DbConnection();
            try
            {
                string categoriesSql =
                    @"SELECT idcategory as id, name, min, max, isoptional, isquantifiable from extras_categories";

                string extrasSql =
                    @"SELECT idextra as id, name, price, isavailable 
                                     FROM extras WHERE extras_categories_idcategory = @IdCategory";

                var categories = await db.QueryAsync<LExtrasCategory>(categoriesSql);

                if (categories.Count() > 0)
                {
                    foreach (var c in categories)
                    {
                        c.Extras = await db.QueryAsync<Extra>(extrasSql, new { IdCategory = c.Id });
                    }
                }

                return categories;
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

        public async Task<int> CreateProductsCategory(NProductCategory category)
        {
            var db = DbConnection();

            try
            {
                string sql =
                    @"INSERT INTO products_categories (name) 
                                       VALUES (@Name);
                                       SELECT LAST_INSERT_ID();";

                int categoryId = await db.ExecuteScalarAsync<int>(sql, category);

                return categoryId;
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

        public async Task<int> CreateExtrasCategory(NExtrasCategory category)
        {
            var db = DbConnection();

            try
            {
                string sql =
                    @"INSERT INTO extras_categories (name, isoptional, isquantifiable, min, max) 
                                       VALUES (@Name, @IsOptional, @IsQuantifiable, @Min, @Max);
                                       SELECT LAST_INSERT_ID();";

                int categoryId = await db.ExecuteScalarAsync<int>(sql, category);

                return categoryId;
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

        public async Task<bool> UpdateProductsCategory(NProductCategory category)
        {
            var db = DbConnection();

            try
            {
                string sql =
                    @"UPDATE products_categories
                               SET name = @Name
                               WHERE idcategory = @Id";

                await db.ExecuteAsync(sql, category);

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

        public async Task<bool> UpdateExtrasCategory(NExtrasCategory category)
        {
            var db = DbConnection();

            try
            {
                string sql =
                    @"UPDATE extras_categories
                             SET name = @Name,
                                 isoptional = @IsOptional,
                                 isquantifiable = @IsQuantifiable,
                                 min = @Min,
                                 max = @Max
                             WHERE idcategory = @Id";

                await db.ExecuteAsync(sql, category);

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

        public async Task<bool> AddExtrasToProduct(int IdProduct, int IdExtraCategory)
        {
            var db = DbConnection();

            try
            {
                string sql =
                    @"INSERT into products_has_extras_categories
                    VALUES (@IdProduct, @IdExtraCategory);";

                await db.ExecuteAsync(sql, new { IdProduct, IdExtraCategory });

                return true;
            }
            catch (MySqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteCategory(int IdCategory, string type)
        {
            var db = DbConnection();
            try
            {
                string sql = string.Empty;
                if (type.ToUpper() == Types["PRODUCT"])
                {
                    sql =
                        @"DELETE FROM products_categories
                          WHERE idcategory = @IdCategory";
                }
                else
                {
                    sql =
                        @"DELETE FROM extras_categories
                          WHERE idcategory = @IdCategory";
                }

                await db.ExecuteAsync(sql, new { IdCategory });

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

        public async Task<IEnumerable<Category>> GetCategoriesName()
        {
            var db = DbConnection();
            try
            {
                string sql = @"SELECT idcategory as id, name FROM products_categories;";

                var categories = await db.QueryAsync<Category>(sql);

                return categories;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
