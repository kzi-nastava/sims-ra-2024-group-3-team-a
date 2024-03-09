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
    public class GuestRatingDTO
    {
        public GuestRatingDTO() { }

        public GuestRatingDTO(int cleannessRating, int rulesRespectRating, string comment)
        {
            this.cleannessRating = cleannessRating;
            this.rulesRespectRating = rulesRespectRating;
            this.comment = comment;
        }

        public GuestRatingDTO(GuestRatingDTO guestRatingDTO)
        {
            cleannessRating = guestRatingDTO.CleannessRating;
            rulesRespectRating = guestRatingDTO.RulesRespectRating;
            comment = guestRatingDTO.Comment;
        }

        public GuestRatingDTO(GuestRating guestRating)
        {
            cleannessRating = guestRating.CleannessRating;
            rulesRespectRating = guestRating.RulesRespectRating;
            comment = guestRating.Comment;
        }

        private int cleannessRating;
        public int CleannessRating
        {
            get { return cleannessRating; }
            set
            {
                if (value != cleannessRating)
                {
                    cleannessRating = value;
                    OnPropertyChanged();
                }
            }
        }

        private int rulesRespectRating;
        public int RulesRespectRating
        {
            get { return rulesRespectRating; }
            set
            {
                if (value != rulesRespectRating)
                {
                    rulesRespectRating = value;
                    OnPropertyChanged();
                }
            }
        }

        private string comment;
        public string Comment
        {
            get { return comment; }
            set
            {
                if (value != comment)
                {
                    comment = value;
                    OnPropertyChanged();
                }
            }
        }   

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public GuestRating ToGuestRating()
        {
            return new GuestRating(cleannessRating, rulesRespectRating, comment);
        }
    }
}
