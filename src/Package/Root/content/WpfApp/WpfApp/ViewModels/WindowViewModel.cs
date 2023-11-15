using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
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
            ChangeToken.OnChange(() => Configuration.GetReloadToken(), onChange);
        }

        private void onChange()
        {
            Title = Configuration.GetRequiredSection("Settings:KeyOne").Value;
        }
    }
}