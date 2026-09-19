using System;

using CommunityToolkit.Mvvm.ComponentModel;

namespace __SourceName__.ViewModels
{
    public partial class AboutViewModel : ObservableObject
    {
        public WindowViewModel WindowViewModel { get; }

        public NavbarViewModel NavbarViewModel { get; }

        public AboutViewModel(WindowViewModel windowViewModel, NavbarViewModel navbarViewModel)
        {
            ArgumentNullException.ThrowIfNull(windowViewModel);
            ArgumentNullException.ThrowIfNull(navbarViewModel);

            WindowViewModel = windowViewModel;
            NavbarViewModel = navbarViewModel;
        }
    }
}
