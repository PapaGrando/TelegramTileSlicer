using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using TelegramCropper.Services;

namespace TelegramCropper.Controllers
{
    [ApiController]
    [Route("bot")]
    public class BotController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post(
            [FromServices] HandleUpdateService handleUpdateService,
            [FromBody] Update update)
        {
            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;

            await handleUpdateService.EchoAsync(update, cancellationToken);

            return Ok();
        }

#if DEBUG
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Get method of controller");
        }
#endif
    }
}
