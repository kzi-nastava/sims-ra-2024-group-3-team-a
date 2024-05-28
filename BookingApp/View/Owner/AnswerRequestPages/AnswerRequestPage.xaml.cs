using BookingApp.DTO;
using BookingApp.Model.Enums;
using BookingApp.Repository;
using BookingApp.ViewModel.Owner.AnswerRequestViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.View.Owner.AnswerRequestPages
{
    /// <summary>
    /// Interaction logic for AnswerRequestPage.xaml
    /// </summary>
    public partial class AnswerRequestPage : Page
    {
        private AnswerRequestViewModel _answerRequestViewModel;

        public AnswerRequestPage(MessageDTO messageDTO)
        {
            InitializeComponent();

            _answerRequestViewModel = new AnswerRequestViewModel(messageDTO);
            DataContext = _answerRequestViewModel;

            foreach (var date in _answerRequestViewModel.OccupiedDates)
            {
                calendarOccupiedDays.BlackoutDates.Add(new CalendarDateRange(date, date));
            }
        }  
    }
}
