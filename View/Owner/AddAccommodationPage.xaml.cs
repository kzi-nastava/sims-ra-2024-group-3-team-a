using BookingApp.DTO;
using BookingApp.Model.Enums;
using BookingApp.Repository;
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
        private AccommodationRepository _repository;
        private AccommodationDTO _accommodationDTO;
        private OwnerMainWindow _ownerMainWindow;

        private List<string> images;

        private Brush _defaultBrushBorder;

        public AddAccommodationPage(OwnerMainWindow ownerMainWindow)
        {
            InitializeComponent();
            _defaultBrushBorder = textBoxName.BorderBrush.Clone();
            comboBoxType.SelectedItem = comboBoxItemApartment;
            
            _repository = new AccommodationRepository();
            _accommodationDTO = new AccommodationDTO();
            _ownerMainWindow = ownerMainWindow;

            _accommodationDTO.CancellationPeriod = 1;

            DataContext = _accommodationDTO;
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            if (comboBoxType.SelectedItem == comboBoxItemApartment)
                _accommodationDTO.Type = AccomodationType.Apartment;
            else if (comboBoxType.SelectedItem == comboBoxItemHouse)
                _accommodationDTO.Type = AccomodationType.House;
            else
                _accommodationDTO.Type = AccomodationType.Cottage;

            _accommodationDTO.Images = images;

            _repository.Save(_accommodationDTO.ToAccommodation());

            setDefaultValues();
            _ownerMainWindow.Update();
        }

        private void setDefaultValues()
        {
            _accommodationDTO.Name = string.Empty;
            _accommodationDTO.Type = AccomodationType.Apartment;
            _accommodationDTO.Capacity = 0;
            _accommodationDTO.MinDaysReservation = 0;
            _accommodationDTO.CancellationPeriod = 1;
            _accommodationDTO.PlaceDTO.Country = string.Empty;
            _accommodationDTO.PlaceDTO.City = string.Empty;
            if(_accommodationDTO.Images != null)
                _accommodationDTO.Images.Clear();
        }

        private void AddImages(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;

            bool? response = openFileDialog.ShowDialog();

            if (response == true)
            {
                images = openFileDialog.FileNames.ToList();

                for (int i = 0; i < images.Count; i++)
                {
                    images[i] = System.IO.Path.GetRelativePath(AppDomain.CurrentDomain.BaseDirectory, images[i]).ToString();
                }
            }
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            if(InputCheck())
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
                            textBox.BorderBrush = Brushes.Red;
                            textBox.BorderThickness = new Thickness(2);
                            validInput = false;
                        }
                        else
                        {
                            textBox.BorderBrush = _defaultBrushBorder;
                            textBox.BorderThickness = new Thickness(2);
                        }
                    } 
                }
            }

            return validInput;
        }
        private bool InputCheck()
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
