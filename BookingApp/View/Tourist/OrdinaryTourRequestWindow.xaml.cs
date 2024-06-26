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
    /// Interaction logic for OrdinaryTourRequestWindow.xaml
    /// </summary>
    public partial class OrdinaryTourRequestWindow : Window
    {
        public static OrdinaryTourRequestViewModel _ordinaryTourRequestViewModel;
        public OrdinaryTourRequestWindow(UserDTO loggedInUser, int complexTourRequestId)
        {
            InitializeComponent();
            
            _ordinaryTourRequestViewModel = new OrdinaryTourRequestViewModel(loggedInUser, complexTourRequestId);
            DataContext = _ordinaryTourRequestViewModel;
            if (_ordinaryTourRequestViewModel.CloseAction == null)
                _ordinaryTourRequestViewModel.CloseAction = new Action(this.Close);
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

     
        

    }
}
