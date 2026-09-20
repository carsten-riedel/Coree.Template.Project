#if( CSharpProjectOptions == "DisableImplicitUsings" )
using System.Drawing;
using System.Windows.Forms;

#endif
namespace __SourceName__
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null!;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _settingsSubscription?.Dispose();
                components?.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            SuspendLayout();
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "MainForm";
            ResumeLayout(false);
        }

        #endregion
    }
}
