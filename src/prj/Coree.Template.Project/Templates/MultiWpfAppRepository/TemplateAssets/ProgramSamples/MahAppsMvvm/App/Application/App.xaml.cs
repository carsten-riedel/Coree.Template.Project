using System;
using System.Threading;
using System.Windows;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (!TryAcquireSingleInstance())
            {
                return;
            }

            StartProfileOptimization();

            var builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings
            {
                Args = e.Args,
                ContentRootPath = AppContext.BaseDirectory,
            });

            builder.Services.AddSingleton<WindowViewModel>();
            builder.Services.AddSingleton<HomeViewModel>();
            builder.Services.AddSingleton<AboutViewModel>();
            builder.Services.AddSingleton<NavbarViewModel>();
            builder.Services.AddSingleton<SettingsViewModel>();
            builder.Services.AddSingleton<WindowView>();

            host = builder.Build();
            Services = host.Services;

            await host.StartAsync();
            MainWindow = host.Services.GetRequiredService<WindowView>();
            MainWindow.Show();
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

        private bool TryAcquireSingleInstance()
        {
            var appName = typeof(App).Assembly.GetName().Name!;
            var mutex = new Mutex(true, appName, out var createdNew);

            if (createdNew)
            {
                singleInstanceMutex = mutex;
                return true;
            }

            mutex.Dispose();
            MessageBox.Show($"{appName} is already running!", "Multiple Instances not supported.", MessageBoxButton.OK, MessageBoxImage.Error);
            Current.Shutdown();
            return false;
        }

        private static void StartProfileOptimization()
        {
            System.Runtime.ProfileOptimization.SetProfileRoot(AppContext.BaseDirectory);
            System.Runtime.ProfileOptimization.StartProfile($"{typeof(App).Assembly.GetName().Name}.profile");
        }
    }
}
