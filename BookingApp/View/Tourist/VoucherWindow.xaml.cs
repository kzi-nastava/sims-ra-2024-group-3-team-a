using BookingApp.DTO;
using BookingApp.ViewModel.Owner;
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
    /// Interaction logic for VoucherWindow.xaml
    /// </summary>
    public partial class VoucherWindow : Window
    {
        private VoucherViewModel _voucherViewModel;
        public VoucherWindow(UserDTO loggedInUser)
        {
            InitializeComponent();

            _voucherViewModel = new VoucherViewModel(loggedInUser);

            DataContext = _voucherViewModel;

            //voucherListView.Focus();
        }
    }
}
