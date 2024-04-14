using BookingApp.DTO;
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

        public VoucherViewModel()
        {

            _voucherService = new VoucherService();
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
