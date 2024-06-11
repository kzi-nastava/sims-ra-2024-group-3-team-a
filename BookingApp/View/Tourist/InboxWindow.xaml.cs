using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Model.Enums;
using BookingApp.Repository;
using BookingApp.ViewModel.Tourist;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for InboxWindow.xaml
    /// </summary>
    public partial class InboxWindow : Window
    {
        private InboxViewModel _inboxViewModel;
        public InboxWindow(UserDTO loggedInUser)
        {
            InitializeComponent();

            _inboxViewModel = new InboxViewModel(loggedInUser);

            DataContext = _inboxViewModel;
            inboxListView.Focus();
            if (_inboxViewModel.CloseAction == null)
                _inboxViewModel.CloseAction = new Action(this.Close);
        }
    }
}
