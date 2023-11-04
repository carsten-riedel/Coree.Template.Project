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

        public Form1()
        {
            InitializeIcon();
            InitializeComponent();
        }
    }
}