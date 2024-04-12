using BookingApp.DTO;
using BookingApp.Model.Enums;
using BookingApp.Repository;
using BookingApp.ViewModel.Owner;
using Microsoft.Win32;
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
    /// Interaction logic for AddAccommodationPage.xaml
    /// </summary>
    public partial class AddAccommodationPage : Page
    {
        private Brush _defaultBrushBorder;
        private AddAccommodationViewModel _addAccommodationViewModel;

        public AddAccommodationPage(UserDTO loggedInOwner)
        {
            InitializeComponent();

            _addAccommodationViewModel = new AddAccommodationViewModel(loggedInOwner);
            DataContext = _addAccommodationViewModel;
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            if(CheckInput())
                buttonAdd.IsEnabled = true;
            else
                buttonAdd.IsEnabled = false;
        }
        private bool EmptyTextBoxCheck()
        {
            bool validInput = true;

            foreach(var grid in stackPanel.Children.OfType<Grid>())
            {
                foreach(var control in grid.Children)
                {
                    if(control is TextBox)
                    {
                        TextBox textBox = (TextBox)control;
                        if (textBox.Text == string.Empty)
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
            }

            return validInput;
        }
        private bool CheckInput()
        {
            bool validInput = EmptyTextBoxCheck();

            if(!int.TryParse(textBoxCapacity.Text, out int capacity))
            {
                BorderBrushToRed(textBoxCapacity);
                validInput = false;
            }
            else
            {
                if(int.Parse(textBoxCapacity.Text) < 1)
                {
                    BorderBrushToRed(textBoxCapacity);
                    validInput = false;
                }
                else
                {
                    BorderBrushToDefault(textBoxCapacity);
                }
            }

            if (!int.TryParse(textBoxMinDaysReservation.Text, out int minDaysReservation))
            {
                BorderBrushToRed(textBoxMinDaysReservation);
                validInput = false;
            }
            else
            {
                if (int.Parse(textBoxMinDaysReservation.Text) < 1)
                {
                    BorderBrushToRed(textBoxMinDaysReservation);
                    validInput = false;
                }
                else
                {
                    BorderBrushToDefault(textBoxMinDaysReservation);
                }
            }

            if (!int.TryParse(textBoxCancellationPeriod.Text, out int cancellationPeriod))
            {
                BorderBrushToRed(textBoxCancellationPeriod);
                validInput = false;
            }
            else
            {
                if (int.Parse(textBoxCancellationPeriod.Text) < 0)
                {
                    BorderBrushToRed(textBoxCancellationPeriod);
                    validInput = false;
                }
                else
                {
                    BorderBrushToDefault(textBoxCancellationPeriod);
                }
            }

            return validInput;
        }
        private void BorderBrushToRed(TextBox textBox)
        {
            textBox.BorderBrush = Brushes.Red;
            textBox.BorderThickness = new Thickness(2);
        }
        private void BorderBrushToDefault(TextBox textBox)
        {
            textBox.BorderBrush = _defaultBrushBorder;
            textBox.BorderThickness = new Thickness(2);
        }
    }
}
