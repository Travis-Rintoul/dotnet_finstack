using System.Text.Json.Serialization;

public class ResponseMeta
{
    public int Code { get; set; }
    public string Message { get; set; } = string.Empty;

    public IEnumerable<Error> Errors { get; set; } = Enumerable.Empty<Error>();
    public IEnumerable<Error> Warnings { get; set; } = Enumerable.Empty<Error>();

}
