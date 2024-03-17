﻿using BookingApp.Model;
using BookingApp.Model.Enums;
using BookingApp.Repository;
using BookingApp.View.Owner;
using BookingApp.View.Guest;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for SignInForm.xaml
    /// </summary>
    public partial class SignInForm : Window
    {

        private readonly UserRepository _repository;

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                if (value != _username)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SignInForm()
        {
            InitializeComponent();
            DataContext = this;
            _repository = new UserRepository();
        }

        private void SignIn(object sender, RoutedEventArgs e)
        {
            User user = _repository.GetByUsername(Username);
            if (user != null)
            {
                if(user.Password == txtPassword.Password)
                {
                    switch (user.Role)
                    {
                        case UserRole.Default:
                        { 
                            CommentsOverview commentsOverview = new CommentsOverview(user);
                            commentsOverview.Show();
                            Close();
                            break;
                        }
                        case UserRole.Owner:
                        {
                            OwnerMainWindow ownerMainWindow = new OwnerMainWindow(user);
                            ownerMainWindow.Show();
                            Close();
                            break;
                        }
                        case UserRole.Guest:
                        {
                            GuestMainWindow guestMainWindow = new GuestMainWindow(user);
                            guestMainWindow.Show();
                            Close();
                            MessageBox.Show("You signed in as Guest User!");
                            break;
                        }
                        case UserRole.Guide:
                        {
                                GuideMainWindow guideMainWindow= new GuideMainWindow(user);
                                guideMainWindow.Show();
                                break;
                            
                        }
                        case UserRole.Tourist:
                        {
                            MessageBox.Show("You signed in as Tourist User!");
                            TouristMainWindow tourView = new TouristMainWindow(user);
                            tourView.Show();
                            break;
                        }
                    }
  
                } 
                else
                {
                    MessageBox.Show("Wrong password!");
                }
            }
            else
            {
                MessageBox.Show("Wrong username!");
            }
            
        }
    }
}
