using BookingApp.Model.Enums;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    class SuperGuide : ISerializable
    {
        public int Id { get; set; }
        public int GuideId { get; set; }
        public Languages Language { get; set; }
        public DateTime BeginingTime { get; set; }
     
        public SuperGuide()
        {
        }

        public SuperGuide(int id, int guideId, DateTime beginingTime,  Languages languages)
        {
            Id = id;
            GuideId = guideId;
            Language = languages;
        }

        public string[] ToCSV()
        {
      
                string[] csvValues = { Id.ToString(), GuideId.ToString(), BeginingTime.ToString(), Language.ToString() };
                return csvValues;
          
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            GuideId = Convert.ToInt32(values[1]);
            BeginingTime = DateTime.Parse(values[2]);
            Language = (Languages)Enum.Parse(typeof(Languages), values[3]);
        }
    }
}
