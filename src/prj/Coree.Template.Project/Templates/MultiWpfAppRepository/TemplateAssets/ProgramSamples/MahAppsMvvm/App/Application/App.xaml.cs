using System;
using System.Threading;
using System.Windows;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using __SourceName__.ViewModels;
using __SourceName__.Views;

namespace __SourceName__
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IHost? host;
        private Mutex? singleInstanceMutex;

        internal static IServiceProvider? Services { get; private set; }

        private void ConfigureAppConfiguration(HostBuilderContext context, IConfigurationBuilder builder)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
            builder.AddConfiguration(config);
        }

        private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddSingleton<WindowViewModel>();
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<AboutViewModel>();
            services.AddSingleton<NavbarViewModel>();
            services.AddSingleton<SettingsViewModel>();
        }

        private void ConfigureLogging(HostBuilderContext context, ILoggingBuilder builder)
        {
            builder.ClearProviders();
            builder.AddConsole();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            var appName = System.Reflection.Assembly.GetEntryAssembly()!.GetName().Name;
            var mutex = new Mutex(true, appName, out var createdNew);
            if (!createdNew)
            {
                mutex.Dispose();
                MessageBox.Show($"{appName} is already running!", "Multiple Instances not supported.", MessageBoxButton.OK, MessageBoxImage.Error);
                Current.Shutdown();
                return;
            }

            singleInstanceMutex = mutex;
            System.Runtime.ProfileOptimization.SetProfileRoot(AppDomain.CurrentDomain.BaseDirectory);
            System.Runtime.ProfileOptimization.StartProfile($@"{System.Reflection.Assembly.GetAssembly(this.GetType())!.GetName().Name}.profile");
            host = new HostBuilder().ConfigureServices(ConfigureServices).ConfigureAppConfiguration(ConfigureAppConfiguration).ConfigureLogging(ConfigureLogging).Build();
            Services = host.Services;
            await host.StartAsync();
            var mainWindow = new WindowView();
            MainWindow = mainWindow;
            mainWindow.Show();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            if (host != null)
            {
                await host.StopAsync(TimeSpan.FromSeconds(3));
                host.Dispose();
            }

            singleInstanceMutex?.ReleaseMutex();
            singleInstanceMutex?.Dispose();
            base.OnExit(e);
        }
    }
}
