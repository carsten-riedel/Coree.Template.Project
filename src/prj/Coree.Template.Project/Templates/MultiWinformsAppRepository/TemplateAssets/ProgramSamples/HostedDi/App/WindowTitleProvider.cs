using Microsoft.Extensions.Configuration;

namespace __SourceName__
{
    internal sealed class WindowTitleProvider
    {
        internal const string ConfigurationKey = "Settings:Subkey1:Value1";
        private const string DefaultWindowTitle = "MainForm";
        private readonly IConfiguration configuration;

        public WindowTitleProvider(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        internal string GetWindowTitle()
        {
            return configuration[ConfigurationKey] ?? DefaultWindowTitle;
        }
    }
}
