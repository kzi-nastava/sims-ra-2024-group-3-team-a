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

        public AnonymousTouristDTO(AnonymousTourist anonymousTourist)
        {
            id = anonymousTourist.Id;
            name = anonymousTourist.Name;
            surname = anonymousTourist.Surname;
            age = anonymousTourist.Age;
            flag = anonymousTourist.Flag;
            joiningKeyPoint = anonymousTourist.JoiningKeyPoint;

        }

       public AnonymousTouristDTO(AnonymousTouristDTO anonymousTouristDTO)
        {
            id = anonymousTouristDTO.Id;
            name = anonymousTouristDTO.name;
            surname = anonymousTouristDTO.surname;
            age = anonymousTouristDTO.age;
            flag = anonymousTouristDTO.flag;
            joiningKeyPoint = anonymousTouristDTO.joiningKeyPoint;

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
