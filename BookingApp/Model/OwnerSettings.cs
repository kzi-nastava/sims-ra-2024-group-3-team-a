using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class OwnerSettings : ISerializable
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public bool SeenWizard { get; set; }
        public bool HelpOn { get; set; }

        public OwnerSettings() { }

        public OwnerSettings(int id, int ownerId, bool seenWizard, bool helpOn)
        {
            Id = id;
            OwnerId = ownerId;
            SeenWizard = seenWizard;
            HelpOn = helpOn;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), OwnerId.ToString(), SeenWizard.ToString(), HelpOn.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            OwnerId = Convert.ToInt32(values[1]);
            SeenWizard = Convert.ToBoolean(values[2]);
            HelpOn = Convert.ToBoolean(values[3]);
        }
    }
}
