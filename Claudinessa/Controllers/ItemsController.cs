using Claudinessa.Data.Repositories.Orders.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Claudinessa.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IItemsRepository _itemsRepository;

        public ItemsController(IItemsRepository itemsRepository)
        {
            _itemsRepository = itemsRepository;
        }

        [HttpDelete("{idItem}")]
        public async Task<IActionResult> DeleteItem(int idItem)
        {
            return Ok(await _itemsRepository.DeleteItem(idItem));
        }

    }
}
