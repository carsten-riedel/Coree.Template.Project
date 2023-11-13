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
            set { SetProperty(ref titleSource, value) ; WindowViewModel.TitleSource = titleSource; SetHelperCondition(value); SetHint(value); }
        }

        private void SetHelperCondition(string TitleSource)
        {
            if (TitleSource == "Source")
            {
                Helper = "HintAssist.HelperText";
            }
            else
            {
                Helper = "Press shift+enter to reset and open pane";
            }
        }

        private void SetHint(string TitleSource)
        {
            if (!String.IsNullOrEmpty(TitleSource))
            {
                Hint = String.Concat(TitleSource.Reverse());
            }
            else
            {
                Hint = String.Empty;
            }
        }

        [ObservableProperty]
        private string hint = "HintAssist.Hint";

        [ObservableProperty]
        private string helper = "HintAssist.HelperText";


        [RelayCommand]
        private void SetTitleSourceString()
        {
            TitleSource = "Source";
            Hint = "HintAssist.Hint";
            Helper = "HintAssist.HelperText";
        }

        [RelayCommand]
        public void EnterKeyDown()
        {
            TitleSource = "Source";
            Hint = "HintAssist.Hint";
            Helper = "HintAssist.HelperText";
            NavbarViewModel.IsOpen = !NavbarViewModel.IsOpen;  
        }

        public HomeViewModel()
        {
            WindowViewModel = App.Services!.GetRequiredService<WindowViewModel>();
            NavbarViewModel = App.Services!.GetRequiredService<NavbarViewModel>();
        }
    }
}