using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.Service;
using BookingApp.View.Tourist;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.ViewModel.Tourist
{
    public class FinishedToursViewModel : ViewModel
    {

        private TourService _tourService { get; set; }
        private TourReviewService _tourReviewService { get; set; }

        private ObservableCollection<TourDTO> _finishedTourDTO;

        private TourDTO _selectedTourDTO = null;

        private UserDTO _userDTO;

        private RelayCommand _showTourReviewWindowCommand;

        public FinishedToursViewModel(UserDTO loggedInUser)
        {

            _tourService = new TourService();
            _tourReviewService = new TourReviewService();
            _userDTO = loggedInUser;
            List<TourDTO> finishedTours = _tourService.GetFinishedTours().Select(finishedTours => new TourDTO(finishedTours)).ToList();
            _finishedTourDTO = new ObservableCollection<TourDTO>(finishedTours);
            _showTourReviewWindowCommand = new RelayCommand(ShowTourReviewWindow);
        }

        public ObservableCollection<TourDTO> FinishedToursDTO
        {
            get
            {
                return _finishedTourDTO;
            }

            set
            {
                _finishedTourDTO = value;
                OnPropertyChanged();

            }
        }

        public TourDTO SelectedTourDTO
        {
            get
            {
                return _selectedTourDTO;
            }
            set
            {
                _selectedTourDTO = value;
                OnPropertyChanged();
              
            }
        }

        public RelayCommand ShowTourReviewWindowCommand
        {
            get
            {
                return _showTourReviewWindowCommand;
            }
            set
            {
                _showTourReviewWindowCommand = value;
                OnPropertyChanged();
            }
        }

        public void ShowTourReviewWindow()
        {

            if (_selectedTourDTO == null)
            {
                return;
            }

            var selectedItem = _selectedTourDTO as TourDTO;

            if(_tourReviewService.IsTourRated(selectedItem.ToTourAllParam(), _userDTO.ToUser()))
            {
                MessageBox.Show("This tour is allready rated!");
            }
            else
            {
                TourReviewWindow tourReviewWindow = new TourReviewWindow(new TourDTO(selectedItem), _userDTO);
                tourReviewWindow.ShowDialog();
            }
            
        }
    }
}
