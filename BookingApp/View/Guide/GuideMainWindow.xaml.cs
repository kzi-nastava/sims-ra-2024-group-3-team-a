using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.View.Guide;
using BookingApp.View.Owner;
using BookingApp.ViewModel.Guide;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookingApp.View
{

    public partial class GuideMainWindow : Window
    {
        public static GuideMainWindow Instance;
        public GuideMainWindow(User guide)
        {
            InitializeComponent();
            DataContext = new GuideMainViewModel(guide);
            Instance = this;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }
        public static GuideMainWindow GetInstance()
        {
            return Instance;
        }
       

    }
}
