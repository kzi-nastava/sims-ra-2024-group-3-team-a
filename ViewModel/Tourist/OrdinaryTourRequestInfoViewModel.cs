using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Tourist
{
    public class OrdinaryTourRequestInfoViewModel : ViewModel
    {

       // private OrdinaryTourRequestService _ordinaryTourRequestService { get; set; }

        private OrdinaryTourRequestDTO _ordinaryTourRequestDTO { get; set; }

        

        public ObservableCollection<TouristDTO> _touristsDTO;
        public OrdinaryTourRequestInfoViewModel(OrdinaryTourRequestDTO ordinaryTourRequestDTO)
        {
            
            _ordinaryTourRequestDTO = new OrdinaryTourRequestDTO(ordinaryTourRequestDTO);
            _touristsDTO = new ObservableCollection<TouristDTO>(ordinaryTourRequestDTO.TouristsDTO);
    
        }

        public OrdinaryTourRequestDTO OrdinaryTourRequestDTO
        {
            get
            {
                return _ordinaryTourRequestDTO;
            }
            set
            {
                _ordinaryTourRequestDTO = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TouristDTO> TouristsDTO
        {
            get
            {
                return _touristsDTO;
            }

            set
            {
                _touristsDTO = value;
                OnPropertyChanged();

            }
        }




    }
}
