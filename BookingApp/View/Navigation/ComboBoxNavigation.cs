using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookingApp.View.Navigation
{
    public class ComboBoxNavigation : ComboBox
    {


        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (e.Key == Key.Up)
            {
                if (SelectedIndex > 0)
                {
                    SelectedIndex--;
                    e.Handled = true; // Handle the event to prevent default navigation
                }
            }
            else if (e.Key == Key.Down)
            {
                if (SelectedIndex < Items.Count - 1)
                {
                    SelectedIndex++;
                    e.Handled = true; // Handle the event to prevent default navigation
                }
            }
            else if ( e.Key == Key.Left)
            {
                MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));
            }  
            else if ( e.Key == Key.Right)
            {
                MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
                
        }
    }  

}   
