using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookingApp.View.Navigation
{
    public class ListViewNavigation : ListView
    {
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (e.Key == Key.Up)
            {
                if (SelectedItem != null)
                {
                    int index = Items.IndexOf(SelectedItem);
                    if (index > 0)
                    {
                        SelectedItem = Items[index - 1];
                        ScrollIntoView(SelectedItem);
                        e.Handled = true; // Handle the event to prevent default navigation
                    }
                }
            }
            else if (e.Key == Key.Down)
            {
                if (SelectedItem != null)
                {
                    int index = Items.IndexOf(SelectedItem);
                    if (index < Items.Count - 1)
                    {
                        SelectedItem = Items[index + 1];
                        ScrollIntoView(SelectedItem);
                        e.Handled = true; // Handle the event to prevent default navigation
                    }
                }
            }
        }
    }
}
