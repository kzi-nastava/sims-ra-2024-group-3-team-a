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
    public class Post : ISerializable
    {
        public int Id;
        public int ForumId { get; set; }
        public String Username { get; set; }
        public string Text { get; set; }
        public int Reports { get; set; }
        public PostType Type { get; set; }
        public List<String> OwnersReported { get; set; }

        public Post()
        { 
            OwnersReported = new List<String>();
        }

        public Post(int id, int forumId, string username, string text, int reports, PostType type, List<String> ownersReported)
        {
            Id = id;
            ForumId = forumId;
            Username = username;
            Text = text;
            Reports = reports;
            Type = type;
            OwnersReported = ownersReported;
        }

        public string[] ToCSV()
        {
            if (OwnersReported != null)
            {
                string ownersReported = string.Join("|", OwnersReported);
                string[] csvValues = { Id.ToString(), ForumId.ToString(), Username, Text, Reports.ToString(), Type.ToString(), ownersReported };
                return csvValues;
            }
            else
            {
                string[] csvValues = { Id.ToString(), ForumId.ToString(), Username, Text, Reports.ToString(), Type.ToString() };
                return csvValues;
            }  
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            ForumId = Convert.ToInt32(values[1]);
            Username = values[2];
            Text = values[3];
            Reports = Convert.ToInt32(values[4]);
            Type = (PostType)Enum.Parse(typeof(PostType), values[5]);
            for (int i = 6; i < values.Length; i++)
            {
                OwnersReported.Add(values[i]);
            }
        }
    }
}
