using FinStack.Common;

namespace FinStack.API.Tests.Helpers;

public static class EndpointHelperMethods
{
    public static async Task<Result<T>> GetAsResultAsync<T>(this HttpClient client, string requestUri)
    {
        var response = await client.GetAsync(requestUri);

        if (!response.IsSuccessStatusCode)
        {
            var meta = await response.Content.ReadFromJsonAsync<ResponseMeta>();
            return Result.Failure<T>(meta!.Errors);
        }

        var content = await response.Content.ReadFromJsonAsync<T>();
        if (content == null)
        {
            return Result.Failure<T>("ERROR", $"Unable to Deserialize {requestUri} content");
        }

        return Result.Success(content);
    }

    public static async Task<Result<O>> PostAsResultAsync<O, I>(this HttpClient client, string requestUri, I body)
    {
        var response = await client.PostAsJsonAsync(requestUri, body);
        if (!response.IsSuccessStatusCode)
        {
            var meta = await response.Content.ReadFromJsonAsync<ResponseMeta>();
            return Result.Failure<O>(meta!.Errors);
        }

        var content = await response.Content.ReadFromJsonAsync<O>();
        if (content == null)
        {
            return Result.Failure<O>("ERROR", $"Unable to Deserialize {requestUri} content");
        }

        return Result.Success(content);
    }
}
