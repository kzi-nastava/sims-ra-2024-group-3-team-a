using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO
{
    public class ReviewDTO
    {
        public ReviewDTO()
        {
            GuestImages = new List<string>();
        }

        public ReviewDTO(int ownerCleannessRating, int ownerRulesRespectRating, string ownerComment, int guestCleanlinessRating, int guestHospitalityRating, string guestComment, int guestRenovationRating, string guestRenovationComment, List<string> guestImages)
        {
            this.ownerCleannessRating = ownerCleannessRating;
            this.ownerRulesRespectRating = ownerRulesRespectRating;
            this.ownerComment = ownerComment;
            this.guestCleanlinessRating = guestCleanlinessRating;
            this.guestHospitalityRating = guestHospitalityRating;
            this.guestComment = guestComment;
            this.guestRenovationRating = guestRenovationRating;
            this.guestRenovationComment = guestRenovationComment;
            this.guestImages = guestImages;
        }

        public ReviewDTO(int ownerCleannessRating, int ownerRulesRespectRating, string ownerComment)
        {
            this.ownerCleannessRating = ownerCleannessRating;
            this.ownerRulesRespectRating = ownerRulesRespectRating;
            this.ownerComment = ownerComment;
        }

        public ReviewDTO(int guestCleanlinessRating, int guestHospitalityRating, string guestComment, List<string> guestImages)
        {
            this.guestCleanlinessRating = guestCleanlinessRating;
            this.guestHospitalityRating = guestHospitalityRating;
            this.guestComment = guestComment;
            this.guestImages = guestImages;
        }

        public ReviewDTO(ReviewDTO reviewDTO)
        {
            ownerCleannessRating = reviewDTO.OwnerCleannessRating;
            ownerRulesRespectRating = reviewDTO.OwnerRulesRespectRating;
            ownerComment = reviewDTO.OwnerComment;
            guestCleanlinessRating = reviewDTO.GuestCleanlinessRating;
            guestHospitalityRating = reviewDTO.GuestHospitalityRating;
            guestComment = reviewDTO.GuestComment;
            guestRenovationRating = reviewDTO.GuestRenovationRating;
            guestRenovationComment = reviewDTO.GuestRenovationComment;
            guestImages = reviewDTO.GuestImages;
        }

        public ReviewDTO(Review review)
        {
            ownerCleannessRating = review.OwnerCleannessRating;
            ownerRulesRespectRating = review.OwnerRulesRespectRating;
            ownerComment = review.OwnerComment;
            guestCleanlinessRating = review.GuestCleannessRating;
            guestHospitalityRating = review.GuestHospitalityRating;
            guestComment = review.GuestComment;
            guestRenovationRating = review.GuestRenovationRating;
            guestRenovationComment = review.GuestRenovationComment;
            guestImages = review.GuestImages;
        }

        private int ownerCleannessRating;
        public int OwnerCleannessRating
        {
            get { return ownerCleannessRating; }
            set
            {
                if (value != ownerCleannessRating)
                {
                    ownerCleannessRating = value;
                    OnPropertyChanged();
                }
            }
        }

        private int ownerRulesRespectRating;
        public int OwnerRulesRespectRating
        {
            get { return ownerRulesRespectRating; }
            set
            {
                if (value != ownerRulesRespectRating)
                {
                    ownerRulesRespectRating = value;
                    OnPropertyChanged();
                }
            }
        }

        private string ownerComment;
        public string OwnerComment
        {
            get { return ownerComment; }
            set
            {
                if (value != ownerComment)
                {
                    ownerComment = value;
                    OnPropertyChanged();
                }
            }
        }

        private int guestCleanlinessRating;
        public int GuestCleanlinessRating
        {
            get { return guestCleanlinessRating; }
            set
            {
                if (value != guestCleanlinessRating)
                {
                    guestCleanlinessRating = value;
                    OnPropertyChanged();
                }
            }
        }

        private int guestHospitalityRating;
        public int GuestHospitalityRating
        {
            get { return guestHospitalityRating; }
            set
            {
                if (value != guestHospitalityRating)
                {
                    guestHospitalityRating = value;
                    OnPropertyChanged();
                }
            }
        }

        private string guestComment;
        public string GuestComment
        {
            get { return guestComment; }
            set
            {
                if (value != guestComment)
                {
                    guestComment = value;
                    OnPropertyChanged();
                }
            }
        }

        private int guestRenovationRating;
        public int GuestRenovationRating
        {
            get { return guestRenovationRating; }
            set
            {
                if (value != guestRenovationRating)
                {
                    guestRenovationRating = value;
                    OnPropertyChanged();
                }
            }
        }

        private string guestRenovationComment;
        public string GuestRenovationComment
        {
            get { return guestRenovationComment; }
            set
            {
                if (value != guestRenovationComment)
                {
                    guestRenovationComment = value;
                    OnPropertyChanged();
                }
            }
        }

        private List<string> guestImages;
        public List<string> GuestImages
        {
            get { return guestImages; }
            set
            {
                if (value != guestImages)
                {
                    guestImages = value;
                    OnPropertyChanged();
                }
            }
        }   

        public Review ToGuestRating()
        {
            return new Review(ownerCleannessRating, ownerRulesRespectRating, ownerComment, guestCleanlinessRating, guestHospitalityRating, guestComment, guestRenovationRating, guestRenovationComment, guestImages);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }    
    }
}
