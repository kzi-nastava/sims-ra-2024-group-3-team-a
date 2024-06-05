using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Model;
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
        private RelayCommand _searchCommand;
        private RelayCommand _showMyToursWindowCommand;
        private RelayCommand _showInboxWindowCommand;
        private RelayCommand _showFinishedToursWindowCommand;
        private RelayCommand _showVoucherWindowCommand;
        private RelayCommand _showAppropriateWindowCommand;
        private RelayCommand _popUpCommand;
        private RelayCommand _closePopUpCommand;
        private RelayCommand _logOutCommand;
        private RelayCommand _showCommand;
        private RelayCommand _showOrindaryTourRequestWindow;
        private RelayCommand _showSettingsWindowCommand;
        private RelayCommand _showTourRequestsCommand;
        private App app;
        private string _currentLanguage;
        public Action CloseAction { get; set; }
        private RelayCommand _closeWindowCommand;

        private UserDTO _userDTO;
        public CreateTourRequestViewModel(UserDTO loggedInUser)
        {
            _userDTO = loggedInUser;
            _openOrdinaryTourRequestWindowCommand = new RelayCommand(OpenOrdinaryTourRequestWindow);
            _openComplexTourRequestWindowCommand = new RelayCommand(OpenComplexTourRequestWindow);
            var currentLanguage = App.Instance.CurrentLanguage.Name;
            _closeWindowCommand =new  RelayCommand(CloseWindow);
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
       
        public RelayCommand ShowMyToursWindowCommand
        {
            get
            {
                return _showMyToursWindowCommand;
            }
            set
            {
                _showMyToursWindowCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand CloseWindowCommand
        {
            get
            {
                return _closeWindowCommand;
            }
            set
            {
                _closeWindowCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand ShowFinishedToursWindowCommand
        {
            get
            {
                return _showFinishedToursWindowCommand;
            }
            set
            {
                _showFinishedToursWindowCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand ShowTourRequestsCommand
        {
            get
            {
                return _showTourRequestsCommand;
            }
            set
            {
                _showTourRequestsCommand = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand ShowVoucherWindowCommand
        {
            get
            {
                return _showVoucherWindowCommand;
            }
            set
            {
                _showVoucherWindowCommand = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand ShowAppropriateWindowCommand
        {
            get
            {
                return _showAppropriateWindowCommand;
            }
            set
            {
                _showAppropriateWindowCommand = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand ShowOrindaryTourRequestWindowCommand
        {
            get
            {
                return _showOrindaryTourRequestWindow;
            }
            set
            {
                _showOrindaryTourRequestWindow = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand ShowInboxWindowCommand
        {
            get
            {
                return _showInboxWindowCommand;
            }
            set
            {
                _showInboxWindowCommand = value;
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
        public void CloseWindow()
        {

           
            CloseAction();
        }

    }
}
