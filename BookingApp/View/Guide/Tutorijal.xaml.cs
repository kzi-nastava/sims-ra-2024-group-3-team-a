//using BookingApp.DTO;
//using BookingApp.Model;
//using BookingApp.Repository;
//using BookingApp.ViewModel.Guide;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;

//namespace BookingApp.View.Guide
//{

//    /// <summary>
//    /// Interaction logic for TouristStatisticsWindow.xaml
//    /// </summary>
//    public partial class Tutorijal : Window
//    {
//        public Tutorijal()
//        {
//            InitializeComponent();
//            DataContext = new TutorijalViewModel();
//            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
//        }
//    }
//}
using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using static iText.Layout.Borders.Border;

namespace BookingApp.View.Guide
{
    /// <summary>
    /// Interaction logic for Review.xaml
    /// </summary>
    public partial class Tutorijal : Window
    {
        DispatcherTimer timer;
        public Tutorijal()
        {
            InitializeComponent();
            mediaPlayer.Source = new Uri("../Resources/Images/Guide/Video/tutorijal.mp4", UriKind.RelativeOrAbsolute);
            mediaPlayer.Play();
        }
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Play();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Pause();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
        }
    }
}
