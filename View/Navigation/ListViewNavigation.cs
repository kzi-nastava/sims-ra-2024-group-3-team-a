using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookingApp.View.Navigation
{
    public class ListViewNavigation : ListView
    {
        public static bool GetSelectOnFocus(DependencyObject obj)
        {
            return (bool)obj.GetValue(SelectOnFocusProperty);
        }

        public static void SetSelectOnFocus(DependencyObject obj, bool value)
        {
            obj.SetValue(SelectOnFocusProperty, value);
        }

        public static readonly DependencyProperty SelectOnFocusProperty =
            DependencyProperty.RegisterAttached("SelectOnFocus", typeof(bool), typeof(ListViewNavigation), new PropertyMetadata(false, OnSelectOnFocusChanged));

        private static void OnSelectOnFocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ListViewItem listViewItem)
            {
                if ((bool)e.NewValue)
                {
                    listViewItem.GotFocus += ListViewItem_GotFocus;
                }
                else
                {
                    listViewItem.GotFocus -= ListViewItem_GotFocus;
                }
            }
        }

        private static void ListViewItem_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is ListViewItem listViewItem)
            {
                listViewItem.IsSelected = true;
            }
        }
    }
}
