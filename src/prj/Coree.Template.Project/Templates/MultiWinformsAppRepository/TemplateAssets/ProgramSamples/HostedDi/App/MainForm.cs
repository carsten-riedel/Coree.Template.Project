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
        private static readonly Action<ILogger, Exception?> LogMainWindowInitialized = LoggerMessage.Define(
            LogLevel.Information,
            new EventId(1, nameof(MainForm)),
            "Main window initialized with reloadable configuration.");

        private readonly WindowTitleProvider windowTitleProvider;
        private IDisposable? settingsSubscription;

        public MainForm(
            ILogger<MainForm> logger,
            IConfiguration configuration,
            WindowTitleProvider windowTitleProvider)
        {
            this.windowTitleProvider = windowTitleProvider;

            InitializeComponent();
            CreateHandle();
            ApplySettings();
            settingsSubscription = ChangeToken.OnChange(configuration.GetReloadToken, OnSettingsChanged);

            LogMainWindowInitialized(logger, null);
        }

        private void ApplySettings()
        {
            Text = windowTitleProvider.GetWindowTitle();
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
