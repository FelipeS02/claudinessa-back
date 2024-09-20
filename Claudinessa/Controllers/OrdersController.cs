using Claudinessa.Data.Repositories.Orders.Interface;
using Claudinessa.Model;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;

namespace Claudinessa.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IItemsRepository _itemsRepository;
        private readonly IOrdersRepository _ordersRepository;

        public OrdersController(
            IItemsRepository itemsRepository,
            IOrdersRepository ordersRepository
        )
        {
            _itemsRepository = itemsRepository;
            _ordersRepository = ordersRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            return Ok(await _ordersRepository.GetOrders());
        }

        [HttpGet("{idOrder}")]
        public async Task<IActionResult> GetOrderDetails(int idOrder)
        {
            var order = await _ordersRepository.GetOrderById(idOrder);

            var items = await _itemsRepository.GetItems(idOrder);

            foreach (var item in items)
            {
                if (item.Id == 0)
                    return BadRequest("Order item id cannot be zero");

                item.Extras = await _itemsRepository.GetItemExtras(item.Id);
            }

            order.Products = items;

            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            if (order == null)
                return BadRequest("Order cannot be null");

            if (order.Products == null || !order.Products.Any())
                return BadRequest("Order must have at least one product");

            int idOrder = await _ordersRepository.CreateOrder(order);

            var extrasById = await _itemsRepository.CreateItems(order.Products, idOrder);

            return Ok(await _itemsRepository.AddItemExtras(extrasById, idOrder));
        }

        [HttpPut("{idOrder}")]
        public async Task<IActionResult> UpdateState(int idOrder, [FromQuery] int state)
        {
            return Ok(await _ordersRepository.UpdateState(idOrder, state));
        }

        [HttpDelete("{idOrder}")]
        public async Task<IActionResult> DeleteOrder(int idOrder)
        {
            return Ok(await _ordersRepository.DeleteOrder(idOrder));
        }
    }
}
