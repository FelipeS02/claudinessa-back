using Claudinessa.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Claudinessa.Data.Repositories.Products.Interface
{
    public interface IProductsRepository
    {
        Task<bool> CreateProduct(NProduct product);
        Task<bool> UpdateProduct(NProduct product);
        Task<bool> DeleteProduct(int IdProduct);
        Task<IEnumerable<string>> GetProductsImages();
        Task<Product> GetProduct(int IdProduct);
    }
}
