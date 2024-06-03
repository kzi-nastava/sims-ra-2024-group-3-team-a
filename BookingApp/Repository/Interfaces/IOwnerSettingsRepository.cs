using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.Interfaces
{
    public interface IOwnerSettingsRepository
    {
        public List<OwnerSettings> GetAll();
        public OwnerSettings Save(OwnerSettings ownerSettings);
        public OwnerSettings Update(OwnerSettings ownerSettings);
        public void Delete(OwnerSettings ownerSettings);
        public OwnerSettings GetById(int id);
    }
}
