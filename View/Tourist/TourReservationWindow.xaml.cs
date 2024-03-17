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

        private readonly TourReservationRepository _tourReservationRepository;

        private readonly TourRepository _tourRepository;

        private TouristDTO _touristDTO;

        private TouristMainWindow _tourMainWindow;

        private UserDTO _userDTO;

        public int unlistedTouristsCounter;

        public int CurrentCapacity;

        private Brush _defaultBrushBorder;

        public bool isListFilled;

        public ObservableCollection<TouristDTO> Tourists { get; set; }
        public TourReservationWindow(TouristMainWindow tourMainWindow, TourReservationRepository tourReservationRepository,TourDTO tourDTO, UserDTO userDTO)
        {
            InitializeComponent();
            _defaultBrushBorder=textBoxCurrentCapacity.BorderBrush.Clone();
            textBoxNumber.Text = 0.ToString();
            _tourReservationRepository = tourReservationRepository;
            _tourRepository = new TourRepository();
            _tourDTO = new TourDTO(tourDTO);
            _userDTO = userDTO;
            _tourReservationDTO = new TourReservationDTO(tourDTO, _userDTO);
            _touristDTO=new TouristDTO();
            _tourMainWindow = tourMainWindow;
            Tourists = new ObservableCollection<TouristDTO>();
            DataContext = new { Tour = _tourDTO, User = _userDTO };
            dataGridTourists.ItemsSource = Tourists;
            bool isListFilled = false;
            buttonSubmitInfo.IsEnabled = false;
        }
       
        private void ConfirmReservation_Click(object sender, RoutedEventArgs e)
        {
            if(AreAllListed(unlistedTouristsCounter) || IsListAllreadyFilled(unlistedTouristsCounter))
            {
                UpdateTour(CurrentCapacity);

                _tourReservationDTO.TouristsDTO = Tourists.ToList();
                _tourReservationRepository.Save(_tourReservationDTO.ToTourReservation());

                _tourMainWindow.Update();

                MessageBox.Show("Successfully added reservation!");
                Close();
            }
            else
            {
                MessageBox.Show("You didn't add number of tourists that you previousli selected!");
            } 
        }
        private void UpdateTour(int currentCapacity)
        {
            _tourDTO.CurrentCapacity = currentCapacity;
            _tourRepository.Update(_tourDTO.ToTourWithCapacity());
        }

        public bool AreAllListed(int unlistedTouristCounter)
        {
            return unlistedTouristCounter == 0;
        }
        public bool IsListAllreadyFilled(int unlistedTouristCounter)
        {
            if(isListFilled)
                return unlistedTouristsCounter == Tourists.Count();
            else
                return false;
        }

        private void ConfirmTouristsNumber_Click(object sender, RoutedEventArgs e)
        {
            unlistedTouristsCounter = Int32.Parse(textBoxNumber.Text);

            CurrentCapacity = _tourDTO.CurrentCapacity - unlistedTouristsCounter;
            textBoxCurrentCapacity.Text = CurrentCapacity.ToString();

            unlistedTouristsCounter = AdditionalChecking(CurrentCapacity, unlistedTouristsCounter);

            MessageBox.Show("You added number of tourists!");
        }
        private int AdditionalChecking(int currentCapacity, int unlistedTouristCounter)
        {
            if (currentCapacity < 0)
            {
                MessageBox.Show("Number of tourists you added is out of the range!");
                buttonAdd.IsEnabled = false;
                buttonSubmitInfo.IsEnabled = false;
                return unlistedTouristCounter;
            }
            else if (Tourists.Count() > 0)
            {
                buttonSubmitInfo.IsEnabled = true;
                isListFilled = true;
                return UnemptyListAdditionalChecking(unlistedTouristCounter);

            }
            else
            {
                buttonAdd.IsEnabled = true;
                buttonSubmitInfo.IsEnabled = true;
                return unlistedTouristCounter;
            }
        }
        private int UnemptyListAdditionalChecking(int unlistedTouristCounter)
        {

            if (unlistedTouristCounter > Tourists.Count())
            {
                unlistedTouristCounter = unlistedTouristCounter - Tourists.Count();
                buttonAdd.IsEnabled = true;
                return unlistedTouristCounter;
            }
            else if(unlistedTouristCounter < Tourists.Count())
            {
                MessageBox.Show("Remove tourists from table!");
                buttonAdd.IsEnabled = false;
                return unlistedTouristCounter;
            }
            buttonAdd.IsEnabled = false;
            return unlistedTouristCounter;
        }

        private void SubmitUserInfo_Click(object sender, RoutedEventArgs e)
        {
            if (Tourists.Count() > 0)
            {
                _touristDTO = new TouristDTO(textBoxFirstName.Text, textBoxSurname.Text, Int32.Parse(textBoxAge.Text));
                Tourists[0] = _touristDTO;

            }
            else
            {
                _touristDTO = new TouristDTO(textBoxFirstName.Text, textBoxSurname.Text, Int32.Parse(textBoxAge.Text));
                Tourists.Add(_touristDTO);
                unlistedTouristsCounter = unlistedTouristsCounter - 1;

                if (AreAllListed(unlistedTouristsCounter))
                    buttonAdd.IsEnabled = false;
            }
        }

        private void ShowAnonymousTouristWindow(object sender, RoutedEventArgs e)
        {
            AnonymousTouristWindow anonymousTouristWindow = new AnonymousTouristWindow(this, _tourReservationDTO, Tourists, unlistedTouristsCounter);
            anonymousTouristWindow.Show();
        }

        private void CancelReservation_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void RemoveTourist_Click(object sender, RoutedEventArgs e)
        {
            _touristDTO = dataGridTourists.SelectedItem as TouristDTO;
            Tourists.Remove(_touristDTO);

            if (unlistedTouristsCounter > Tourists.Count())
            {
               if((unlistedTouristsCounter - Tourists.Count())!=0)
               {
                    unlistedTouristsCounter--;
                    buttonAdd.IsEnabled = true;
               }
               else
               {
                    buttonAdd.IsEnabled = false;
               }
            }  
            else
                buttonAdd.IsEnabled = false; 
        }
        
        private void textBox_TextChanged(object sender, EventArgs e)
        {
            if(CheckInput())
                buttonConfirm.IsEnabled = true;
            else
                buttonConfirm.IsEnabled = false;
        }
        private bool EmptyTextBoxCheck()
        {
            bool validInput = true;

            foreach(var control in gridMain.Children)
            {
                if(control is TextBox)
                { 
                    TextBox textBox = (TextBox)control;
                    if(textBox.Text==String.Empty)
                    {
                        BorderBrushToRed(textBox);
                        validInput = false;
                    }
                    else
                    {
                        BorderBrushToDefault(textBox);
                    }
                }
            }

            return validInput;
        }
        private bool CheckInput()
        {
            bool validInput = EmptyTextBoxCheck();
            
            if(!int.TryParse(textBoxNumber.Text,out int number))
            {
                BorderBrushToRed(textBoxNumber);
                validInput = false;
                buttonOK.IsEnabled = false;
            }
            else
            {
                if(int.Parse(textBoxNumber.Text) < 1)
                {
                    BorderBrushToRed(textBoxNumber);
                    validInput = false;
                    buttonOK.IsEnabled = false;
                }
                else
                {
                    BorderBrushToDefault(textBoxNumber);
                    buttonOK.IsEnabled = true;
                }
            }

            if (!int.TryParse(textBoxAge.Text, out int age))
            {
                BorderBrushToRed(textBoxAge);
                validInput = false;
                buttonSubmitInfo.IsEnabled = false;
            }
            else
            {
                if (int.Parse(textBoxAge.Text) < 1)
                {
                    BorderBrushToRed(textBoxAge);
                    validInput = false;
                    buttonSubmitInfo.IsEnabled = false;
                }
                else
                {
                    BorderBrushToDefault(textBoxNumber);
                    buttonSubmitInfo.IsEnabled = true;
                }
            }
            return validInput;

        }
        private void BorderBrushToRed(TextBox textBox)
        {
            textBox.BorderBrush = Brushes.HotPink;
            textBox.BorderThickness = new Thickness(2);
        }
        private void BorderBrushToDefault(TextBox textBox)
        {
            textBox.BorderBrush = _defaultBrushBorder;
            textBox.BorderThickness = new Thickness(2);
        }
    }
}
