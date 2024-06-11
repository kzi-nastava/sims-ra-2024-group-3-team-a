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
    public class TouristProfileDTO: INotifyPropertyChanged
    {
        public TouristProfileDTO() { }

        public TouristProfileDTO(string name, string surname, int age)
        {
            this.name = name;
            this.surname = surname;
            this.age = age;
        }

        public TouristProfileDTO(TouristProfile tourist)
        {
            id = tourist.Id;
            userId = tourist.UserId;
            name = tourist.Name;
            surname = tourist.Surname;
            age = tourist.Age;
           
        }

        public TouristProfileDTO(TouristProfileDTO touristDTO)
        {
            id = touristDTO.Id;
            userId = touristDTO.UserId;
            name = touristDTO.name;
            surname = touristDTO.surname;
            age = touristDTO.age;
         
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

       



        public TouristProfile ToTouristProfile()
        {

            TouristProfile tourist = new TouristProfile(id, userId,name, surname, age);
             return tourist;
            
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
