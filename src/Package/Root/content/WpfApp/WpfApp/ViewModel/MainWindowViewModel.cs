using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.ViewModel
{
    internal partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        internal string title = "MainWindow";

        [ObservableProperty]
        internal string titleSource = "Source";

        [RelayCommand]
        private void SetTitleSourceString()
        {
            Debug.WriteLine($"Hello {TitleSource}!");
            TitleSource = "Source";
        }
    }
}