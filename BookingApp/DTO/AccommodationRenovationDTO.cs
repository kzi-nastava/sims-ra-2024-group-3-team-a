using BookingApp.InjectorNameSpace;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO
{
    public class AccommodationRenovationDTO : INotifyPropertyChanged
    {
        private IAccommodationRepository _accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();

        public AccommodationRenovationDTO() { }

        public AccommodationRenovationDTO(AccommodationRenovationDTO accommodationRenovationDTO)
        {
            id = accommodationRenovationDTO.Id;
            accommodationId = accommodationRenovationDTO.AccommodationId;
            beginDate = accommodationRenovationDTO.BeginDate;
            endDate = accommodationRenovationDTO.EndDate;
            description = accommodationRenovationDTO.Description;
        }

        public AccommodationRenovationDTO(AccommodationRenovation accommodationRenovation)
        {
            id = accommodationRenovation.Id;
            accommodationId = accommodationRenovation.AccommodationId;
            beginDate = accommodationRenovation.BeginDate;
            endDate = accommodationRenovation.EndDate;
            description = accommodationRenovation.Description;
        }

        private int id;
        public int Id
        {
            get { return id; }
            set
            {
                if (value != id)
                {
                    id = value;
                    OnPropertyChanged();
                }
            }
        }

        private int accommodationId;
        public int AccommodationId
        {
            get { return accommodationId; }
            set
            {
                if (value != accommodationId)
                {
                    accommodationId = value;
                    OnPropertyChanged();
                }
            }
        }

        private AccommodationDTO accommodationDTO;
        public AccommodationDTO AccommodationDTO
        {
            get { return new AccommodationDTO(_accommodationRepository.GetById(accommodationId)); }
            set {}
        }

        private DateTime beginDate;
        public DateTime BeginDate
        {
            get { return beginDate; }
            set
            {
                if (value != beginDate)
                {
                    beginDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime endDate;
        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                if (value != endDate)
                {
                    endDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                if (value != description)
                {
                    description = value;
                    OnPropertyChanged();
                }
            }
        }

        public AccommodationRenovation ToAccommodationRenovation()
        {
            return new AccommodationRenovation(id, accommodationId, beginDate, endDate, description);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
