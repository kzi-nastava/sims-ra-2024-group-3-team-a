using BookingApp.Localization;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {


        private static App _instance;

        // Constructor
        public App()
        {
            _instance = this;
        }

        // Static property to access the current instance
        public static App Instance => _instance;

        // Current language property
        public CultureInfo CurrentLanguage => TranslationSource.Instance.CurrentCulture;
        public void ChangeLanguage(string lang)
        {
            TranslationSource.Instance.CurrentCulture = new System.Globalization.CultureInfo(lang);
        }
    }
}
