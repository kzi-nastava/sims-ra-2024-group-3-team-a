using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.Repository;
using BookingApp.Service;
using System.Collections.ObjectModel;
using System.Windows;

namespace BookingApp.ViewModel.Guide
{
    public class ReviewDetailsViewModel : ViewModel
    {
        private readonly TouristDTO _touristDTO;
        private readonly TouristService _touristService;
        private RelayCommand _markAsInvalidCommand;
        private ObservableCollection<TourReviewDTO> _reviews { get; set; }

        public ReviewDetailsViewModel(TouristDTO touristDTO)
        {
            _touristDTO = touristDTO;
            _touristService = new TouristService();
            Reviews = new ObservableCollection<TourReviewDTO>();
            Reviews.Add(_touristDTO.Review);
            _markAsInvalidCommand = new RelayCommand(MarkAsInvalid);
        }
        public RelayCommand MarkAsInvalidCommand
        {
            get { return _markAsInvalidCommand; }
            set
            {
                _markAsInvalidCommand = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<TourReviewDTO> Reviews
        {
            get { return _reviews; }
            set
            {
                _reviews = value;
                OnPropertyChanged();
            }
        }

        public void MarkAsInvalid()
        {
            _touristDTO.Review.IsNotValid = "nije validna";
            _touristService.Update(_touristDTO.ToTourist());
        }
    }
}
