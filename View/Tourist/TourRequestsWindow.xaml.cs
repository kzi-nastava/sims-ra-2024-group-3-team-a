﻿using BookingApp.DTO;
using BookingApp.ViewModel.Tourist;
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
using System.Windows.Shapes;

namespace BookingApp.View.Tourist
{
    /// <summary>
    /// Interaction logic for TourRequestsWindow.xaml
    /// </summary>
    public partial class TourRequestsWindow : Window
    {
        private TourRequestsViewModel _tourRequestsViewModel;
        public TourRequestsWindow(UserDTO loggedInUser)
        {

            _tourRequestsViewModel = new TourRequestsViewModel(loggedInUser);
            InitializeComponent();
            DataContext = _tourRequestsViewModel;

        }
    }
}