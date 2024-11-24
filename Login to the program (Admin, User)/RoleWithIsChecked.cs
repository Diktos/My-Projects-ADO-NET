using Cafe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Login_to_the_program__Admin__User_
{
    public class RoleWithIsChecked:NotifyPropertyChanged // Для оновлення плюсиків принадлежності у тригері
    {
        public Role role;
        public bool isChecked;

        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                if (isChecked != value)
                {
                    isChecked = value;
                    OnPropertyChanged(nameof(IsChecked));
                }
            }
        }
        public Role Role
        {
            get { return role; }
            set
            {
                if (role != value)
                {
                    role = value;
                    OnPropertyChanged(nameof(Cafe.Models.Role));
                }
            }
        }
    }
}
