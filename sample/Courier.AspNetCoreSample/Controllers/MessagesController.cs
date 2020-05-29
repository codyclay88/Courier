using Courier.AspNetCoreSample.MessageTypes;
using Microsoft.AspNetCore.Mvc;

namespace Courier.AspNetCoreSample.Controllers
{
    [ApiController]
    [Route("/messages")]
    public class MessagesController : ControllerBase
    {
        private readonly ICourier _courier;

        public MessagesController(ICourier courier)
        {
            _courier = courier;
        }

        [HttpPost]
        public ActionResult PostMessage(
            [FromBody] TextMessageDto messageDto)
        {
            _courier.Send(new TextMessage(messageDto.Contents));
            return Ok();
        }
    }

    public class TextMessageDto
    {
        public string Contents { get; set; }
    }
}