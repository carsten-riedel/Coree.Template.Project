#if( DependencyInjection )
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
#endif
#if( ViewModel )
using WpfApp.ViewModels;
#endif
using WpfApp.Views;
using System;
using System.Windows;
using System.Threading;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #if( DependencyInjection )
        private static IServiceProvider? services;

        public static IServiceProvider? Services { get => services; set => services = value; }
        
        public IHost? host;

        private void ConfigureAppConfiguration(HostBuilderContext context, IConfigurationBuilder builder)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
            builder.AddConfiguration(config);
        }

        private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            #if( ViewModel )
            services.AddSingleton<WindowViewModel>();
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<AboutViewModel>();
            services.AddSingleton<NavbarViewModel>();
            services.AddSingleton<SettingsViewModel>();
            #endif
        }

        private void ConfigureLogging(HostBuilderContext context, ILoggingBuilder builder)
        {
            builder.ClearProviders();
            builder.AddConsole();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            bool createdNew;
            var appName = System.Reflection.Assembly.GetEntryAssembly()!.GetName().Name;
            Mutex m = new Mutex(true, appName, out createdNew);
            if (!createdNew)
            {
                MessageBox.Show($"{appName} is already running!", "Multiple Instances not supported.",MessageBoxButton.OK, MessageBoxImage.Error);
                System.Windows.Application.Current.Shutdown();
                return;
            }

            #if( ProfileOptimizationStartup )
            System.Runtime.ProfileOptimization.SetProfileRoot(AppDomain.CurrentDomain.BaseDirectory);
            System.Runtime.ProfileOptimization.StartProfile($@"{System.Reflection.Assembly.GetAssembly(this.GetType())!.GetName().Name}.profile");
            #endif
            host = new HostBuilder().ConfigureServices(ConfigureServices).ConfigureAppConfiguration(ConfigureAppConfiguration).ConfigureLogging(ConfigureLogging).Build();
            Services = host.Services;
            await host.StartAsync();
            WindowView mainWindow = new();
            if (mainWindow != null)
            {
                mainWindow.Show();
            }
            else
            {
                System.Windows.Application.Current.Shutdown();
            }
        }
        
        protected override async void OnExit(ExitEventArgs e)
        {
            if (host != null)
            {
                await host.StopAsync(TimeSpan.FromSeconds(3));
            }
        }

        #else

        protected override void OnStartup(StartupEventArgs e)
        {
            bool createdNew;
            var appName = System.Reflection.Assembly.GetEntryAssembly()!.GetName().Name;
            Mutex m = new Mutex(true, appName, out createdNew);
            if (!createdNew)
            {
                MessageBox.Show($"{appName} is already running!", "Multiple Instances not supported.",MessageBoxButton.OK, MessageBoxImage.Error);
                System.Windows.Application.Current.Shutdown();
                return;
            }
            #if( ProfileOptimizationStartup )
            System.Runtime.ProfileOptimization.SetProfileRoot(AppDomain.CurrentDomain.BaseDirectory);
            System.Runtime.ProfileOptimization.StartProfile($@"{System.Reflection.Assembly.GetAssembly(this.GetType())!.GetName().Name}.profile");
            #endif
            this.StartupUri = new Uri(@"..\View\MainWindow.xaml", UriKind.Relative);
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            
        }
        #endif


    }
}
