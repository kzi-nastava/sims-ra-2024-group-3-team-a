using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class TouristProfile: ISerializable
    {
        public int Id { get; set; }    
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public int Age { get; set; }    


        public TouristProfile()
        {

        }
        public TouristProfile(int id, int userId, string name, string surname, int age)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Surname = surname;
            Age = age;
        }

        public string[] ToCSV()
        {
        
          string[] csvValues = { Id.ToString(), UserId.ToString(),Name, Surname, Age.ToString()};
          return csvValues;
            

        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            UserId = Convert.ToInt32(values[1]);
            Name = values[2];
            Surname = values[3];
            Age = Convert.ToInt32(values[4]);
        }

    }

}
