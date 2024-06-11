using BookingApp.DTO;
using BookingApp.Repository;
using BookingApp.ViewModel.Owner.AnswerRequestViewModels;
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

namespace BookingApp.View.Owner.AnswerRequestPages
{
    /// <summary>
    /// Interaction logic for RequestDetailsPage.xaml
    /// </summary>
    public partial class RequestDetailsPage : Page
    {
        private RequestDetailsViewModel _requestDetailsViewModel;

        public RequestDetailsPage(MessageDTO messageDTO)
        {
            InitializeComponent();
            
            _requestDetailsViewModel = new RequestDetailsViewModel(messageDTO);
            DataContext = _requestDetailsViewModel;
        }
    }
}
