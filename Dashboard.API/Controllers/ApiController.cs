using Dashboard.BLL.Services;
using Dashboard.DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.API.Controllers
{
    [ApiController]
    [Route("api/tools")]
    public class ApiController : BaseController
    {
        [HttpGet("currency")]
        public async Task<IActionResult> GetCurrencyAsync()
        {
            const string url = @"https://api.privatbank.ua/p24api/pubinfo?json&exchange&coursid=5";

            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);
            var content = (List<CurrencyVM>)(await response.Content.ReadFromJsonAsync(typeof(List<CurrencyVM>)));

            if(content != null)
            {
                return Ok(ServiceResponse.GetOkResponse("Валюта", content));
            }

            return BadRequest(ServiceResponse.GetBadRequestResponse("Не вдалося отримати валюту"));
        }
    }
}
