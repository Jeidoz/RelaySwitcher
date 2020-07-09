using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;

namespace Switcher.Windows
{
    public partial class ThirdPartyLibrariesWnd : Window
    {
        public ThirdPartyLibrariesWnd()
        {
            InitializeComponent();
        }

        private void HyperLink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}