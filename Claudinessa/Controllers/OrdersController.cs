using Claudinessa.Data.Repositories.Orders.Interface;
using Claudinessa.Model;
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
            Console.WriteLine(order.Id);
            var items = await _itemsRepository.GetItems(idOrder);

            foreach (var item in items)
            {
                if (item.Id.HasValue)
                {
                    item.Extras = await _itemsRepository.GetItemExtras(item.Id.Value);
                }
            }

            order.Products = items;

            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            int idOrder = await _ordersRepository.CreateOrder(order);

            if (order.Products != null)
            {
                var extrasById = await _itemsRepository.CreateItems(order.Products, idOrder);

                return Ok(await _itemsRepository.AddItemExtras(extrasById, idOrder));
            }

            return Ok(true);
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
