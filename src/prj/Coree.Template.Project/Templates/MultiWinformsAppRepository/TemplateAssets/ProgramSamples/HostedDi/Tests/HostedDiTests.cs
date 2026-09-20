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
        public void MainForm_UsesFallbackWithoutConfiguredTitle()
        {
            RunInStaThread(() =>
            {
                IConfiguration configuration = new ConfigurationBuilder().Build();
                var services = new ServiceCollection();
                services.AddSingleton<IConfiguration>(configuration);
                services.AddLogging();
                services.AddSingleton<MainForm>();

                using ServiceProvider serviceProvider = services.BuildServiceProvider();
                using MainForm form = serviceProvider.GetRequiredService<MainForm>();

                Assert.AreEqual("MainForm", form.Text);
            });
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
                services.AddSingleton<MainForm>();

                using ServiceProvider serviceProvider = services.BuildServiceProvider();
                MainForm form = serviceProvider.GetRequiredService<MainForm>();

                Assert.AreEqual("Initial title", form.Text);
                Assert.AreSame(form, serviceProvider.GetRequiredService<MainForm>());

                configuration[MainForm.WindowTitleConfigurationKey] = "Reloaded title";
                configuration.Reload();

                Assert.AreEqual("Reloaded title", form.Text);
            });
        }

        private static IConfigurationRoot CreateConfiguration(string title)
        {
            var values = new Dictionary<string, string?>
            {
                [MainForm.WindowTitleConfigurationKey] = title,
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
