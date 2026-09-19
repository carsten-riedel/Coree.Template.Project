using System;

using CommunityToolkit.Mvvm.ComponentModel;

namespace __SourceName__.ViewModels
{
    public partial class NavbarViewModel : ObservableObject
    {
        public WindowViewModel WindowViewModel { get; }

        [ObservableProperty]
        public partial bool IsOpen { get; set; }

        public NavbarViewModel(WindowViewModel windowViewModel)
        {
            ArgumentNullException.ThrowIfNull(windowViewModel);
            WindowViewModel = windowViewModel;
        }

        partial void OnIsOpenChanged(bool value)
        {
            WindowViewModel.StatusBar = $"Pane is {value}";
        }
    }
}
