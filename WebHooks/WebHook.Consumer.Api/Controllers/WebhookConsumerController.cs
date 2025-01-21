using Microsoft.AspNetCore.Mvc;
using WebHook.Consumer.Api.Models;

namespace WebHook.Consumer.Api.Controllers;

[ApiController]
[Route("api/webhook-consumer")]
public class WebHookConsumerController : ControllerBase
{
    [HttpPost("receive")]
    public async Task<IActionResult> ReceiveWebhook([FromBody] WebHookPayload payload)
    {
        // Log or process the webhook payload
        await Task.Run(() =>
        {
            // Simulate processing the webhook
            System.Console.WriteLine($"Received Event: {payload.Event}");
            System.Console.WriteLine($"Received Data: {payload.Data}");
        });

        // Respond to the publisher
        return Ok(new { Message = "Webhook received successfully" });
    }
}
