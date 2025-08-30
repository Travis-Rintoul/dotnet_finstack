using System.Runtime.InteropServices;
using System.Text;



public class RustEngineService : IRustEngineService
{
  private const string LIB_NAME = "/home/travis/Projects/dotnet_finstack/FinStack.Engine/target/release/libfin_stack_engine.so";

  [DllImport(LIB_NAME)]
  internal static extern int schedule_job(
      [MarshalAs(UnmanagedType.LPUTF8Str)] string commandCode,
      [MarshalAs(UnmanagedType.LPUTF8Str)] string commandBody,
      [Out] byte[] guidOut   // must be length 16
  );

  [DllImport(LIB_NAME)]
  private static extern int configure(byte[] bytes, UIntPtr len);


  public Guid ProcessJob(string jobCode, string json)
  {
    var guidBytes = new byte[16];
    int rc = schedule_job(jobCode, json, guidBytes);
    if (rc != 0)
    {
      throw new InvalidOperationException($"Rust job failed with error {rc}");
    }

    return new Guid(guidBytes);
  }

  public int Configure(EngineConfig config)
  {
    string json = System.Text.Json.JsonSerializer.Serialize(config);
    byte[] bytes = Encoding.UTF8.GetBytes(json);

    return configure(bytes, (UIntPtr)bytes.Length);
  }
}