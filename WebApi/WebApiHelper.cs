using Newtonsoft.Json;
using Refit;

namespace WebApi;

public static class WebApiHelper
{
    private const string Host = "https://api.kgb.test.itlabs.top";

    public static IUserApi User { get; }

    static WebApiHelper()
    {
        var jsonSerializerSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented
        };

        var jsonSerializer = new NewtonsoftJsonContentSerializer(jsonSerializerSettings);

        var refitSettings = new RefitSettings
        {
            ContentSerializer = jsonSerializer
        };

        User = RestService.For<IUserApi>(CreateHttpClient(), refitSettings);
    }

    private static HttpClient CreateHttpClient()
    {
        var client = new HttpClient
        {
            BaseAddress = new Uri(Host)
        };
        return client;
    }
}