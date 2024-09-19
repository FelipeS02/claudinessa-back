using Claudinessa.Model;
using System.Threading.Tasks;

namespace Claudinessa.Data.Repositories.Products.Interface
{
    public interface ICategoriesRepository
    {
        Task<IEnumerable<LProductCategory>> GetProductsCategories();
        Task<IEnumerable<LExtrasCategory>> GetExtrasCategories();
        Task<int> CreateProductsCategory(NProductCategory category);
        Task<bool> UpdateProductsCategory(NProductCategory category);
        Task<int> CreateExtrasCategory(NExtrasCategory category);
        Task<bool> UpdateExtrasCategory(NExtrasCategory category);
        Task<bool> AddExtrasToProduct(int IdProduct, int IdExtraCategory);
        Task<bool> DeleteCategory(int idCategory, string type);
        Task<IEnumerable<Category>> GetCategoriesName();
    }
}
