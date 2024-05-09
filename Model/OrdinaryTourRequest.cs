using BookingApp.Model.Enums;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace BookingApp.Model
{
    public class OrdinaryTourRequest: ISerializable
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int GuideId { get; set; }
        public Location Place { get; set; }

        public string Description { get; set; }

        public Languages Language { get; set; }

        public List<Tourist> Tourists { get; set; }

        public int NumberOfTourists { get; set; }

        public TourRequestStatus Status { get; set; }
        public DateOnly BeginDate { get; set; }
        public DateOnly EndDate { get; set; }

        public OrdinaryTourRequest()
        {
            Place = new Location();
            Tourists = new List<Tourist>();
        }

        public OrdinaryTourRequest(int id, int guideId,int userId, Location place, string description, Languages language, List<Tourist> tourists, int numberOfTourists, TourRequestStatus status, DateOnly beginDate, DateOnly endDate)
        {
            Id = id;
            GuideId = guideId;
            UserId = userId;
            Place = place;
            Description = description;
            Language = language;
            Tourists = tourists;
            NumberOfTourists = numberOfTourists;
            Status = status;
            BeginDate = beginDate;
            EndDate = endDate;
        }

        public string[] ToCSV()
        {
            if (Tourists != null)
            {
                string tourists = CreateTouristsString();
                string[] csvValues = { Id.ToString(),GuideId.ToString(), UserId.ToString(), Place.City, Place.Country, Description, Language.ToString(), NumberOfTourists.ToString(), Status.ToString(), BeginDate.ToString(), EndDate.ToString(), tourists };
                return csvValues;
            }
            else
            {
                string[] csvValues = { Id.ToString(), GuideId.ToString(), UserId.ToString(), Place.City, Place.Country, Description, Language.ToString(), NumberOfTourists.ToString(), Status.ToString(), BeginDate.ToString(), EndDate.ToString() };
                return csvValues;
            }
        }

        private string CreateTouristsString()
        {
            string tourists = string.Empty;
            foreach (var tourist in Tourists)
            {
                tourists += tourist.Name + '|' + tourist.Surname + '|' + tourist.Age.ToString() + '|' + tourist.JoiningKeyPoint + '|';
            }

            tourists = tourists.Substring(0, tourists.Length - 1);
            return tourists;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            GuideId = Convert.ToInt32(values[1]);
            UserId = Convert.ToInt32(values[2]);
            Place.City = values[3];
            Place.Country = values[4];
            Description = values[5];
            Language = (Languages)Enum.Parse(typeof(Languages), values[6]);
            NumberOfTourists = Convert.ToInt32(values[7]);
            Status = (TourRequestStatus)Enum.Parse(typeof(TourRequestStatus), values[8]);
            BeginDate = DateOnly.Parse(values[9]);
            EndDate = DateOnly.Parse(values[10]);
            for (int i = 11; i < values.Length; i = i + 4)
            {
                if (i + 3 < values.Length)
                {
                    Tourists.Add(new Tourist(values[i], values[i + 1], Convert.ToInt32(values[i + 2]), values[i + 3]));
                }
            }
        }
    }
}
