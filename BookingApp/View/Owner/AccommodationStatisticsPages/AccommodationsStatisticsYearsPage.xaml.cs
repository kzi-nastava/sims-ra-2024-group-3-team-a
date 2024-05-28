using BookingApp.DTO;
using BookingApp.ViewModel.Owner.AccommodationStatisticsViewModels;
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
    /// Interaction logic for AccommodationsStatisticsYearsView.xaml
    /// </summary>
    /// 



    public partial class AccommodationsStatisticsYearsPage : Page
    {
        private AccommodationStatisticsYearViewModel _accommodationStatisticsYearViewModel;

        public AccommodationsStatisticsYearsPage(AccommodationDTO accommodationDTO)
        {
            InitializeComponent();

            _accommodationStatisticsYearViewModel = new AccommodationStatisticsYearViewModel(accommodationDTO);
            DataContext = _accommodationStatisticsYearViewModel;
        }
    }
}
