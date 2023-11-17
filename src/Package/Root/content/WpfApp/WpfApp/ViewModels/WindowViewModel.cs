using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using WpfApp.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.ViewModels
{
    public partial class WindowViewModel : ObservableObject
    {
        public readonly IConfiguration Configuration;

        [ObservableProperty]
        private string title = "Window";

        [ObservableProperty]
        private string titleSource = "Source";

        [ObservableProperty]
        private string statusBar = "Statusbar";

        public WindowViewModel()
        {
            Configuration = App.Services!.GetRequiredService<IConfiguration>();
            ChangeToken.OnChange(() => Configuration.GetReloadToken(), OnChange);
            OnChange();
        }

        private void OnChange()
        {
            StatusBar = Configuration.GetSection("Settings:Subkey1:Value1").Get<string>().EmptyIfNull();
        }
    }
}