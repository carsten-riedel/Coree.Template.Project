using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
#if( ViewModel )
using Microsoft.Extensions.DependencyInjection;
using WpfApp.ViewModel;
#endif

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            #if( ViewModel )
            this.DataContext = App.Services!.GetRequiredService<MainWindowViewModel>();
            #endif
            InitializeComponent();
        }

        private void GoToSource(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("https://github.com/carsten-riedel/Coree.Template.Project") { UseShellExecute = true });
        }
    }
}
