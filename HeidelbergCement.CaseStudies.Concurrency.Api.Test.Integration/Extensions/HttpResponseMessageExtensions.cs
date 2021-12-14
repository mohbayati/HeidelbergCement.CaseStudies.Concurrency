using System.Net.Http;
using System.Threading.Tasks;
using HeidelbergCement.CaseStudies.Concurrency.Api.Test.Utilities;

namespace HeidelbergCement.CaseStudies.Concurrency.Api.Test.Extensions;

public static class HttpResponseMessageExtensions
{
    public static Task<T> DeserializeContent<T>(this HttpResponseMessage message)
    {
        return JsonUtils.DeserializeAsync<T>(message.Content.ReadAsStreamAsync());
    }
}