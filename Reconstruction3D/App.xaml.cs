using Reconstruction3D.Views;
using System.Windows;

namespace Reconstruction3D
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var mainVindow = new MainWindow();
            mainVindow.Show();
        }
    }
}