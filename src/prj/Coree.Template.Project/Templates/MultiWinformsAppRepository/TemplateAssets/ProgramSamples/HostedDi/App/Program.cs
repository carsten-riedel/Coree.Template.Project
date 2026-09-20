#if( CSharpProjectOptions == "DisableImplicitUsings" )
using System;
using System.Windows.Forms;

#endif
using System.Reflection;
using System.Runtime;
using System.Threading;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using __SourceName__.Resources;

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
                    Strings.MultipleInstancesMessage.Replace("{0}", applicationName, StringComparison.Ordinal),
                    Strings.MultipleInstancesTitle,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            HostApplicationBuilder builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings
            {
                Args = args,
                ContentRootPath = AppContext.BaseDirectory,
            });

            builder.Services.AddSingleton<MainForm>();
            builder.Services.Configure<ConsoleLifetimeOptions>(options => options.SuppressStatusMessages = true);

            builder.Logging.ClearProviders();
            builder.Logging.AddDebug();

            using IHost host = builder.Build();
            try
            {
                host.Start();
                ApplicationConfiguration.Initialize();
                Application.Run(host.Services.GetRequiredService<MainForm>());
            }
            finally
            {
                try
                {
                    host.StopAsync(TimeSpan.FromSeconds(3)).GetAwaiter().GetResult();
                }
                finally
                {
                    singleInstanceMutex.ReleaseMutex();
                }
            }
        }
    }
}
