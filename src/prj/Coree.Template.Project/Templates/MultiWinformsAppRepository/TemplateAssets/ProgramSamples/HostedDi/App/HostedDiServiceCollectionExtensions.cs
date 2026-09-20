using Microsoft.Extensions.DependencyInjection;

namespace __SourceName__
{
    internal static class HostedDiServiceCollectionExtensions
    {
        internal static IServiceCollection AddHostedDiApplication(this IServiceCollection services)
        {
            services.AddSingleton<WindowTitleProvider>();
            services.AddSingleton<MainForm>();
            return services;
        }
    }
}
