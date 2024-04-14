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
        private TourReservationWindow _tourReservationWindow;

        private int _unlistedTouristsCounter;

        private Brush _defaultBrushBorder;

        public AnonymousTouristWindow (TourReservationWindow tourReservationWindow, TourReservationDTO tourReservationDTO, ObservableCollection<TouristDTO> anonymousTourists, int touristCounter)
        {
            InitializeComponent();
            DataContext = this;

            _unlistedTouristsCounter = touristCounter;

            _defaultBrushBorder = textBoxName.BorderBrush.Clone();
            textBoxAge.Text = 0.ToString();

            _tourReservationWindow = tourReservationWindow;  
        }

        public void AddToReservationList_Click(object sender, RoutedEventArgs e)
        {
           TouristDTO touristDTO = new TouristDTO(textBoxName.Text,textBoxSurname.Text, Int32.Parse(textBoxAge.Text));
           _tourReservationWindow.Tourists.Add(touristDTO);

           DecreasingUnlistedTouristsNumber(_unlistedTouristsCounter);

           Close();
        }
        private void DecreasingUnlistedTouristsNumber(int number)
        {
            number = number - 1;
            _tourReservationWindow.unlistedTouristsCounter = number;

           /* if (_tourReservationWindow.AreAllListed(number))
            {
                _tourReservationWindow.buttonAdd.IsEnabled = false;
            }*/
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Cancelled adding of tourist!");
            Close();
        }
        private void textBox_TextChanged(object sender, EventArgs e)
        {
            if (CheckInput())
                buttonSubmit.IsEnabled = true;
            else
                buttonSubmit.IsEnabled = false;

        }
        private bool EmptyTextBoxCheck()
        {
            bool validInput = true;

            foreach (var control in gridMain.Children)
            {
                if (control is TextBox)
                {
                    TextBox textBox = (TextBox)control;
                    if (textBox.Text == String.Empty)
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

            if (!int.TryParse(textBoxAge.Text, out int age))
            {
                BorderBrushToRed(textBoxAge);
                validInput = false;
                
            }
            else
            {
                if (int.Parse(textBoxAge.Text) < 1)
                {
                    BorderBrushToRed(textBoxAge);
                    validInput = false;
                   
                }
                else
                {
                    BorderBrushToDefault(textBoxAge);
                   
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
