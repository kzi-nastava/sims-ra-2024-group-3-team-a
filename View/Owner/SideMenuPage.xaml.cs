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

namespace BookingApp.View.Owner
{
    /// <summary>
    /// Interaction logic for SideMenuPage.xaml
    /// </summary>
    public partial class SideMenuPage : Page
    {
        private OwnerMainWindow _ownerMainWindow;
        public SideMenuPage(OwnerMainWindow ownerMainWindow)
        {
            InitializeComponent();
            _ownerMainWindow = ownerMainWindow;
        }
        public void CloseSideMenu(object sender, RoutedEventArgs e)
        {
            _ownerMainWindow.frameSideMenu.Content = null;
        }
        public void ShowAccommodationsPage(object sender, RoutedEventArgs e)
        {
            _ownerMainWindow.frameMain.Content = new AccommodationsPage(_ownerMainWindow);
        }

        public void ShowReviewsPage(object sender, RoutedEventArgs e)
        {
            _ownerMainWindow.frameMain.Content = new ReviewsPage(_ownerMainWindow);
        }

        public void ShowInboxPage(object sender, RoutedEventArgs e)
        {
            _ownerMainWindow.frameMain.Content = new InboxPage(_ownerMainWindow);
        }
    }
}
