using BookingApp.Model;
using BookingApp.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO
{
    public class SuperGuestDTO
    {
        public SuperGuestDTO() { }

        public SuperGuestDTO(SuperGuest superGuest)
        {
             points = superGuest.Points;
             id = superGuest.Id;
             userId = superGuest.UserId;
             endingDate = superGuest.EndingDate;
        }
        private int points;
        public int Points
        {
            get { return points; }
            set
            {
                if (value != points)
                {
                    points = value;
                    OnPropertyChanged();
                }
            }
        }
        private int id;
        public int Id
        {
            get { return id; }
            set
            {
                if (value != id)
                {
                    id = value;
                    OnPropertyChanged();
                }
            }
        }
        private int userId;
        public int UserId
        {
            get { return userId; }
            set
            {
                if (value != userId)
                {
                    userId = value;
                    OnPropertyChanged();
                }
            }
        }
        private DateOnly endingDate;
        public DateOnly EndingDate
        {
            get { return endingDate; }
            set
            {
                if (value != endingDate)
                {
                    endingDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
