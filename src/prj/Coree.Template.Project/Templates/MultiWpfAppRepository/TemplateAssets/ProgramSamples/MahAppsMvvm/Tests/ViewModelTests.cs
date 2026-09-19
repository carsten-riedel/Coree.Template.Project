using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using __SourceName__.Extensions;
using __SourceName__.ViewModels;

namespace __SourceName__.Tests
{
    [TestClass]
    [DoNotParallelize]
    public sealed class ViewModelTests
    {
        private IConfigurationRoot configuration = null!;
        private WindowViewModel windowViewModel = null!;
        private NavbarViewModel navbarViewModel = null!;

        [TestInitialize]
        public void Initialize()
        {
            configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Settings:Subkey1:Value1"] = "Configured value",
                })
                .Build();
            windowViewModel = new WindowViewModel(configuration);
            navbarViewModel = new NavbarViewModel(windowViewModel);
        }

        [TestMethod]
        public void EmptyIfNull_handles_null_and_non_null_values()
        {
            string? nullValue = null;

            Assert.AreEqual(String.Empty, nullValue.EmptyIfNull());
            Assert.AreEqual("value", "value".EmptyIfNull());
        }

        [TestMethod]
        public void WindowViewModel_reads_and_reloads_configuration()
        {
            Assert.AreSame(configuration, windowViewModel.Configuration);
            Assert.AreEqual("Configured value", windowViewModel.StatusBar);

            configuration["Settings:Subkey1:Value1"] = "Reloaded value";
            configuration.Reload();

            Assert.AreEqual("Reloaded value", windowViewModel.StatusBar);
        }

        [TestMethod]
        public void Neutral_resources_fall_back_to_english()
        {
            try
            {
                ResourceDesigner.Resource.Culture = CultureInfo.GetCultureInfo("en-US");
                Assert.AreEqual("Goto source code", ResourceDesigner.Resource.CurrentUICultureSpecificString);

                ResourceDesigner.Resource.Culture = CultureInfo.GetCultureInfo("ja-JP");
                Assert.AreEqual("Goto source code", ResourceDesigner.Resource.CurrentUICultureSpecificString);
            }
            finally
            {
                ResourceDesigner.Resource.Culture = null;
            }
        }

        [TestMethod]
        public void NavbarViewModel_updates_the_status_bar()
        {
            Assert.AreSame(windowViewModel, navbarViewModel.WindowViewModel);

            navbarViewModel.IsOpen = true;
            Assert.AreEqual("Pane is True", windowViewModel.StatusBar);

            navbarViewModel.IsOpen = false;
            Assert.AreEqual("Pane is False", windowViewModel.StatusBar);
        }

        [TestMethod]
        public void HomeViewModel_updates_text_navigation_and_sqlite()
        {
            var viewModel = new HomeViewModel(windowViewModel, navbarViewModel);
            Assert.AreSame(windowViewModel, viewModel.WindowViewModel);
            Assert.AreSame(navbarViewModel, viewModel.NavbarViewModel);

            viewModel.TitleSource = "Example";
            Assert.AreEqual("Example", windowViewModel.TitleSource);
            Assert.AreEqual("elpmaxE", viewModel.Hint);
            Assert.AreEqual("Press shift+enter to reset and open pane", viewModel.Helper);

            viewModel.TitleSource = null!;
            Assert.AreEqual(String.Empty, viewModel.Hint);

            viewModel.EnterKeyDownCommand.Execute(null);
            Assert.AreEqual("Source", viewModel.TitleSource);
            Assert.AreEqual("HintAssist.Hint", viewModel.Hint);
            Assert.AreEqual("HintAssist.HelperText", viewModel.Helper);
            Assert.IsTrue(navbarViewModel.IsOpen);

            var originalDirectory = Environment.CurrentDirectory;
            var temporaryDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(temporaryDirectory);

            try
            {
                Environment.CurrentDirectory = temporaryDirectory;
                viewModel.SetTitleSourceStringCommand.Execute(null);
                Assert.AreEqual("Sqlite: ButtonPressed!", viewModel.ButtonText);
                Assert.IsTrue(File.Exists(Path.Combine(temporaryDirectory, "demo.db")));
            }
            finally
            {
                Environment.CurrentDirectory = originalDirectory;
                SqliteConnection.ClearAllPools();
                Directory.Delete(temporaryDirectory, true);
            }
        }

        [TestMethod]
        public void SecondaryViewModels_expose_their_dependencies()
        {
            var about = new AboutViewModel(windowViewModel, navbarViewModel);
            var settings = new SettingsViewModel(windowViewModel, navbarViewModel);

            Assert.AreSame(windowViewModel, about.WindowViewModel);
            Assert.AreSame(navbarViewModel, about.NavbarViewModel);
            Assert.AreSame(windowViewModel, settings.WindowViewModel);
            Assert.AreSame(navbarViewModel, settings.NavbarViewModel);
        }
    }
}
