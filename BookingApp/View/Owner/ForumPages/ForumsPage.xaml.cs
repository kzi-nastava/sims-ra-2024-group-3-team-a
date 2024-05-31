using BookingApp.DTO;
using BookingApp.ViewModel.Owner.AnswerRequestViewModels;
using BookingApp.ViewModel.Owner.ForumViewModels;
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

namespace BookingApp.View.Owner.ForumPages
{
    public partial class ForumsPage : Page
    {
        private ForumsViewModel _forumsViewModel;
        public ForumsPage(UserDTO loggedInUser)
        {
            InitializeComponent();

            _forumsViewModel = new ForumsViewModel(loggedInUser);
            DataContext = _forumsViewModel;
        }
    }
}
