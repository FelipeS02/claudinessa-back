using Claudinessa.Data.Repositories.Orders.Interface;
using Claudinessa.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Claudinessa.Model.Shipment;

namespace Claudinessa.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ShipmentsController : ControllerBase
    {
        private readonly IShipmentsRepository _shipmentsRepository;

        public ShipmentsController(IShipmentsRepository shipmentsRepository)
        {
            _shipmentsRepository = shipmentsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetShipments()
        {
            return Ok(await _shipmentsRepository.GetShipments());
        }

        [HttpGet]
        public async Task<IActionResult> GetTypes()
        {
            return Ok(await _shipmentsRepository.GetTypes());
        }

        [HttpPost]
        public async Task<IActionResult> CreateShipment([FromBody] NShipment shipment)
        {
            return Ok(await _shipmentsRepository.CreateShipment(shipment));
        }

        [HttpPost]
        public async Task<IActionResult> CreateType([FromBody] Shipment.Type type)
        {
            return Ok(await _shipmentsRepository.CreateType(type));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateShipment([FromBody] NShipment shipment)
        {
            return Ok(await _shipmentsRepository.UpdateShipment(shipment));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateType([FromBody] Shipment.Type type)
        {
            return Ok(await _shipmentsRepository.UpdateType(type));
        }

        [HttpDelete("{IdType}")]
        public async Task<IActionResult> DeleteType(int IdType)
        {
            return Ok(await _shipmentsRepository.DeleteType(IdType));
        }

        [HttpDelete("{IdShipment}")]
        public async Task<IActionResult> DeleteShipment(int IdShipment)
        {
            return Ok(await _shipmentsRepository.DeleteShipment(IdShipment));
        }
    }
}
