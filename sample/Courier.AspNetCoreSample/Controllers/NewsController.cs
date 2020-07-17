using Courier.AspNetCoreSample.MessageTypes;
using Microsoft.AspNetCore.Mvc;

namespace Courier.AspNetCoreSample.Controllers
{
    [ApiController]
    [Route("/news")]
    public class NewsController : ControllerBase
    {
        private readonly ICourier _courier;

        public NewsController(ICourier courier)
        {
            _courier = courier;
        }

        [HttpPost]
        public ActionResult PostMessage(
            [FromBody] TextMessageDto messageDto)
        {
            _courier.Dispatch(new SomethingHappenedEvent(messageDto.Contents));
            return Ok();
        }
    }

    public class TextMessageDto
    {
        public string Contents { get; set; }
    }
}