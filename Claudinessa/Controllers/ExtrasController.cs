using Claudinessa.Data.Repositories.Products.Interface;
using Claudinessa.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Claudinessa.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExtrasController : ControllerBase
    {
        private readonly IExtrasRepository _extrasRepository;

        public ExtrasController(IExtrasRepository extrasRepository)
        {
            _extrasRepository = extrasRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateExtra([FromBody] NExtra extra)
        {
            return Ok(await _extrasRepository.CreateExtra(extra));
        }

        [HttpDelete("{IdExtra}")]
        public async Task<IActionResult> DeleteExtra(int IdExtra)
        {
            return Ok(await _extrasRepository.DeleteExtra(IdExtra));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateExtra([FromBody] NExtra extra)
        {
            return Ok(await _extrasRepository.UpdateExtra(extra));
        }
    }
}
