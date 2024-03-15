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

        private AnonymousTouristDTO _anonymousTouristDTO;

        private TouristMainWindow _tourMainWindow;

        private UserDTO _userDTO;

        public int unlistedTouristsCounter;

        public int CurrentCapacity;

        private Brush _defaultBrushBorder;

        public ObservableCollection<AnonymousTouristDTO> AnonymousTourists { get; set; }
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
            _anonymousTouristDTO=new AnonymousTouristDTO();
            _tourMainWindow = tourMainWindow;
            AnonymousTourists = new ObservableCollection<AnonymousTouristDTO>();
            DataContext = new { Tour = _tourDTO, User = _userDTO };
            dataGridAnonymousTourists.ItemsSource = AnonymousTourists;
            buttonSubmitInfo.IsEnabled = false;
        }
       

        private void ConfirmReservation_Click(object sender, RoutedEventArgs e)
        {
            if(AreAllListed(unlistedTouristsCounter))
            {
                UpdateTour(CurrentCapacity);

                _tourReservationDTO.AnonymousTouristDTOs = AnonymousTourists.ToList();
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

        private void AddAnonymousTourist_Click(object sender, RoutedEventArgs e )
        {
            AnonymousTouristWindow anonymousTouristWindow = new AnonymousTouristWindow(this, _tourReservationDTO, AnonymousTourists, unlistedTouristsCounter);
            anonymousTouristWindow.Show();
        }

        private void ConfirmTouristsNumber_Click(object sender, RoutedEventArgs e)
        {
                unlistedTouristsCounter = Int32.Parse(textBoxNumber.Text);

                CurrentCapacity = _tourDTO.CurrentCapacity - unlistedTouristsCounter;
                textBoxCurrentCapacity.Text = CurrentCapacity.ToString();

                buttonSubmitInfo.IsEnabled = true;

                unlistedTouristsCounter = AdditionalChecking(CurrentCapacity, unlistedTouristsCounter);

                MessageBox.Show("You added number of tourists!");
        }
            

        
        private void SubmitUserInfo_Click(object sender, RoutedEventArgs e)
        {
           
           
            if(AnonymousTourists.Count() > 0)
            {
                _anonymousTouristDTO = new AnonymousTouristDTO(textBoxFirstName.Text, textBoxSurname.Text, Int32.Parse(textBoxAge.Text));
                AnonymousTourists[0] = _anonymousTouristDTO;

            }
           else
           {
                _anonymousTouristDTO = new AnonymousTouristDTO(textBoxFirstName.Text, textBoxSurname.Text, Int32.Parse(textBoxAge.Text));
                AnonymousTourists.Add(_anonymousTouristDTO);
                unlistedTouristsCounter = unlistedTouristsCounter - 1;

                if(AreAllListed(unlistedTouristsCounter))
                buttonAdd.IsEnabled = false;
           }
            
        }
        private void CancelReservation_Click(object sender, RoutedEventArgs e)
        {
            Close();
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
        private int AdditionalChecking(int currentCapacity, int  unlistedTouristCounter)
        {
            if (currentCapacity < 0)
            {
                MessageBox.Show("Number of tourists you added is out of the range!");
                buttonAdd.IsEnabled = false;
               
            }
            else if (AnonymousTourists.Count() >= 0)
            {
                if (unlistedTouristCounter > AnonymousTourists.Count())
                {
                    unlistedTouristCounter = unlistedTouristCounter - AnonymousTourists.Count();
                    buttonAdd.IsEnabled = true;
                    return unlistedTouristCounter;
                }
                else
                {
                    MessageBox.Show("Remove tourists from table!");
                    buttonAdd.IsEnabled = false;
                    return unlistedTouristCounter;
                }
            }

            if(AreAllListed(unlistedTouristCounter))
                buttonAdd.IsEnabled = false;
            else
                buttonAdd.IsEnabled = true;

            return unlistedTouristCounter;
        }

        private void RemoveTourist_Click(object sender, RoutedEventArgs e)
        {
            _anonymousTouristDTO = dataGridAnonymousTourists.SelectedItem as AnonymousTouristDTO;
            AnonymousTourists.Remove(_anonymousTouristDTO);

            //unlistedTouristsCounter = unlistedTouristsCounter + 1;

            if(unlistedTouristsCounter > AnonymousTourists.Count()) 
                buttonAdd.IsEnabled = true;
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
