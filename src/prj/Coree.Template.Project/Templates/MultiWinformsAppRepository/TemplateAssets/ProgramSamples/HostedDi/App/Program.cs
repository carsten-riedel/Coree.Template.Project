#if( CSharpProjectOptions == "DisableImplicitUsings" )
using System;
using System.Windows.Forms;

#endif
using System.Reflection;
using System.Runtime;
using System.Threading;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace __SourceName__
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            string applicationName = Assembly.GetEntryAssembly()?.GetName().Name
                ?? typeof(Program).Assembly.GetName().Name
                ?? nameof(__SourceName__);

            ProfileOptimization.SetProfileRoot(AppContext.BaseDirectory);
            ProfileOptimization.StartProfile($"{applicationName}.profile");

            using var singleInstanceMutex = new Mutex(true, applicationName, out bool createdNew);
            if (!createdNew)
            {
                MessageBox.Show(
                    $"{applicationName} is already running!",
                    "Multiple instances are not supported.",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
            ConfigureApplication(builder);

            using IHost host = builder.Build();
            host.StartAsync().GetAwaiter().GetResult();

            try
            {
                ApplicationConfiguration.Initialize();
                Application.Run(host.Services.GetRequiredService<MainForm>());
            }
            finally
            {
                host.StopAsync().GetAwaiter().GetResult();
            }
        }

        private static void ConfigureApplication(HostApplicationBuilder builder)
        {
            builder.Configuration
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            builder.Services.AddHostedDiApplication();
            builder.Services.Configure<ConsoleLifetimeOptions>(options => options.SuppressStatusMessages = true);

            builder.Logging.ClearProviders();
            builder.Logging.AddDebug();
        }
    }
}
