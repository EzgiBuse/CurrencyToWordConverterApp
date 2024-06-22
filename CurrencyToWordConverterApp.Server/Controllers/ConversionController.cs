using CurrencyToWordConverterApp.Server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyToWordConverterApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversionController : ControllerBase
    {
        private readonly INumberToWordService _numberToWordService;

        public ConversionController(INumberToWordService numberToWordService)
        {
            _numberToWordService = numberToWordService;
        }

        [HttpGet("{amount}")]
        public IActionResult ConvertCurrency(string amount)
        {
            if (string.IsNullOrWhiteSpace(amount))
            {
                return BadRequest("Error: Amount cannot be null or empty.");
            }

            try
            {
                string result = _numberToWordService.Convert(amount);

                if (result.StartsWith("Error"))
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: An unexpected error occurred.");
            }
        }
    }
}


