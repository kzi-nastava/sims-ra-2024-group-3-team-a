using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.ViewModel.Owner.AccommodationRenovationViewModels;
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

namespace BookingApp.View.Owner.AccommodationRenovationPages
{
    /// <summary>
    /// Interaction logic for AccommodationRenovationDetailsPage.xaml
    /// </summary>
    public partial class AccommodationRenovationDetailsPage : Page
    {
        AccommodationRenovationDetailsViewModel _accommodationRenovationDetailsViewModel;
        public AccommodationRenovationDetailsPage(AccommodationRenovationDTO accommodationRenovationDTO)
        {
            InitializeComponent();

            _accommodationRenovationDetailsViewModel = new AccommodationRenovationDetailsViewModel(accommodationRenovationDTO);
            DataContext = _accommodationRenovationDetailsViewModel;
        }
    }
}
