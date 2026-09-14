#if( DependencyInjection )
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
#endif
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace Winforms
{
    public partial class Form1 : Form
    {
        #region InitializeIcon

        public void InitializeIcon()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Winforms.Resources.application.ico";
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

        #if( DependencyInjection )
        public readonly IConfiguration Configuration;
        #endif

        public Form1()
        {
            InitializeIcon();
            InitializeComponent();
            this.CreateHandle();
            #if( DependencyInjection )
            Configuration = Program.Services!.GetRequiredService<IConfiguration>();
            ChangeToken.OnChange(() => Configuration.GetReloadToken(), OnChange);
            OnChange();
            #endif
        }

        #if( DependencyInjection )
        private void OnChange()
        {
            this.Invoke((MethodInvoker)delegate { this.Text = Configuration.GetSection("Settings:Subkey1:Value1").Get<string>(); });
        }
        #endif
    }
}