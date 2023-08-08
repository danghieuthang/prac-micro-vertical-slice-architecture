namespace TodoApp.Infrastructure.Core.Hosting;
public class HostingEnvironmentVariables
{
    public static string? GetDotNetEnvironment() => "DOTNET_ENVIRONMENT";
    public static string? GetAspNetCoreEnvironment() => "ASPNETCORE_ENVIRONMENT";
}
