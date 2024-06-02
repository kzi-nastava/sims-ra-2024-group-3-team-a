using BookingApp.InjectorNameSpace;
using BookingApp.Model;
using BookingApp.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Service
{
    public class OwnerSettingsService
    {
        private IOwnerSettingsRepository _ownerSettingsRepository;

        public OwnerSettingsService(IOwnerSettingsRepository ownerSettingsRepository)
        {
            _ownerSettingsRepository = ownerSettingsRepository;
        }


        public List<OwnerSettings> GetAll()
        {
            return _ownerSettingsRepository.GetAll();
        }

        public OwnerSettings GetOwnerSettingsByOwner(User Owner)
        {
            return GetAll().FirstOrDefault(os => os.OwnerId == Owner.Id);
        }

        public OwnerSettings Save(OwnerSettings ownerSettings)
        {
            return _ownerSettingsRepository.Save(ownerSettings);
        }

        public OwnerSettings Update(OwnerSettings ownerSettings)
        {
            return _ownerSettingsRepository.Update(ownerSettings);
        }

        public void Delete(OwnerSettings ownerSettings)
        {
            _ownerSettingsRepository.Delete(ownerSettings);
        }

        public OwnerSettings GetById(int id)
        {
            return _ownerSettingsRepository.GetById(id);
        }
    }
}
