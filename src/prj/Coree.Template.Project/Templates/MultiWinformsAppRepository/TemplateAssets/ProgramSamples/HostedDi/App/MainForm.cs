#if( CSharpProjectOptions == "DisableImplicitUsings" )
using System;
using System.Windows.Forms;

#endif
using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

using __SourceName__.Resources;

namespace __SourceName__
{
    internal sealed partial class MainForm : Form
    {
        internal const string WindowTitleConfigurationKey = "Settings:Subkey1:Value1";
        private readonly IConfiguration _configuration;
        private readonly ILogger<MainForm> _logger;
        private IDisposable? _settingsSubscription;

        #region Logging

        [LoggerMessage(EventId = 1, Level = LogLevel.Debug, Message = "Window title applied: {WindowTitle}.")]
        private partial void LogWindowTitleApplied(string windowTitle);

        #endregion

        public MainForm(
            ILogger<MainForm> logger,
            IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(logger);
            ArgumentNullException.ThrowIfNull(configuration);

            _logger = logger;
            _configuration = configuration;

            InitializeComponent();
            CreateHandle();
            ApplySettings();
            _settingsSubscription = ChangeToken.OnChange(_configuration.GetReloadToken, OnSettingsChanged);
        }

        private void ApplySettings()
        {
            string? configuredTitle = _configuration[WindowTitleConfigurationKey];
            Text = configuredTitle == nameof(Strings.AppSettingsWindowTitle)
                ? Strings.AppSettingsWindowTitle
                : configuredTitle ?? Strings.DefaultWindowTitle;
            LogWindowTitleApplied(Text);
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
