using BookingApp.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace BookingApp.DTO
{
    public class AnonymousGuestDTO
    {
        public AnonymousGuestDTO() { }

        public AnonymousGuestDTO(string name, string surname, int age)
        {
            this.name = name;
            this.surname = surname;
            this.age = age;
        }

        public AnonymousGuestDTO(AnonymousGuest anonaymousGuest)
        {
            name = anonaymousGuest.Name;
            surname = anonaymousGuest.Surname;
            age = anonaymousGuest.Age;
        }

        public AnonymousGuestDTO(AnonymousGuestDTO anonymousGuestDTO)
        {
            name = anonymousGuestDTO.name;
            surname = anonymousGuestDTO.surname;
            age = anonymousGuestDTO.age;

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
        public AnonymousGuest ToAnonymousGuest()
        {
            return new AnonymousGuest(name, surname, age);
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}