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
    public partial class HomeViewModel : ObservableObject
    {
        public readonly WindowViewModel WindowViewModel;
        public readonly NavbarViewModel NavbarViewModel;

        private string titleSource = "Source";

        public string TitleSource
        {
            get { return titleSource; }
            set { SetProperty(ref titleSource, value); WindowViewModel.TitleSource = titleSource; }
        }

        [RelayCommand]
        private void SetTitleSourceString()
        {
            TitleSource = "Source";
        }

        public HomeViewModel()
        {
            WindowViewModel = App.Services!.GetRequiredService<WindowViewModel>();
            NavbarViewModel = App.Services!.GetRequiredService<NavbarViewModel>();
        }
    }
}