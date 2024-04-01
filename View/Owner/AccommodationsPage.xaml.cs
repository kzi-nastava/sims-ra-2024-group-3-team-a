using BookingApp.DTO;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for AccommodationsPage.xaml
    /// </summary>
    public partial class AccommodationsPage : Page
    {
        public static ObservableCollection<AccommodationDTO> AccommodationsDTO { get; set; }

        private readonly AccommodationRepository _accommodationRepository;

        private OwnerMainWindow _ownerMainWindow;
        private UserDTO _loggedInOwner;

        public AccommodationsPage(OwnerMainWindow ownerMainWindow)
        {
            InitializeComponent();
            _accommodationRepository = new AccommodationRepository();
            AccommodationsDTO = new ObservableCollection<AccommodationDTO>();

            DataContext = this;
            _ownerMainWindow = ownerMainWindow;
            
            _loggedInOwner = _ownerMainWindow.LoggedInOwner;

            _ownerMainWindow.Update();

            AccommodationsDTO = OwnerMainWindow.AccommodationsDTO;
        }

        private void ShowSideMenu(object sender, RoutedEventArgs e)
        {
            _ownerMainWindow.ShowSideMenu(sender, e);
        }
        private void ShowAddAccomodationPage(object sender, RoutedEventArgs e)
        {
            _ownerMainWindow.ShowAddAccommodationPage(sender, e);
        }
    }
}
