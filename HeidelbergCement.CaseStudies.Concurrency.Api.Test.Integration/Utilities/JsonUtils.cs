using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HeidelbergCement.CaseStudies.Concurrency.Api.Test.Utilities;

public static class JsonUtils
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = GetSerializerOptions();


    private static JsonSerializerOptions GetSerializerOptions()
    {
        var options = new JsonSerializerOptions
        {
            IgnoreNullValues = true,
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };
        options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        return options;
    }
        
    public static async Task<T> DeserializeAsync<T>(Task<Stream> stream)
    {
        return await JsonSerializer.DeserializeAsync<T>(await stream, JsonSerializerOptions);
    }

    public static string Serialize(object content)
    {
        return  JsonSerializer.Serialize(content, JsonSerializerOptions);
    }
}