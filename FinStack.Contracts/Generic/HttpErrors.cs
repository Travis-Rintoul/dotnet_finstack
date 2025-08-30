namespace FinStack.Contracts.Generic;

public static class HttpErrors
{
    public static Error HttpError(int StatusCode, string? ReasonPhrase, string RequestUri) =>
        new("Http.Error", $"{StatusCode} {ReasonPhrase} from {RequestUri}");

    public static Error EmptyBody(int StatusCode, string RequestUri) =>
        new("Http.EmptyBody", $"No content returned from {RequestUri} (status {StatusCode}).");

    public static Error DeserializationError(string? errorPayload) =>
        new("Http.DeserializationError", errorPayload ?? "Unknown JSON error");
}
