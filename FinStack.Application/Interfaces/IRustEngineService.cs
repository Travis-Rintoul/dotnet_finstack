public interface IRustEngineService
{
    int Configure(EngineConfig config);
    Guid ProcessJob(string JobCode, string Json);
}