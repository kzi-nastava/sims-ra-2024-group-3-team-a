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
    public class VoucherViewModel : ViewModel
    {
        private VoucherService _voucherService;

        private ObservableCollection<VoucherDTO> _voucherDTO;
        private UserDTO _userDTO;
        public VoucherViewModel(UserDTO loggedInUser)
        {
            _userDTO = loggedInUser;
            IVoucherRepository voucherRepository = Injector.CreateInstance<IVoucherRepository>();
            _voucherService = new VoucherService(voucherRepository);
            _voucherService.UpdateHeader();
            List<VoucherDTO> vouchers = _voucherService.GetAll().Select(vouchers => new VoucherDTO(vouchers)).ToList();
            _voucherDTO = new ObservableCollection<VoucherDTO>(vouchers);
            
        }

        public ObservableCollection<VoucherDTO> VouchersDTO
        {
            get
            {
                return _voucherDTO;
            }
            set
            {
                _voucherDTO = value;
                OnPropertyChanged();
            }
        }

        
    }
}
