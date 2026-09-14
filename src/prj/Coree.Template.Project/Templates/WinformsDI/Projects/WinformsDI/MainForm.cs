
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace WinformsDI
{
    public partial class MainForm : Form
    {
        #region InitializeIcon

        public void InitializeIcon()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "WinformsDI.Resources.application.ico";
            if (assembly != null)
            {
                using Stream? stream = assembly.GetManifestResourceStream(resourceName);
                if (stream != null)
                {
                    this.Icon = new Icon(stream);
                }
            }
        }

        #endregion


        private readonly IConfiguration configuration;
        private readonly ILogger<MainForm> logger;

        public MainForm(ILogger<MainForm> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
            InitializeIcon();
            InitializeComponent();
            this.CreateHandle();
            ChangeToken.OnChange(() => configuration.GetReloadToken(), () => { Invoke((MethodInvoker)delegate { AppSettingsInit(); }); });
            logger.LogInformation("Hide the console window by setting OutputType to 'WinExe' in the project file (csproj).");
            AppSettingsInit();
        }

        private void AppSettingsInit()
        {
            this.Text = configuration.GetSection("Settings:Subkey1:Value1").Get<string>();
        }

    }
}