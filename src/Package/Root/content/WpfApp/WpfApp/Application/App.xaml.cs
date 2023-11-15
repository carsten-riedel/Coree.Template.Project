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
        
        public readonly IHost host;

        public App()
        {
            host = new HostBuilder().ConfigureServices(ConfigureServices).ConfigureAppConfiguration(ConfigureAppConfiguration).ConfigureLogging(ConfigureLogging).Build();
            Services = host.Services;
        }

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
            #if( ProfileOptimizationStartup )
            System.Runtime.ProfileOptimization.SetProfileRoot(AppDomain.CurrentDomain.BaseDirectory);
            System.Runtime.ProfileOptimization.StartProfile($@"{System.Reflection.Assembly.GetAssembly(this.GetType())!.GetName().Name}.profile");
            #endif
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
            await host.StopAsync(TimeSpan.FromSeconds(3));
        }
        #else
        protected override void OnStartup(StartupEventArgs e)
        {
            this.StartupUri = new Uri(@"..\View\MainWindow.xaml", UriKind.Relative);
            #if( ProfileOptimizationStartup )
            System.Runtime.ProfileOptimization.SetProfileRoot(AppDomain.CurrentDomain.BaseDirectory);
            System.Runtime.ProfileOptimization.StartProfile($@"{System.Reflection.Assembly.GetAssembly(this.GetType())!.GetName().Name}.profile");
            #endif
            base.OnStartup(e);
        }
        #endif


    }
}
