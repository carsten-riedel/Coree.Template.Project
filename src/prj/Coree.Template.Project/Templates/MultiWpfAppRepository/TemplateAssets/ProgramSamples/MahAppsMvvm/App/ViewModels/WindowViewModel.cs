using System;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

using __SourceName__.Extensions;

namespace __SourceName__.ViewModels
{
    public partial class WindowViewModel : ObservableObject
    {
        public IConfiguration Configuration { get; }

        [ObservableProperty]
        public partial string Title { get; set; } = "Window";

        [ObservableProperty]
        public partial string TitleSource { get; set; } = "Source";

        [ObservableProperty]
        public partial string StatusBar { get; set; } = "Statusbar";

        public WindowViewModel(IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration);

            TitleSource = ResourceDesigner.Resource.CurrentUICultureSpecificString;
            Configuration = configuration;
            ChangeToken.OnChange(() => Configuration.GetReloadToken(), OnChange);
            OnChange();
        }

        private void OnChange()
        {
            StatusBar = Configuration.GetSection("Settings:Subkey1:Value1").Get<string>().EmptyIfNull();
        }
    }
}
