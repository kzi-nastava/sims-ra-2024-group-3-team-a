using BookingApp.Model;
using BookingApp.Repository.Interfaces;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class OwnerSettingsRepository : IOwnerSettingsRepository
    {
        private const string FilePath = "../../../Resources/Data/ownerSettings.csv";

        private readonly Serializer<OwnerSettings> _serializer;

        private List<OwnerSettings> _ownerSettingsList;

        public OwnerSettingsRepository()
        {
            _serializer = new Serializer<OwnerSettings>();
            _ownerSettingsList = _serializer.FromCSV(FilePath);
        }

        public List<OwnerSettings> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public OwnerSettings Save(OwnerSettings ownerSettings)
        {
            ownerSettings.Id = NextId();
            _ownerSettingsList = _serializer.FromCSV(FilePath);
            _ownerSettingsList.Add(ownerSettings);
            _serializer.ToCSV(FilePath, _ownerSettingsList);
            return ownerSettings;
        }

        private int NextId()
        {
            _ownerSettingsList = _serializer.FromCSV(FilePath);
            if (_ownerSettingsList.Count < 1)
            {
                return 1;
            }
            return _ownerSettingsList.Max(os => os.Id) + 1;
        }

        public OwnerSettings Update(OwnerSettings ownerSettings)
        {
            _ownerSettingsList = _serializer.FromCSV(FilePath);
            OwnerSettings current = _ownerSettingsList.Find(os => os.Id == ownerSettings.Id);
            if (current == null) throw new Exception("OwnerSettings not found.");
            int index = _ownerSettingsList.IndexOf(current);
            _ownerSettingsList.Remove(current);
            _ownerSettingsList.Insert(index, ownerSettings);
            _serializer.ToCSV(FilePath, _ownerSettingsList);
            return ownerSettings;
        }

        public void Delete(OwnerSettings ownerSettings)
        {
            _ownerSettingsList = _serializer.FromCSV(FilePath);
            OwnerSettings found = _ownerSettingsList.Find(os => os.Id == ownerSettings.Id);
            if (found == null) throw new Exception("OwnerSettings not found.");
            _ownerSettingsList.Remove(found);
            _serializer.ToCSV(FilePath, _ownerSettingsList);
        }

        public OwnerSettings GetById(int id)
        {
            _ownerSettingsList = _serializer.FromCSV(FilePath);
            return _ownerSettingsList.Find(os => os.Id == id);
        }
    }
}
