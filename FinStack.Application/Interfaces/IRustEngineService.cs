public interface IRustEngineService
{
    int Configure(object config);
    int ProcessJob(string JobCode, string Json);
}