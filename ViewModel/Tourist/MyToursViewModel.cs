using BookingApp.DTO;
using BookingApp.Model;
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
    public class MyToursViewModel: ViewModel
    {
        private TourService _tourService {  get; set; }

        private ObservableCollection<TourDTO> _activeTourDTO;

        private ObservableCollection<TourDTO> _unactiveTourDTO;

        private TourDTO _selectedTourDTO = null;

        public MyToursViewModel()
        {

            _tourService = new TourService();
            List<TourDTO> activeTours = _tourService.GetActiveTours().Select(activeTours => new TourDTO(activeTours)).ToList();
            List<TourDTO> unactiveTours = _tourService.GetUnactiveTours().Select(unactiveTours => new TourDTO(unactiveTours)).ToList();
            _activeTourDTO = new ObservableCollection<TourDTO>(activeTours);
            _unactiveTourDTO = new ObservableCollection<TourDTO>(unactiveTours);
        }
        public ObservableCollection<TourDTO> ActiveToursDTO
        {
            get 
            {
                return _activeTourDTO;
            }

            set
            {
                _activeTourDTO = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TourDTO> UnactiveToursDTO
        {
            get
            {
                return _unactiveTourDTO;
            }

            set
            {
                _unactiveTourDTO = value;
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
                ShowTourTrackingWindow();
            }
        }

        public void ShowTourTrackingWindow()
        {

            if (_selectedTourDTO == null)
            {
                return;
            }

            var selectedItem = _selectedTourDTO as TourDTO;
            TrackTourWindow trackTourWindow = new TrackTourWindow(new TourDTO(selectedItem));
            trackTourWindow.ShowDialog();
        }
    }
}
