using BookingApp.DTO;
using BookingApp.Repository;
using BookingApp.ViewModel.Owner;
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
    /// Interaction logic for NewReviewDetailsPage.xaml
    /// </summary>
    public partial class NewReviewDetailsPage : Page
    {
        private NewMessageDetails _newReviewDetailsViewModel;

        public NewReviewDetailsPage(MessageDTO messageDTO)
        {
            InitializeComponent();

            _newReviewDetailsViewModel = new NewMessageDetails(messageDTO);
            DataContext = _newReviewDetailsViewModel;
        }
    }
}
