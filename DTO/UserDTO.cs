using BookingApp.Model;
using BookingApp.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;

namespace BookingApp.DTO
{
    public class UserDTO
    {
        public UserDTO() { }

        public UserDTO(int id, string username, string password, UserRole role, bool isSuper)
        {
            this.id = id;
            this.username=username;
            this.password = password;
            this.role=role;
            this.isSuper = isSuper;
        }

        public UserDTO(User user)
        {
            id = user.Id;
            username=user.Username;
            password=user.Password;
            role=user.Role;
            isSuper = user.IsSuper;
        }

        public UserDTO(UserDTO userDTO)
        {
            id=userDTO.id;
            username=userDTO.username;
            password=userDTO.password;
            role=userDTO.role;
            isSuper=userDTO.isSuper;
        }
        
        private int id;
        public int Id
        {
            get { return id; }
            set
            {
                if (value != id)
                {
                    id = value;
                    OnPropertyChanged();
                }
            }
        }
        private string username;
        public string Username
        {
            get { return username; }
            set
            {
                if (value != username)
                {
                    username = value;
                    OnPropertyChanged();
                }
            }
        }
        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                if (value != password)
                {
                    password = value;
                    OnPropertyChanged();
                }
            }
        }
        private UserRole role;
        public UserRole Role
        {
            get { return role; }
            set
            {
                if (value != role)
                {
                    role = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool isSuper;
        public bool IsSuper
        {
            get { return isSuper; }
            set
            {
                if (value != isSuper)
                {
                    isSuper = value;
                    OnPropertyChanged();
                }
            }
        }

        public User ToUser()
        {
            return new User(id,username,password,role,isSuper);
        }



        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
