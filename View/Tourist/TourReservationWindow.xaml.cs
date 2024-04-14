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
      
        private Brush _defaultBrushBorder;

        private TourReservationViewModel _tourReservationViewModel;
        public TourReservationWindow(TouristMainWindow tourMainWindow, TourReservationService tourReservationService,TourDTO tourDTO, UserDTO userDTO)
        {
            InitializeComponent();
            _defaultBrushBorder=textBoxCurrentCapacity.BorderBrush.Clone();
            textBoxNumber.Text = 0.ToString();
             
            _tourReservationViewModel = new TourReservationViewModel(tourReservationService, tourDTO, userDTO);
            DataContext = _tourReservationViewModel;
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
