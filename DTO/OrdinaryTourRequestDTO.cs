using BookingApp.Model.Enums;
using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Runtime.CompilerServices;

namespace BookingApp.DTO
{
    public class OrdinaryTourRequestDTO:  INotifyPropertyChanged
    {
        public OrdinaryTourRequestDTO()
        {
            locationDTO = new LocationDTO();

        }

        public OrdinaryTourRequestDTO(int id, int guideId, int userId, Location place, string description, Languages language, List<Tourist> tourists, int numberOfTourists, TourRequestStatus status, DateOnly beginDate, DateOnly endDate)
        {
            this.id = id;
            this.guideId = guideId;
            this.userId = userId;
            locationDTO = new LocationDTO(place);
            this.description = description;
            this.language = language;
            this.numberOfTourists = numberOfTourists;
            this.status = status;
            this.beginDate = beginDate;
            this.endDate = endDate;
            touristsDTO = new List<TouristDTO>();
            foreach(Tourist tour in tourists)
            {
                touristsDTO.Add(new TouristDTO(tour));
            }

        }

        public OrdinaryTourRequestDTO ( OrdinaryTourRequest ordinaryTourRequest)
        {
            id = ordinaryTourRequest.Id ;
            userId =  ordinaryTourRequest.UserId ;
            guideId = ordinaryTourRequest.GuideId ;
            locationDTO = new LocationDTO(ordinaryTourRequest.Place);
            numberOfTourists = ordinaryTourRequest.NumberOfTourists ;
            status = ordinaryTourRequest .Status ;
            beginDate = ordinaryTourRequest .BeginDate ;
            endDate = ordinaryTourRequest .EndDate ;
            touristsDTO = new List<TouristDTO>();
            foreach (Tourist tour in ordinaryTourRequest.Tourists)
            {
                touristsDTO.Add(new TouristDTO(tour));
            }
        }

        public OrdinaryTourRequestDTO(OrdinaryTourRequestDTO ordinaryTourRequestDTO)
        {
            id = ordinaryTourRequestDTO.Id;
            userId = ordinaryTourRequestDTO.UserId;
            guideId = ordinaryTourRequestDTO.GuideId;
            locationDTO = new LocationDTO(ordinaryTourRequestDTO.LocationDTO);
            numberOfTourists = ordinaryTourRequestDTO.NumberOfTourists;
            status = ordinaryTourRequestDTO.Status;
            beginDate = ordinaryTourRequestDTO.BeginDate;
            endDate = ordinaryTourRequestDTO.EndDate;
            touristsDTO = ordinaryTourRequestDTO.TouristsDTO;
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

        private int guideId;
        public int GuideId

        {
            get { return guideId; }
            set
            {
                if (value != guideId)
                {
                    guideId = value;
                    OnPropertyChanged();
                }
            }
        }

        private int userId; 
        public int UserId
        {
            get { return userId; }
            set
            {
                if (value != userId)
                {
                    userId = value;
                    OnPropertyChanged();
                }
            }
        }

        private LocationDTO locationDTO;
        public LocationDTO LocationDTO
        {
            get { return locationDTO; }
            set
            {
                if (value != locationDTO)
                {
                    locationDTO = value;
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

        private Languages language;
        public Languages Language
        {
            get { return language; }
            set
            {
                if (value != language)
                {
                    language = value;
                    OnPropertyChanged();
                }
            }
        }

        private int numberOfTourists;
        public int NumberOfTourists

        {
            get { return numberOfTourists; }
            set
            {
                if (value != numberOfTourists)
                {
                    numberOfTourists = value;
                    OnPropertyChanged();
                }
            }
        }

        private TourRequestStatus status;
        public TourRequestStatus Status
        {
            get { return status; }
            set
            {
                if (value != status)
                {
                    status = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateOnly beginDate;
        public DateOnly BeginDate
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

        private DateOnly endDate;
        public DateOnly EndDate
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

        private List<TouristDTO> touristsDTO;
        public List<TouristDTO> TouristsDTO
        {
            get { return touristsDTO; }
            set
            {
                if (value != touristsDTO)
                {
                    touristsDTO = value;
                    OnPropertyChanged();
                }
            }
        }
        public string LocationDTOString
        {
            get { return locationDTO.ToString(); }

        }
        public OrdinaryTourRequest ToOrdinaryTourRequest()
        {
            List<Tourist> tourists = new List<Tourist>();

            foreach (TouristDTO touristDTO in touristsDTO)
            {
                tourists.Add(touristDTO.ToTourist());
            }

            return new OrdinaryTourRequest(id, guideId, userId, locationDTO.ToLocation(), description,language, tourists, numberOfTourists, status, beginDate, endDate);
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }




    }
}
