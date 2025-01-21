using System.Text.Json;
using System.Xml;
using WebHook.Receiver.Api.Models;

namespace WebHook.Receiver.Api.Storage;

public static class WebhookStorage
{
    private static readonly Dictionary<string, List<WebhookData>> _storage = new();

    public static void Save(string uniqueId, IHeaderDictionary headers, object payload)
    {
        if (!_storage.ContainsKey(uniqueId))
            _storage[uniqueId] = new List<WebhookData>();

        var headersDict = headers.ToDictionary(h => h.Key, h => h.Value.ToString());
        var webhookData = new WebhookData
        {
            Id = Guid.NewGuid(),
            UniqueId = uniqueId,
            Payload = payload.ToString(),
            Timestamp = DateTime.UtcNow
        };
        webhookData.SetHeadersDictionary(headersDict);

        _storage[uniqueId].Add(webhookData);
    }

    public static List<WebhookData> Get(string uniqueId) => _storage.GetValueOrDefault(uniqueId) ?? new List<WebhookData>();
}
