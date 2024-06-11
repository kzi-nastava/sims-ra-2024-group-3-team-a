using BookingApp.Model.Enums;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class ComplexTourRequest: ISerializable
    {
        public int Id { get; set; }
        public TourRequestStatus Status { get; set; }

       public ComplexTourRequest ()
       {
            Status = TourRequestStatus.OnWait;
       }

        public ComplexTourRequest(int id, TourRequestStatus status)
        {
            Id = id;
            Status = status;
        }

        public string[] ToCSV()
        {
                string[] csvValues = { Id.ToString(), Status.ToString(), };
                return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Status = (TourRequestStatus)Enum.Parse(typeof(TourRequestStatus), values[1]);
          
        }
    }
}
