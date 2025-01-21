using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebHook.Receiver.Api.Context;
using WebHook.Receiver.Api.HubService;
using WebHook.Receiver.Api.Models;

namespace WebHook.Receiver.Api.Controllers;

public class WebhookController(WebhookReceiverContext context, IHubContext<WebhookHub> hubContext) : Controller
{
    private readonly WebhookReceiverContext _context = context;
    private readonly IHubContext<WebhookHub> _hubContext = hubContext;

    [HttpPost("{uniqueId}")]
    public async Task<IActionResult> ReceiveWebhook(string uniqueId, [FromBody] object payload)
    {
        var headersDict = Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString());
        var webhookData = new WebhookData
        {
            Id = Guid.NewGuid(),
            UniqueId = uniqueId,
            Payload = payload.ToString(),
            Timestamp = DateTime.UtcNow
        };
        webhookData.SetHeadersDictionary(headersDict);

        _context.Webhooks.Add(webhookData);
        await _context.SaveChangesAsync();

        // Notify connected clients via SignalR
        await _hubContext.Clients.Group(uniqueId).SendAsync("NewWebhook", webhookData);

        return Ok();
    }

    [HttpGet("{uniqueId}")]
    public IActionResult GetWebhooks(string uniqueId)
    {
        var webhooks = _context.Webhooks.Where(w => w.UniqueId == uniqueId).ToList();
        return Ok(webhooks);
    }

    [HttpGet("generate")]
    public IActionResult GenerateWebhookUrl()
    {
        var uniqueId = Guid.NewGuid().ToString();
        return Ok(new { Url = $"https://localhost:7185/api/webhook/{uniqueId}" });
    }
}
