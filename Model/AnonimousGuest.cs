using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class AnonimousGuest
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }

        public AnonimousGuest() { }
        public AnonimousGuest(string name, string surname,int age)
        {
            Name = name;
            Surname = surname;
            Age = age;
        } 
    }
}
