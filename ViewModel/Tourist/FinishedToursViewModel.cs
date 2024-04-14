using BookingApp.DTO;
using BookingApp.Service;
using BookingApp.View.Tourist;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Tourist
{
    public class FinishedToursViewModel : ViewModel
    {

        private TourService _tourService { get; set; }

        private ObservableCollection<TourDTO> _finishedTourDTO;

        private TourDTO _selectedTourDTO = null;

        private UserDTO _userDTO;

        public FinishedToursViewModel(UserDTO loggedInUser)
        {

            _tourService = new TourService();
            _userDTO = loggedInUser;
            List<TourDTO> finishedTours = _tourService.GetFinishedTours().Select(finishedTours => new TourDTO(finishedTours)).ToList();
            _finishedTourDTO = new ObservableCollection<TourDTO>(finishedTours);
          
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
                ShowTourReviewWindow();
            }
        }

        public void ShowTourReviewWindow()
        {

            if (_selectedTourDTO == null)
            {
                return;
            }

            var selectedItem = _selectedTourDTO as TourDTO;
            TourReviewWindow tourReviewWindow = new TourReviewWindow(new TourDTO(selectedItem), _userDTO);
            tourReviewWindow.ShowDialog();
        }
    }
}
