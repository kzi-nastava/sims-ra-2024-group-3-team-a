using BookingApp.DTO;
using BookingApp.Model;
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
    /// Interaction logic for OrdinaryTourRequestInfoWindow.xaml
    /// </summary>
    public partial class OrdinaryTourRequestInfoWindow : Window
    {
        private OrdinaryTourRequestInfoViewModel _ordinaryTourRequestInfoViewModel;
        public OrdinaryTourRequestInfoWindow(OrdinaryTourRequestDTO ordinaryTourRequestDTO)
        {
            InitializeComponent();
            _ordinaryTourRequestInfoViewModel = new OrdinaryTourRequestInfoViewModel(ordinaryTourRequestDTO);
            DataContext = _ordinaryTourRequestInfoViewModel;
        }
    }
}
