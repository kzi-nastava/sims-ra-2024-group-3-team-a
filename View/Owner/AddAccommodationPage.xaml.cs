﻿using BookingApp.DTO;
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

        private List<string> images;

        public event EventHandler AccommodationAdded;

        public AddAccommodationPage()
        {
            InitializeComponent();
            
            _repository = new AccommodationRepository();
            _accommodationDTO = new AccommodationDTO();

            DataContext = _accommodationDTO;
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            if (comboBoxStatus.SelectedItem == comboBoxItemApartment)
                _accommodationDTO.Type = AccomodationType.Apartment;
            else if (comboBoxStatus.SelectedItem == comboBoxItemHouse)
                _accommodationDTO.Type = AccomodationType.House;
            else
                _accommodationDTO.Type = AccomodationType.Cottage;

            _accommodationDTO.Images = images;

            _repository.Save(_accommodationDTO.ToAccommodation());

            AccommodationAdded?.Invoke(this, EventArgs.Empty);
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

        }
    }
}
