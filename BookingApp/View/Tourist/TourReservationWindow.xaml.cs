using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Service;
using BookingApp.View.Tourist;
using BookingApp.ViewModel.Tourist;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for TourReservation.xaml
    /// </summary>
    public partial class TourReservationWindow : Window
    { 
      
        private Brush _defaultBrushBorder;

        private TourReservationViewModel _tourReservationViewModel;
        public static TourReservationWindow Instance;

        private IInputElement[] FocusChain;
        public TourReservationWindow(TourReservationService tourReservationService,TourDTO tourDTO, UserDTO userDTO)
        {
            InitializeComponent();
          //  _defaultBrushBorder=textBoxCurrentCapacity.BorderBrush.Clone();
         //   textBoxNumber.Text = 0.ToString();

             
            _tourReservationViewModel = new TourReservationViewModel(tourReservationService, tourDTO, userDTO);
            DataContext = _tourReservationViewModel;
            FocusChain = new IInputElement[]
            {
               textBoxName,
               textBoxSurname,
               textBoxAge
            };
            if (Instance == null)
            {
                Instance = this;
            }
            if (_tourReservationViewModel.CloseAction == null)
                _tourReservationViewModel.CloseAction = new Action(this.Close);
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            this.PreviewKeyDown += TouristReservationWindow_PreviewKeyDown;

        }
        public static TourReservationWindow GetInstance()
        {
            return Instance;
        }

        private void Menu_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var focusedElement = Keyboard.FocusedElement as DependencyObject;
            while (focusedElement != null && !(focusedElement is DataGrid) && !(focusedElement is ListView))
            {
                focusedElement = VisualTreeHelper.GetParent(focusedElement);
            }

            if (focusedElement is DataGrid)
            {
                DataGridTourists_KeyDown(sender, e);
                return;
            }
            else if (focusedElement is ListView)
            {
                ListView_KeyDown(sender, e);
                return;
            }
            // Handle only arrow key events
            if ( e.Key == Key.Down )
            {
                
                    // Navigate to the next element in the FocusChain when Up arrow key is pressed
                    var next = FocusChain[0];
                    for (var i = 0; i < FocusChain.Length; i++)
                    {
                        if (FocusChain[i].IsKeyboardFocusWithin)
                        {
                            next = FocusChain[(i + 1) % FocusChain.Length];
                            break;
                        }
                    }

                    next.Focus();
                    Keyboard.Focus(next);
                 
            }
                else  if(e.Key == Key.Up) 
                {
                    // Navigate to the previous element in the FocusChain when Down arrow key is pressed
                    var next = FocusChain[0];
                    for (var i = 0; i < FocusChain.Length; i++)
                    {
                        if (FocusChain[i].IsKeyboardFocusWithin && i != 0)
                        {
                            next = FocusChain[(i - 1) % FocusChain.Length];
                            break;
                        }
                    }

                    next.Focus();
                    Keyboard.Focus(next);
             
                }
        }
        private void DataGridTourists_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up || e.Key == Key.Down)
            {
                // Check if the DataGrid has focus
                if (sender is DataGrid dataGrid)
                {
                    // Get the currently selected item
                    var selectedItem = dataGrid.SelectedItem;

                    if (selectedItem != null)
                    {
                        // Get the index of the selected item
                        var selectedIndex = dataGrid.Items.IndexOf(selectedItem);

                        // Calculate the new index based on the arrow key pressed
                        var newIndex = e.Key == Key.Up ? selectedIndex - 1 : selectedIndex + 1;

                        // Ensure newIndex is within bounds
                        if (newIndex >= 0 && newIndex < dataGrid.Items.Count)
                        {
                            // Set the new selected item
                            dataGrid.SelectedItem = dataGrid.Items[newIndex];

                            // Scroll to the selected item if necessary
                            dataGrid.ScrollIntoView(dataGrid.SelectedItem);

                            e.Handled = true; // Mark the event as handled
                        }
                    }
                    else
                    {
                        // If no item is selected, let the event bubble up to the parent control
                        e.Handled = false;
                    }
                }
            }
        }

        private void ListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (sender is ListView listView)
            {
                if (listView.Items != null && listView.Items.Count > 0)
                {
                    if (listView.SelectedIndex >= 0 && listView.SelectedIndex < listView.Items.Count)
                    {
                        if (e.Key == Key.Up)
                        {
                            if (listView.SelectedIndex > 0)
                            {
                                listView.SelectedIndex--;
                            }
                            else
                            {
                                listView.SelectedIndex = listView.Items.Count - 1;
                            }
                        }
                        else if (e.Key == Key.Down)
                        {
                            if (listView.SelectedIndex < listView.Items.Count - 1)
                            {
                                listView.SelectedIndex++;
                            }
                            else
                            {
                                listView.SelectedIndex = 0;
                            }
                        }
                    }
                }
            }
        }

        private void TouristReservationWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F9 && (Keyboard.Modifiers & ModifierKeys.Shift) == 0 && (Keyboard.Modifiers & ModifierKeys.Control) == 0)
            {

                More.IsSubmenuOpen = true;
                Settings.Focus();
                e.Handled = true;
            }
        }


        /*private void textBox_TextChanged(object sender, EventArgs e)
        {
            if(CheckInput())
                buttonConfirm.IsEnabled = true;
            else
                buttonConfirm.IsEnabled = false;
        }
        private bool EmptyTextBoxCheck()
        {
            bool validInput = true;

            foreach(var control in gridMain.Children)
            {
                if(control is TextBox)
                { 
                    TextBox textBox = (TextBox)control;
                    if(textBox.Text==String.Empty)
                    {
                        BorderBrushToRed(textBox);
                        validInput = false;
                    }
                    else
                    {
                        BorderBrushToDefault(textBox);
                    }
                }
            }

            return validInput;
        }*/
        /* private bool CheckInput()
         {
             bool validInput = EmptyTextBoxCheck();

             if(!int.TryParse(textBoxNumber.Text,out int number))
             {
                 BorderBrushToRed(textBoxNumber);
                 validInput = false;
                 buttonOK.IsEnabled = false;
             }
             else
             {
                 if(int.Parse(textBoxNumber.Text) < 1)
                 {
                     BorderBrushToRed(textBoxNumber);
                     validInput = false;
                     buttonOK.IsEnabled = false;
                 }
                 else
                 {
                     BorderBrushToDefault(textBoxNumber);
                     buttonOK.IsEnabled = true;
                 }
             }

             if (!int.TryParse(textBoxAge.Text, out int age))
             {
                 BorderBrushToRed(textBoxAge);
                 validInput = false;
                 buttonSubmitInfo.IsEnabled = false;
             }
             else
             {
                 if (int.Parse(textBoxAge.Text) < 1)
                 {
                     BorderBrushToRed(textBoxAge);
                     validInput = false;
                     buttonSubmitInfo.IsEnabled = false;
                 }
                 else
                 {
                     BorderBrushToDefault(textBoxNumber);
                     buttonSubmitInfo.IsEnabled = true;
                 }
             }
             return validInput;

         }
         private void BorderBrushToRed(TextBox textBox)
         {
             textBox.BorderBrush = Brushes.HotPink;
             textBox.BorderThickness = new Thickness(2);
         }
         private void BorderBrushToDefault(TextBox textBox)
         {
             textBox.BorderBrush = _defaultBrushBorder;
             textBox.BorderThickness = new Thickness(2);
         }*/
    }
}
