using BookingApp.DTO;
using BookingApp.Model;
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
using System.Windows.Shapes;

namespace BookingApp.View.Tourist
{
    /// <summary>
    /// Interaction logic for AnonymousTouristWindow.xaml
    /// </summary>
    public partial class AnonymousTouristWindow : Window
    {
        private static TourReservationDTO _tourReservationDTO;
        private static AnonymousTouristDTO _anonymousTouristDTO;
        private static TourRepository _tourRepository;
        private static List<AnonymousTouristDTO> _anonymousTouristsDTO;
        private static AnonymousTouristRepository _anonymousTouristRepository;
        private TourReservationWindow _tourReservationWindow;
        private int _numberOfTourists;
        ObservableCollection<AnonymousTouristDTO> anonymousTouristsObserver;

        public AnonymousTouristWindow (TourReservationWindow tourReservationWindow, TourReservationDTO tourReservationDTO, ObservableCollection<AnonymousTouristDTO> anonymousTourists, int numberOfTourists)
        {
            InitializeComponent();
            _tourReservationDTO = tourReservationDTO;
            _anonymousTouristRepository=new AnonymousTouristRepository();
            _anonymousTouristDTO= new AnonymousTouristDTO();
           _anonymousTouristsDTO = new List<AnonymousTouristDTO>();
            anonymousTouristsObserver = anonymousTourists;
            _tourReservationWindow = tourReservationWindow;
            _numberOfTourists = numberOfTourists;
           DataContext = this;
        }

        public void Submit_Click(object sender, RoutedEventArgs e)
        {
           AnonymousTouristDTO anonymousTouristDTO = new AnonymousTouristDTO(textBoxName.Text,textBoxSurname.Text, Int32.Parse(textBoxAge.Text) );
           _tourReservationWindow.AnonymousTourists.Add(anonymousTouristDTO);
           Close();
           

        }
    }
}
