using System.Runtime.InteropServices;

public class RustFinancialEngine : IEngineService
{
    private const string LIB_NAME = "/home/travis/Projects/dotnet_finstack/FinStack.Engine/target/release/libfin_stack_engine.so";

    [DllImport(LIB_NAME)]
      public static extern int schedule_job(IntPtr jobCodePtr, IntPtr jobBodyPtr);

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
}