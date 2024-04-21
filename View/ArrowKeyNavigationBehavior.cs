using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace BookingApp.View
{
    public static class ArrowKeyNavigationBehavior
    {
       
        
            public static bool GetEnableArrowKeyNavigation(DependencyObject obj)
            {
                return (bool)obj.GetValue(EnableArrowKeyNavigationProperty);
            }

            public static void SetEnableArrowKeyNavigation(DependencyObject obj, bool value)
            {
                obj.SetValue(EnableArrowKeyNavigationProperty, value);
            }

            public static readonly DependencyProperty EnableArrowKeyNavigationProperty =
                DependencyProperty.RegisterAttached("EnableArrowKeyNavigation", typeof(bool), typeof(ArrowKeyNavigationBehavior), new PropertyMetadata(false, OnEnableArrowKeyNavigationChanged));

            private static void OnEnableArrowKeyNavigationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                if (d is ListBox listBox)
                {
                    if ((bool)e.NewValue)
                    {
                        listBox.PreviewKeyDown += ListBox_PreviewKeyDown;
                    }
                    else
                    {
                        listBox.PreviewKeyDown -= ListBox_PreviewKeyDown;
                    }
                }
            }

            private static void ListBox_PreviewKeyDown(object sender, KeyEventArgs e)
            {
                var listBox = sender as ListBox;
                if (listBox == null || !listBox.IsEnabled)
                    return;

                if (e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Up || e.Key == Key.Down)
                {
                    // Handle arrow key navigation here
                    if (e.Key == Key.Left || e.Key == Key.Up)
                    {
                        if (listBox.SelectedIndex > 0)
                        {
                            listBox.SelectedIndex--;
                            listBox.ScrollIntoView(listBox.SelectedItem);
                        }
                    }
                    else if (e.Key == Key.Right || e.Key == Key.Down)
                    {
                        if (listBox.SelectedIndex < listBox.Items.Count - 1)
                        {
                            listBox.SelectedIndex++;
                            listBox.ScrollIntoView(listBox.SelectedItem);
                        }
                    }

                    e.Handled = true;
                }
            }
        }
    
}
