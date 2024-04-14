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
    public class TouristDTO : INotifyPropertyChanged
    {
        public TouristDTO() { }

        public TouristDTO(string name, string surname, int age)
        {
            this.name = name;
            this.surname = surname;
            this.age = age;
        }

        public TouristDTO(Tourist tourist)
        {
            id = tourist.Id;
            name = tourist.Name;
            surname = tourist.Surname;
            age = tourist.Age;
            joiningKeyPoint = tourist.JoiningKeyPoint;
            if (tourist.Review != null)
            {
                review = new TourReviewDTO(tourist.Review);
            }
        }

        public TouristDTO(TouristDTO touristDTO)
        {
            id = touristDTO.Id;
            name = touristDTO.name;
            surname = touristDTO.surname;
            age = touristDTO.age;
            flag = touristDTO.flag;
            joiningKeyPoint = touristDTO.joiningKeyPoint;
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

        private bool flag;
        public bool Flag
        {
            get { return flag; }
            set
            {
                if (value != flag)
                {
                    flag = value;
                    OnPropertyChanged();
                }
            }
        }
      
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (value != name)
                {
                    name = value;
                    OnPropertyChanged();
                }
            }
        }

        private string surname;
        public string Surname
        {
            get { return surname; }
            set
            {
                if (value != surname)
                {
                    surname = value;
                    OnPropertyChanged();
                }
            }
        }

        private int age;
        public int Age
        {
            get { return age; }
            set
            {
                if (value != age)
                {
                    age = value;
                    OnPropertyChanged();
                }
            }
        }

        private string joiningKeyPoint;
        public string JoiningKeyPoint
        {
            get { return joiningKeyPoint; }
            set
            {
                if (value != joiningKeyPoint)
                {
                    joiningKeyPoint = value;
                    OnPropertyChanged();
                }
            }
        }

        private TourReviewDTO review;
        public TourReviewDTO Review
        {
            get { return review; }
            set
            {
                if (value != review)
                {
                    review = value;
                    OnPropertyChanged();
                }
            }
        }


        public Tourist ToTourist()
        {
            if (review != null)
            {
                Tourist tourist = new Tourist(id, name, surname, age, joiningKeyPoint, review.ToTourReview());
                return tourist;
            }
            else
            {
                Tourist tourist =  new Tourist(id, name, surname, age, joiningKeyPoint);
                return tourist;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
