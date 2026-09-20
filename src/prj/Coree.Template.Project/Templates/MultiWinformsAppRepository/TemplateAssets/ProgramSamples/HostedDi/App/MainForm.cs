#if( CSharpProjectOptions == "DisableImplicitUsings" )
using System;
using System.Windows.Forms;

#endif
using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace __SourceName__
{
    internal sealed partial class MainForm : Form
    {
        internal const string WindowTitleConfigurationKey = "Settings:Subkey1:Value1";
        private const string DefaultWindowTitle = "MainForm";

        private static readonly Action<ILogger, Exception?> LogMainWindowInitialized = LoggerMessage.Define(
            LogLevel.Information,
            new EventId(1, nameof(MainForm)),
            "Main window initialized with reloadable configuration.");

        private readonly IConfiguration configuration;
        private IDisposable? settingsSubscription;

        public MainForm(
            ILogger<MainForm> logger,
            IConfiguration configuration)
        {
            this.configuration = configuration;

            InitializeComponent();
            CreateHandle();
            ApplySettings();
            settingsSubscription = ChangeToken.OnChange(configuration.GetReloadToken, OnSettingsChanged);

            LogMainWindowInitialized(logger, null);
        }

        private void ApplySettings()
        {
            Text = configuration[WindowTitleConfigurationKey] ?? DefaultWindowTitle;
        }

        [ExcludeFromCodeCoverage]
        private void OnSettingsChanged()
        {
            if (IsDisposed || Disposing)
            {
                return;
            }

            if (InvokeRequired)
            {
                BeginInvoke(ApplySettings);
                return;
            }

            ApplySettings();
        }
    }
}
