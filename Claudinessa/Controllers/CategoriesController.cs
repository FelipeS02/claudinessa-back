using Claudinessa.Data.Repositories.Products.Interface;
using Claudinessa.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Claudinessa.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesRepository _categoriesRepository;

        private readonly IProductsRepository _productsRepository;

        private readonly IExtrasRepository _extrasRepository;

        public CategoriesController(
            ICategoriesRepository categoriesRepository,
            IProductsRepository productsRepository,
            IExtrasRepository extrasRepository
        )
        {
            _categoriesRepository = categoriesRepository;
            _productsRepository = productsRepository;
            _extrasRepository = extrasRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsCategories()
        {
            return Ok(await _categoriesRepository.GetProductsCategories());
        }

        [HttpGet]
        public async Task<IActionResult> GetExtrasCategories()
        {
            return Ok(await _categoriesRepository.GetExtrasCategories());
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoriesName()
        {
            return Ok(await _categoriesRepository.GetCategoriesName());
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductsCategory(
            [FromBody] NProductCategory category
        )
        {
            int categoryId = await _categoriesRepository.CreateProductsCategory(category);

            if (category.Products != null && category.Products.Count() > 0)
            {
                foreach (var product in category.Products)
                {
                    product.Category = categoryId;
                    await _productsRepository.CreateProduct(product);
                }
            }

            return Ok();
        }

        [HttpPost("{IdProduct}")]
        public async Task<IActionResult> AddExtrasToProduct(int IdProduct, int IdCategory)
        {
            return Ok(await _categoriesRepository.AddExtrasToProduct(IdProduct, IdCategory));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProductsCategory(
            [FromBody] NProductCategory category
        )
        {
            return Ok(await _categoriesRepository.UpdateProductsCategory(category));
        }

        [HttpPost]
        public async Task<IActionResult> CreateExtrasCategory([FromBody] NExtrasCategory category)
        {
            int categoryId = await _categoriesRepository.CreateExtrasCategory(category);

            if (category.Extras?.Count() > 0)
            {
                foreach (var extra in category.Extras)
                {
                    extra.Category = categoryId;
                    await _extrasRepository.CreateExtra(extra);
                }
            }

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateExtrasCategory([FromBody] NExtrasCategory category)
        {
            return Ok(await _categoriesRepository.UpdateExtrasCategory(category));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCategory([FromQuery] int idCategory, string type)
        {
            return Ok(await _categoriesRepository.DeleteCategory(idCategory, type));
        }
    }
}
