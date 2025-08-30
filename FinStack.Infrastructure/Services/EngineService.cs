using System.Runtime.InteropServices;
using System.Text;

public class RustEngineService : IRustEngineService
{
  private const string LIB_NAME = "/home/travis/Projects/dotnet_finstack/FinStack.Engine/target/release/libfin_stack_engine.so";

  [DllImport(LIB_NAME)]
  private static extern int schedule_job(IntPtr jobCodePtr, IntPtr jobBodyPtr);

  [DllImport(LIB_NAME)]
  private static extern int configure(byte[] bytes, UIntPtr len);

  public int ProcessJob(string JobCode, string Json)
  {
    IntPtr jobCodePtr = Marshal.StringToHGlobalAnsi(JobCode);
    IntPtr jobBodyPtr = Marshal.StringToHGlobalAnsi(Json);

    int result = schedule_job(jobCodePtr, jobBodyPtr);

    Console.WriteLine($"Result from Rust: {result}");

    Marshal.FreeHGlobal(jobCodePtr);
    Marshal.FreeHGlobal(jobBodyPtr);

    return result;
  }

  public int Configure(object config)
  {
    string json = System.Text.Json.JsonSerializer.Serialize(config);
    byte[] bytes = Encoding.UTF8.GetBytes(json);

    return configure(bytes, (UIntPtr)bytes.Length);
  }
}