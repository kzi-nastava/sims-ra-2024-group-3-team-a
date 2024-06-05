using BookingApp.DTO;
using BookingApp.ViewModel.Owner.WizardAndHelp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.View.Owner.WizardAndHelp
{
    
    public partial class InboxHelpPage : Page
    {
        private InboxHelpViewModel _inboxHelpViewModel;
        public InboxHelpPage(UserDTO loggedInUser)
        {
            InitializeComponent();

            _inboxHelpViewModel = new InboxHelpViewModel(loggedInUser);
            DataContext = _inboxHelpViewModel;
        }
    }
}
