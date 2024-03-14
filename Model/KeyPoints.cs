using BookingApp.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Serializer;
using System.Windows;
using System.Xml.Linq;

namespace BookingApp.Model
{
    public class KeyPoints : ISerializable
    {
        public int Id { get; set; }
        public string Begining { get; set; }
        public List <string> Middle { get; set; }
        public string Ending { get; set; }

        public KeyPoints()
        {

            Middle = new List<string>();
        }
        public KeyPoints(int id, string begining, List <string> middle, string ending)
        {
            Id = id;
            Begining= begining;
            Middle = middle;
            Ending = ending;
        }
        public KeyPoints(int id, string begining, string ending)
        {
            Id = id;
            Begining = begining;
            Middle = new List<string>();
            Ending = ending;
        }


          public string[] ToCSV()
          {
              if (Middle != null)
              {
                  string middlePoints = string.Join("|", Middle);
                  string[] csvValues = { Id.ToString(), Begining, Ending, middlePoints };
                  return csvValues;
              }
              else
              {
                  string[] csvValues = { Id.ToString(), Begining, Ending };
                  return csvValues;
              }
          }
        
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Begining = values[1];
            Ending = values[2];

            Middle.Clear();

            for (int i = 3; i < values.Length; i++)
            {
                Middle.Add(values[i]);
            }


        }


    }
}
