using System.Windows.Controls;
using System.Windows.Input;

namespace BookingApp.View
{
    public class TextBoxNavigation : TextBox
    {
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (e.Key == Key.Up || e.Key == Key.Left)
                MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));
            else if (e.Key == Key.Down || e.Key == Key.Right)
                MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }
    }
}
