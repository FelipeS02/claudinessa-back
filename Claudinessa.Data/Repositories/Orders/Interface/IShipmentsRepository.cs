using Claudinessa.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Claudinessa.Data.Repositories.Orders.Interface
{
    public interface IShipmentsRepository
    {
        Task<int> CreateShipment(NShipment shipment);
        Task<int> CreateType(Shipment.Type type);
        Task<int> UpdateType(Shipment.Type type);
        Task<int> UpdateShipment(NShipment shipment);
        Task<List<LShipment>> GetShipments();
        Task<IEnumerable<Shipment.Type>> GetTypes();

        Task<int> DeleteShipment(int IdShipment);

        Task<int> DeleteType(int IdType);
    }
}
