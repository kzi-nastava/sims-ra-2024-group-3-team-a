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
        public ObservableCollection<TourReviewDTO> Reviews { get; set; }

        public ReviewDetailsViewModel(TouristDTO touristDTO)
        {
            _touristDTO = touristDTO;
            _touristService = new TouristService();
            Reviews = new ObservableCollection<TourReviewDTO>();
            Update();
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

        private void Update()
        {
            Reviews.Add(_touristDTO.Review);
        }

        public void MarkAsInvalid()
        {
            _touristDTO.Review.IsNotValid = "nije validna";
            _touristService.Update(_touristDTO.ToTourist());
        }
    }
}
