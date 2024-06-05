using BookingApp.ViewModel.Owner;
using GalaSoft.MvvmLight.Messaging;
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
using System.Windows.Media.Animation;
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
        private SideMenuViewModel _sideMenuViewModel;

        public SideMenuPage()
        {
            InitializeComponent();
            _sideMenuViewModel = new SideMenuViewModel();
            DataContext = _sideMenuViewModel;

            Messenger.Default.Register<NotificationMessage>(this, (message) =>
            {
                if (message.Notification == "CloseSideMenu")
                {
                    Storyboard storyboard = this.Resources["SlideOutStoryboard"] as Storyboard;
                    storyboard.Begin();
                }
            });        
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Storyboard storyboard = this.Resources["SlideInStoryboard"] as Storyboard;
            storyboard.Begin();
        }

        private void SlideOutStoryboard_Completed(object sender, EventArgs e)
        {
            OwnerMainWindow.SideMenuFrame.Content = null;
        }
    }
}
