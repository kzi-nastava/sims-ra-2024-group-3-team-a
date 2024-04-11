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
        public SideMenuPage()
        {
            InitializeComponent();
        }
        public void CloseSideMenu(object sender, RoutedEventArgs e)
        {
            OwnerMainWindow.SideMenuFrame.Content = null;
        }
        public void ShowAccommodationsPage(object sender, RoutedEventArgs e)
        {
            OwnerMainWindow.MainFrame.Content = new AccommodationsPage(OwnerMainWindow.LoggedInOwner.ToUser());
        }

        public void ShowReviewsPage(object sender, RoutedEventArgs e)
        {
            OwnerMainWindow.MainFrame.Content = new ReviewsPage();
        }

        public void ShowInboxPage(object sender, RoutedEventArgs e)
        {
            OwnerMainWindow.MainFrame.Content = new InboxPage(OwnerMainWindow.LoggedInOwner.ToUser());
        }

        private void LogOut(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            _ownerMainWindow.Close();
        }
    }
}
