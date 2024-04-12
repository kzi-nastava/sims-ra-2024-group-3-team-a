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

        public ObservableCollection<TourReviewDTO> Reviews { get; set; }

        public ReviewDetailsViewModel(TouristDTO touristDTO)
        {
            _touristDTO = touristDTO;
            _touristService = new TouristService();
            Reviews = new ObservableCollection<TourReviewDTO>();
            Update();
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
