public record EngineConfig
{
    public string user { get; set; }
    public string password { get; set; }
    public string host { get; set; }
    public string port { get; set; }
    public string database { get; set; }
    public string enviroment { get; set; }
}