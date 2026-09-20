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

        private static readonly Action<ILogger, string, Exception?> LogWindowTitleApplied = LoggerMessage.Define<string>(
            LogLevel.Debug,
            new EventId(1, nameof(MainForm)),
            "Window title applied: {WindowTitle}.");

        private readonly IConfiguration _configuration;
        private readonly ILogger<MainForm> _logger;
        private IDisposable? settingsSubscription;

        public MainForm(
            ILogger<MainForm> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

            InitializeComponent();
            CreateHandle();
            ApplySettings();
            settingsSubscription = ChangeToken.OnChange(_configuration.GetReloadToken, OnSettingsChanged);
        }

        private void ApplySettings()
        {
            Text = _configuration[WindowTitleConfigurationKey] ?? DefaultWindowTitle;
            LogWindowTitleApplied(_logger, Text, null);
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
