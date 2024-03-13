using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.View.Tourist;
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
        private TourReservationDTO _tourReservationDTO;
        private TourDTO _tourDTO;
        private TourReservationRepository _tourReservationRepository;
        private TourRepository _tourRepository;
        private AnonymousTouristDTO _anonymousTouristDTO;
        private UserDTO _userDTO;
        public int numberOfTourists;
        public int CurrentCapacity;

        public ObservableCollection<AnonymousTouristDTO> AnonymousTourists { get; set; }
        public TourReservationWindow(TourReservationRepository tourReservationRepository,TourDTO tourDTO, UserDTO userDTO)
        {
            InitializeComponent();
            _tourReservationRepository = tourReservationRepository;
            _tourRepository = new TourRepository();
            _tourDTO = new TourDTO(tourDTO);
            _userDTO = userDTO;
            _tourReservationDTO = new TourReservationDTO(tourDTO, _userDTO);
            _anonymousTouristDTO=new AnonymousTouristDTO();
            AnonymousTourists= new ObservableCollection<AnonymousTouristDTO>();
            DataContext = new { Tour = _tourDTO, User = _userDTO };
            dataGridAnonymousTourists.ItemsSource = AnonymousTourists;

        }
       

        private void ConfirmReservation_Click(object sender, RoutedEventArgs e)
        {

            _tourReservationDTO.AnonymousTouristDTOs = AnonymousTourists.ToList();
            _tourDTO.CurrentCapacity = CurrentCapacity;
            _tourRepository.Update(_tourDTO.ToTourWithCapacity());


           _tourReservationRepository.Save(_tourReservationDTO.ToTourReservation());
            Close();

        }

        private void AddAnonymousTourist_Click(object sender, RoutedEventArgs e )
        {
            numberOfTourists = numberOfTourists - 1;
            AnonymousTouristWindow anonymousTouristWindow = new AnonymousTouristWindow (this,_tourReservationDTO, AnonymousTourists, numberOfTourists);
            if (numberOfTourists==0)
            {
                textBoxAdd.IsEnabled = false;
               
            }
            anonymousTouristWindow.Show();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if(textBoxNumber.Text.ToString()!="")
            {
                numberOfTourists = Int32.Parse(textBoxNumber.Text);
                numberOfTourists = numberOfTourists - AnonymousTourists.Count();
                CurrentCapacity = _tourDTO.CurrentCapacity-numberOfTourists;
                textBoxCurrentCapacity.Text = CurrentCapacity.ToString();

                if (CurrentCapacity < 0) 
                {
                    MessageBox.Show("Nuber of tourists you added is out of the range!");
                    textBoxAdd.IsEnabled = false;
                }
                else
                {
                    textBoxAdd.IsEnabled = true;
                    MessageBox.Show("You added number of tourists!");
                }
                
            }
            else
            {
                MessageBox.Show("You didn't input number!");
            }
           
        }
        private void SubmitUserInfo_Click(object sender, RoutedEventArgs e)
        {
            _anonymousTouristDTO = new AnonymousTouristDTO(textBoxFirstName.Text, textBoxSurname.Text, Int32.Parse(textBoxAge.Text));
            AnonymousTourists.Add(_anonymousTouristDTO);
            numberOfTourists = numberOfTourists - 1;
        }
    }
   
}
