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
    public class AnonymousTouristDTO
    {
       public AnonymousTouristDTO() { }

        public AnonymousTouristDTO(string name, string surname, int age)
        {
            this.name=name;
            this.surname=surname;
            this.age=age;
        }

        public AnonymousTouristDTO(AnonymousTourist anonaymousTourist)
        {
            name = anonaymousTourist.Name;
            surname = anonaymousTourist.Surname;
            age = anonaymousTourist.Age;
        }

       public AnonymousTouristDTO(AnonymousTouristDTO anonymousTouristDTO)
        {
            name = anonymousTouristDTO.name;
            surname = anonymousTouristDTO.surname;
            age = anonymousTouristDTO.age;

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
        public AnonymousTourist ToAnonymousTourist()
        {
            return new AnonymousTourist(name,surname,age);
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
