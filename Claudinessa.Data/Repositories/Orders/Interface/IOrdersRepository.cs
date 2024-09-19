using Claudinessa.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Claudinessa.Data.Repositories.Orders.Interface
{
    public interface IOrdersRepository
    {
        Task<int> CreateOrder(Order order);
        Task<bool> UpdateState(int IdOrder, int State);
        Task<bool> DeleteOrder(int IdOrder);
        Task<IEnumerable<Order>> GetOrders();
        Task<Order> GetOrderById(int IdOrder);
    }
}
