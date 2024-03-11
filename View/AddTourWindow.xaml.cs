using BookingApp.DTO;
using BookingApp.Model.Enums;
using BookingApp.Repository;
using Microsoft.Win32;
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
using System.Windows.Shapes;

namespace BookingApp.View
{
  
    public partial class AddTourWindow : Window
    {
        private TourRepository _repository;
        private KeyPointRepository _keyPointRepository;
        private TourDTO _tourDTO;
        private string _tourKeyPoints;

        private List<string> images;

        public event EventHandler TourAdded;
        private AllToursView _allToursView;

        public AddTourWindow(AllToursView allToursView)
        {
            InitializeComponent();
            _repository = new TourRepository();
            _keyPointRepository = new KeyPointRepository();
            _tourDTO = new TourDTO();
            _allToursView = allToursView;
            
            DataContext = _tourDTO; 
        }

       

        private void AddImages(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;

            bool? response = openFileDialog.ShowDialog();

            if (response == true)
            {
                images = openFileDialog.FileNames.ToList();

                for (int i = 0; i < images.Count; i++)
                {
                    images[i] = System.IO.Path.GetRelativePath(AppDomain.CurrentDomain.BaseDirectory, images[i]).ToString();
                }
            }
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            _tourKeyPoints = textBoxKeyPoints.Text;
            string[] tourKeyPoints = _tourKeyPoints.Split(',');
            if (tourKeyPoints.Length < 1)
            {
                MessageBox.Show("Potrebno uneti najmanje dve kljucne tacke (pocetnu i krajnju)");
                return;
            }

            _tourDTO.KeyPointsDTO.Begining = tourKeyPoints[0];
            for(int i = 1; i < tourKeyPoints.Length-1; i++)
            {
                _tourDTO.KeyPointsDTO.Middle.Add(tourKeyPoints[i]);
            }
            _tourDTO.KeyPointsDTO.Ending = tourKeyPoints[tourKeyPoints.Length - 1];
 
            int id = (_keyPointRepository.Save(_tourDTO.KeyPointsDTO.ToKeyPoint())).Id;
            _tourDTO.Images = images;
            _tourDTO.KeyPointsDTO.Id = id;


            _repository.Save(_tourDTO.ToTourAllParam());
            


            _allToursView.Update();
            Close();
        }

        private void LeaveButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
