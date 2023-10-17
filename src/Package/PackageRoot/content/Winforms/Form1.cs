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
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            { 
#pragma warning disable CA1416 // Validate platform compatibility
            this.Icon = new Icon(stream);
#pragma warning restore CA1416 // Validate platform compatibility
            }
        }

        #endregion

        public Form1()
        {
            InitializeIcon();
            InitializeComponent();
        }
    }
}