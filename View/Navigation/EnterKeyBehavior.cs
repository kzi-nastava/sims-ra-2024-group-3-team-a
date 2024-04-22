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
    public class EnterKeyBehavior
    {

        public static ICommand GetEnterKeyCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(EnterKeyCommandProperty);
        }

        public static void SetEnterKeyCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(EnterKeyCommandProperty, value);
        }

        public static readonly DependencyProperty EnterKeyCommandProperty =
            DependencyProperty.RegisterAttached("EnterKeyCommand", typeof(ICommand), typeof(EnterKeyBehavior), new PropertyMetadata(null, OnEnterKeyCommandChanged));

        private static void OnEnterKeyCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Button button)
            {
                button.PreviewKeyDown += Button_PreviewKeyDown;
            }
        }

        private static void Button_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && sender is Button button && button.Command != null && button.Command.CanExecute(null))
            {
                button.Command.Execute(null);
                e.Handled = true;
            }
        }
    }
}
