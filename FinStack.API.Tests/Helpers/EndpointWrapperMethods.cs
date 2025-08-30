using System.Collections;
using System.Net;
using System.Text.Json;
using FinStack.Common;
using FinStack.Contracts.Generic;
using Xunit.Abstractions;

public static class EndpointHelperMethods
{
    public static class TestOutputBridge
    {
        public static ITestOutputHelper? Output { get; set; }
    }

    public static async Task<Result<T>> GetAsResultAsync<T>(this HttpClient client, string requestUri)
    {
        var response = await client.GetAsync(requestUri);
        return await BuildResult<T>(response, requestUri);
    }

    public static async Task<Result<O>> PostAsResultAsync<O, I>(this HttpClient client, string requestUri, I body)
    {
        var response = await client.PostAsJsonAsync(requestUri, body);
        return await BuildResult<O>(response, requestUri);
    }

    private static async Task<Result<T>> BuildResult<T>(HttpResponseMessage response, string requestUri)
    {
        if (!response.IsSuccessStatusCode)
        {
            var meta = await SafeDeserialize<ResponseMeta>(response, requestUri);

            if (meta.success && meta.value != null)
            {
                return Result.Failure<T>(meta.value.Errors);
            }

            return Result.Failure<T>(HttpErrors.HttpError((int)response.StatusCode, response.ReasonPhrase, requestUri));
        }

        if (response.StatusCode == HttpStatusCode.NoContent || response.Content.Headers.ContentLength == 0)
        {
            if (IsSequenceType(typeof(T)))
            {
                return Result.Success((T)CreateEmptySequence(typeof(T)));
            }

            return Result.Failure<T>(HttpErrors.EmptyBody((int)response.StatusCode, requestUri));
        }

        var payload = await SafeDeserialize<T>(response, requestUri);
        if (payload.success && payload.value != null)
        {
            var ok = payload.value;
            return Result.Success(ok);
        }

        return Result.Failure<T>(HttpErrors.DeserializationError(payload.error));
    }

    private static async Task<(bool success, T? value, string? error)> SafeDeserialize<T>(HttpResponseMessage response, string requestUri)
    {
        string raw = string.Empty;

        try
        {
            raw = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(raw))
            {
                if (IsSequenceType(typeof(T)))
                {
                    return (true, (T)CreateEmptySequence(typeof(T)), null);
                }

                return (false, default, "Empty response body");
            }

            var obj = JsonSerializer.Deserialize<T>(raw, new JsonSerializerOptions(JsonSerializerDefaults.Web));
            if (obj is null)
            {
                return (false, default, "Deserialized to null");
            }

            return (true, obj, null);
        }
        catch (JsonException jx)
        {
            TestOutputBridge.Output?.WriteLine($"JSON error while deserializing {typeof(T).Name} from {requestUri}: {jx}\nRaw body:\n{raw}");
            return (false, default, jx.Message);
        }
        catch (Exception ex)
        {
            TestOutputBridge.Output?.WriteLine($"Unexpected error while reading response from {requestUri}: {ex}\nRaw body:\n{raw}");
            return (false, default, ex.Message);
        }
    }

    private static bool IsSequenceType(Type t)
    {
        if (t == typeof(string))
        {
            return false;
        }

        return typeof(IEnumerable).IsAssignableFrom(t);
    }

    private static object CreateEmptySequence(Type t)
    {
        if (t.IsArray)
        {
            var elem = t.GetElementType()!;
            return Array.CreateInstance(elem, 0);
        }

        var ienum = t.IsGenericType ? t.GetGenericArguments()[0] : typeof(object);
        var listType = typeof(List<>).MakeGenericType(ienum);

        return Activator.CreateInstance(listType)!;
    }
}
