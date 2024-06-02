using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using BookingApp.View.Tourist;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Tourist
{
    public class CreateTourRequestViewModel:ViewModel
    {
        private RelayCommand _openOrdinaryTourRequestWindowCommand {  get; set; }
        private RelayCommand _openComplexTourRequestWindowCommand {  get; set; }

        private UserDTO _userDTO;
        public CreateTourRequestViewModel(UserDTO loggedInUser)
        {
            _userDTO = loggedInUser;
            _openOrdinaryTourRequestWindowCommand = new RelayCommand(OpenOrdinaryTourRequestWindow);
            _openComplexTourRequestWindowCommand = new RelayCommand(OpenComplexTourRequestWindow);
        }

        public RelayCommand OpenOrdinaryTourRequestWindowCommand
        {
            get
            {
                return _openOrdinaryTourRequestWindowCommand;
            }
            set
            {
                _openOrdinaryTourRequestWindowCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand OpenComplexTourRequestWindowCommand
        {
            get
            {
                return _openComplexTourRequestWindowCommand;
            }
            set
            {
                _openComplexTourRequestWindowCommand = value;
                OnPropertyChanged();
            }
        }

        public void OpenComplexTourRequestWindow()
        {
            ComplexTourRequestWindow complexTourRequestWindow = new ComplexTourRequestWindow(_userDTO);
            complexTourRequestWindow.ShowDialog();
        }
        public void OpenOrdinaryTourRequestWindow()
        {
            OrdinaryTourRequestWindow ordinaryTourRequestWindow = new OrdinaryTourRequestWindow(_userDTO, -1);
            ordinaryTourRequestWindow.ShowDialog();
        }
    }
}
