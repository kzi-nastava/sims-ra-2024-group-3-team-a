using BookingApp.DTO;
using BookingApp.Model.Enums;
using BookingApp.ViewModel.Guest;
using BookingApp.ViewModel.Owner.ForumViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace BookingApp.View.Guest
{
    /// <summary>
    /// Interaction logic for GuestForumDeatilsPage.xaml
    /// </summary>
    public partial class GuestForumDetailsPage : Page
    {
        private GuestForumDetailsViewModel _guestForumDetailsViewModel;
        public GuestForumDetailsPage(ForumDTO selectedForum, UserDTO loggedInUser)
        {
            InitializeComponent();

            _guestForumDetailsViewModel = new GuestForumDetailsViewModel(selectedForum, loggedInUser);
            DataContext = _guestForumDetailsViewModel;
        }
    }
    public class PostTypeToHeaderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            PostDTO post = (PostDTO)value;

            if (post.Type == PostType.SentByOwner)
            {
                return "★Owner★" + " " + post.Username;
            }
            else if (post.Type == PostType.SentByGuestWhoWasOnLocation)
            {
                return "★Guest★" + " " + post.Username;
            }
            else
            {
                return post.Username;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
