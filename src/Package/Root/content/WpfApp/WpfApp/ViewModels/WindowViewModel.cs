using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
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
        [ObservableProperty]
        private string title = "Window";

        [ObservableProperty]
        private string titleSource = "Source";

        [ObservableProperty]
        private string statusBar = "Statusbar";

        public WindowViewModel()
        {

        }
    }
}