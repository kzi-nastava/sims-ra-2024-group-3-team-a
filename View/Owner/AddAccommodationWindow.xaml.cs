﻿using BookingApp.DTO;
using BookingApp.Model.Enums;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace BookingApp.View.Owner
{
    /// <summary>
    /// Interaction logic for AddAccommodationWindow.xaml
    /// </summary>
    public partial class AddAccommodationWindow : Window
    {
        private AccommodationRepository _repository;
        private AccommodationDTO _accommodationDTO;

        public event EventHandler AccommodationAdded;

        public AddAccommodationWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

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

            _repository.Save(_accommodationDTO.ToAccommodation());

            AccommodationAdded?.Invoke(this, EventArgs.Empty);
            Close();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
