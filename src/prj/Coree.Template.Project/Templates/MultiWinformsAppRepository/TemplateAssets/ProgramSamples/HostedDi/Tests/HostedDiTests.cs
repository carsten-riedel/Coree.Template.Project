#if( CSharpProjectOptions == "DisableImplicitUsings" )
using System;
using System.Collections.Generic;
using System.Linq;

#endif
using System.Threading;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace __SourceName__.Tests
{
    [TestClass]
    public sealed class HostedDiTests
    {
        [TestMethod]
        public void WindowTitleProvider_UsesConfiguredTitle()
        {
            IConfiguration configuration = CreateConfiguration("Configured title");
            var provider = new WindowTitleProvider(configuration);

            Assert.AreEqual("Configured title", provider.GetWindowTitle());
        }

        [TestMethod]
        public void WindowTitleProvider_UsesFallbackWithoutConfiguredTitle()
        {
            IConfiguration configuration = new ConfigurationBuilder().Build();
            var provider = new WindowTitleProvider(configuration);

            Assert.AreEqual("MainForm", provider.GetWindowTitle());
        }

        [TestMethod]
        public void AddHostedDiApplication_RegistersApplicationSingletons()
        {
            var services = new ServiceCollection();

            IServiceCollection returnedServices = services.AddHostedDiApplication();

            Assert.AreSame(services, returnedServices);
            AssertSingleton<WindowTitleProvider>(services);
            AssertSingleton<MainForm>(services);
        }

        [TestMethod]
        public void MainForm_UsesConfigurationAndRespondsToReload()
        {
            RunInStaThread(() =>
            {
                IConfigurationRoot configuration = CreateConfiguration("Initial title");
                var services = new ServiceCollection();
                services.AddSingleton<IConfiguration>(configuration);
                services.AddLogging();
                services.AddHostedDiApplication();

                using ServiceProvider serviceProvider = services.BuildServiceProvider();
                MainForm form = serviceProvider.GetRequiredService<MainForm>();

                Assert.AreEqual("Initial title", form.Text);
                Assert.AreSame(form, serviceProvider.GetRequiredService<MainForm>());

                configuration[WindowTitleProvider.ConfigurationKey] = "Reloaded title";
                configuration.Reload();

                Assert.AreEqual("Reloaded title", form.Text);
            });
        }

        private static IConfigurationRoot CreateConfiguration(string title)
        {
            var values = new Dictionary<string, string?>
            {
                [WindowTitleProvider.ConfigurationKey] = title,
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(values)
                .Build();
        }

        private static void AssertSingleton<TService>(IServiceCollection services)
        {
            ServiceDescriptor descriptor = services.Single(service => service.ServiceType == typeof(TService));
            Assert.AreEqual(ServiceLifetime.Singleton, descriptor.Lifetime);
        }

        private static void RunInStaThread(Action action)
        {
            Exception? failure = null;
            var thread = new Thread(() =>
            {
                try
                {
                    action();
                }
                catch (Exception exception)
                {
                    failure = exception;
                }
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();

            if (failure is not null)
            {
                throw new AssertFailedException($"The STA test action failed: {failure}", failure);
            }
        }
    }
}
