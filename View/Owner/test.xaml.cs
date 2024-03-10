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

namespace BookingApp.View.Owner
{
    /// <summary>
    /// Interaction logic for test.xaml
    /// </summary>
    public partial class test : Window
    {
        public test()
        {
            InitializeComponent();
        }

        private void CreateTextBoxes(object sender, RoutedEventArgs e)
        {
            gridMain.Children.Clear();
            for (int i = 0; i < Int32.Parse(textBoxNum.Text); i++)
            {
                TextBox putnikIme = new TextBox();
                TextBox putnikPrezime = new TextBox();
                TextBox putnikGodine = new TextBox();
                Label label = new Label();
                label.Content = "Putnik " + i;

                Grid.SetRow(label, i);
                Grid.SetColumn(label, 0);
                gridMain.Children.Add(label);

                Grid.SetRow(putnikIme, i);
                Grid.SetColumn(putnikIme, 1);
                gridMain.Children.Add(putnikIme);

                Grid.SetRow(putnikPrezime, i);
                Grid.SetColumn(putnikPrezime, 2);
                gridMain.Children.Add(putnikPrezime);

                Grid.SetRow(putnikGodine, i);
                Grid.SetColumn(putnikGodine, 3);
                gridMain.Children.Add(putnikGodine);  
            }
        }
    }
}
