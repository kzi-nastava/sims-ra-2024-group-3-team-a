using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;

namespace BookingApp.View.Navigation
{
    public class ArrowKeyNavigationBehavior
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
            if (d is Control control)
            {
                if ((bool)e.NewValue)
                {
                    control.PreviewKeyDown += Control_PreviewKeyDown;
                }
                else
                {
                    control.PreviewKeyDown -= Control_PreviewKeyDown;
                }
            }
        }

        private static void Control_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
            {
                var control = sender as Control;
                var parentListView = FindParent<ListView>(control);

                if (parentListView != null)
                {
                    parentListView.Focus();
                    parentListView.SelectedIndex = 0;
                    e.Handled = true;
                }
            }
            else if (e.Key == Key.Space)
            {
                var control = sender as Control;
                var parentListView = FindParent<ListView>(control);

                if (parentListView != null)
                {
                    parentListView.Focus();
                    parentListView.SelectedIndex = 0;
                    e.Handled = true;
                }
            }
        }

        private static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null) return null;

            T parent = parentObject as T;
            return parent ?? FindParent<T>(parentObject);
        }
    }
}
