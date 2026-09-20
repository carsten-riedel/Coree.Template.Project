#if( CSharpProjectOptions == "DisableImplicitUsings" )
using System;
using System.Collections.Generic;

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
        public void MainForm_UsesConfigurationAndRespondsToReload()
        {
            RunInStaThread(() =>
            {
                IConfigurationRoot configuration = CreateConfiguration("Initial title");
                var services = new ServiceCollection();
                services.AddSingleton<IConfiguration>(configuration);
                services.AddLogging();
                services.AddSingleton<WindowTitleProvider>();
                services.AddSingleton<MainForm>();

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
