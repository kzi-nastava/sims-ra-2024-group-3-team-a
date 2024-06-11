using BookingApp.DTO;
using BookingApp.Model.Enums;
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
    /// Interaction logic for RejectedMessagePage.xaml
    /// </summary>
    public partial class RejectedMessagePage : Page
    {
        private RejectedMessageViewModel _rejectedMessageViewModel;

        public RejectedMessagePage(AccommodationReservationChangeRequestDTO accommodationReservationChangeRequestDTO, MessageDTO messageDTO)
        {
            InitializeComponent();
            _rejectedMessageViewModel = new RejectedMessageViewModel(accommodationReservationChangeRequestDTO, messageDTO);
            DataContext = _rejectedMessageViewModel;
        }
    }
}
