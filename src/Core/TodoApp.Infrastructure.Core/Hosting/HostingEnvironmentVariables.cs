namespace TodoApp.Infrastructure.Core.Hosting;
public class HostingEnvironmentVariables
{
    private const string DotNetEnv = "DOTNET_ENVIRONMENT";
    private const string AspNetCoreEnv = "ASPNETCORE_ENVIRONMENT";

    public static string? GetDotNetEnvironment() => DotNetEnv;
    public static string? GetAspNetCoreEnvironment() => AspNetCoreEnv;
}
