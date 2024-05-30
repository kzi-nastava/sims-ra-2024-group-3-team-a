using BookingApp.Model;
using BookingApp.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO
{
    public class ComplexTourRequestDTO:INotifyPropertyChanged
    {
        public ComplexTourRequestDTO()
        {
            this.status = TourRequestStatus.OnWait;
        }

        public ComplexTourRequestDTO(int id, TourRequestStatus status)
        {
            this.id = id;
            this.status = status;
        }

        public ComplexTourRequestDTO(ComplexTourRequest complexTourRequest)
        {
            id = complexTourRequest.Id;
            status = complexTourRequest.Status;
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

        public ComplexTourRequest ToComplexTourRequest()
        {
            return new ComplexTourRequest(id, status);
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
