using Claudinessa.Data.Repositories.Orders.Interface;
using Claudinessa.Data.Repositories.Products.Interface;
using Claudinessa.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Claudinessa.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsRepository _productsRepository;

        public ProductsController(IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] NProduct product)
        {
            return Ok(await _productsRepository.CreateProduct(product));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromBody] NProduct product)
        {
            return Ok(await _productsRepository.UpdateProduct(product));
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsImages()
        {
            return Ok(await _productsRepository.GetProductsImages());
        }

        [HttpGet("{IdProduct}")]
        public async Task<IActionResult> GetProduct(int IdProduct)
        {
            return Ok(await _productsRepository.GetProduct(IdProduct));
        }
    }
}
