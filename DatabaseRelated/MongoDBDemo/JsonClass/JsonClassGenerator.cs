using Newtonsoft.Json.Linq;
using System.Text;

namespace MongoDBDemo.JsonClass;

public static class JsonClassGenerator
{
    public static string GenerateClass(string json, string className)
    {
        JObject jObject = new JObject(json);
        var sb = new StringBuilder();
        sb.AppendLine("using System;");
        sb.AppendLine("using System.Collections.Generic;");
        sb.AppendLine($"public class {className}");
        sb.AppendLine("{");
        foreach (var property in jObject.Properties())
        {
            var type = GetType(property.Value);
            sb.AppendLine($"    public {type} {property.Name} {{ get; set; }}");
        }
        sb.AppendLine("}");
        return sb.ToString();
    }
    private static string GetType(JToken token) =>    
        token.Type switch
        {
            JTokenType.Object => "object",
            JTokenType.Array => "List<object>",
            JTokenType.Integer => "int",
            JTokenType.Float => "double",
            JTokenType.String => "string",
            JTokenType.Boolean => "bool",
            JTokenType.Date => "DateTime",
            JTokenType.Bytes => "byte[]",
            JTokenType.Guid => "Guid",
            JTokenType.Uri => "Uri",
            JTokenType.TimeSpan => "TimeSpan",
            _ => "string",
        };
}
