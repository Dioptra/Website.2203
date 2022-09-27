namespace Website.Server;

public static class Helpers
{
#if BLAZOR_SERVER
    public const bool IsBlazorServer = true;
    public const bool IsBlazorWebAssembly = false;
#else
    public const bool IsBlazorServer = false;
    public const bool IsBlazorWebAssembly = true;
#endif
}
