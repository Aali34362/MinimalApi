using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using WebHook.Publisher.Api.Models;

namespace WebHook.Publisher.Api.Controllers;

[ApiController]
[Route("api/webhook-publisher")]
public class WebHookPublisherController
    (HttpClient httpClient) 
    : ControllerBase
{
    private readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient), "HttpClient cannot be null");


    [HttpPost("send")]
    public async Task<IActionResult> SendWebhook([FromBody] WebHookPayload payload)
    {
        // Webhook endpoint to which the event will be sent
        string webhookUrl = "https://example.com/api/webhook-consumer/receive";

        try
        {
            var content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync(webhookUrl, content);

            if (response.IsSuccessStatusCode)
            {
                return Ok(new { Message = "Webhook sent successfully" });
            }

            return StatusCode((int)response.StatusCode, new { Message = "Failed to send webhook" });
        }
        catch (System.Exception ex)
        {
            return StatusCode(500, new { Message = "Error sending webhook", Error = ex.Message });
        }
    }
}
