using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace BookingApp.View.Navigation 
{
    public class IntegerUpDownNavigation : IntegerUpDown
    {
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (e.Key == Key.Up)
            {
                if (Value < Maximum)
                {
                    Value++;
                    e.Handled = true; // Handle the event to prevent default navigation
                }
            }
            else if (e.Key == Key.Down)
            {
                if (Value > Minimum)
                {
                    Value--;
                    e.Handled = true; // Handle the event to prevent default navigation
                }
            }
            else if (e.Key == Key.Left)
            {
                MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));
            }
            else if (e.Key == Key.Right)
            {
                MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }

        }
    }
}
