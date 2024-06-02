using BookingApp.DTO;
using BookingApp.ViewModel.Owner.WizardAndHelp;
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

namespace BookingApp.View.Owner.WizardAndHelp
{
    /// <summary>
    /// Interaction logic for ReviewHelpPage.xaml
    /// </summary>
    public partial class ReviewHelpPage : Page
    {
        private ReviewHelpViewModel _reviewHelpViewModel;
        public ReviewHelpPage(UserDTO loggedInUser)
        {
            InitializeComponent();

            _reviewHelpViewModel = new ReviewHelpViewModel(loggedInUser);
            DataContext = _reviewHelpViewModel;

        }
    }
}
