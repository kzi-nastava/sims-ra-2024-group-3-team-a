using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Service;
using BookingApp.ViewModel.Tourist;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for FinishedToursWindow.xaml
    /// </summary>
    public partial class FinishedToursWindow : Window
    {
        private FinishedToursViewModel _finishedToursViewModel { get; set; }
        public FinishedToursWindow(UserDTO userDTO)
        {
            InitializeComponent();
            _finishedToursViewModel = new FinishedToursViewModel(userDTO);

            DataContext = _finishedToursViewModel;
            finishedToursListView.Focus();
        }
    }
}
