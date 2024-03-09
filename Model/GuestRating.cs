using BookingApp.Model.Enums;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookingApp.Model
{
    public class GuestRating
    {
        public int CleannessRating { get; set; }
        public int RulesRespectRating { get; set; }
        public string Comment { get; set; }

        public GuestRating() { }

        public GuestRating(int cleannessRating, int rulesRespectRating, string comment)
        {
            CleannessRating = cleannessRating;
            RulesRespectRating = rulesRespectRating;
            Comment = comment;
        }
    }
}
