using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Service;
using BookingApp.View.Tourist;
using BookingApp.ViewModel.Tourist;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for TourReservation.xaml
    /// </summary>
    public partial class TourReservationWindow : Window
    { 
      
        

        private TourReservationViewModel _tourReservationViewModel;
        public static TourReservationWindow Instance;

       
        public TourReservationWindow(TourReservationService tourReservationService,TourDTO tourDTO, UserDTO userDTO)
        {
            InitializeComponent();
         
             
            _tourReservationViewModel = new TourReservationViewModel(tourReservationService, tourDTO, userDTO);
            DataContext = _tourReservationViewModel;
          
            if (Instance == null)
            {
                Instance = this;
            }
            if (_tourReservationViewModel.CloseAction == null)
                _tourReservationViewModel.CloseAction = new Action(this.Close);
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
           

        }
        public static TourReservationWindow GetInstance()
        {
            return Instance;
        }

       
    }
}
