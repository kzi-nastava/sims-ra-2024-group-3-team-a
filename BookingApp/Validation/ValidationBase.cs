using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Validation
{
    public abstract class ValidationBase : ViewModel.ViewModel
    {
        public ValidationErrors ValidationErrors { get; set; }
        public bool IsValid { get; private set; }

        protected ValidationBase()
        {
            this.ValidationErrors = new ValidationErrors();
        }

        protected abstract void ValidateSelf1();
        protected abstract void ValidateSelf2();

        public void Validate1()
        {
            this.ValidationErrors.Clear();
            this.ValidateSelf1();
           
            this.IsValid = this.ValidationErrors.IsValid;
            this.OnPropertyChanged("IsValid");
            this.OnPropertyChanged("ValidationErrors");
        }
        public void Validate2()
        {
            this.ValidationErrors.Clear();
            this.ValidateSelf2();

            this.IsValid = this.ValidationErrors.IsValid;
            this.OnPropertyChanged("IsValid");
            this.OnPropertyChanged("ValidationErrors");
        }
        public void Validate12()
        {
            this.ValidationErrors.Clear();
            this.ValidateSelf2();
            this.ValidateSelf1();
            this.IsValid = this.ValidationErrors.IsValid;
            this.OnPropertyChanged("IsValid");
            this.OnPropertyChanged("ValidationErrors");
        }

    }
}
