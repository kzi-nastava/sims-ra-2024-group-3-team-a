﻿using BookingApp.Model.Enums;
using BookingApp.Serializer;
using System;

namespace BookingApp.Model
{
    public class User : ISerializable
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
        public bool IsSuper { get; set; }

        public User() { }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public User(int id, string username, string password, UserRole role, bool isSuper)
        {
            Id = id;
            Username = username;
            Password = password;
            Role = role;
            IsSuper = isSuper;
        }   

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Username, Password, Role.ToString(), IsSuper.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Username = values[1];
            Password = values[2];
            Role = (UserRole)Enum.Parse(typeof(UserRole), values[3]);
            IsSuper = Convert.ToBoolean(values[4]);
        }
    }
}
