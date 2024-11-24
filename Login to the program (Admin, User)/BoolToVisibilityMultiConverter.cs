using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Cafe.Models;

namespace Login_to_the_program__Admin__User_
{
    internal class BoolToVisibilityMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values[0] is int userId && values[1] is int userRoleId)
            {
                using (CafeDbContext context=new CafeDbContext())
                {
                    var userRoles = context.UserRole
                    .Where(ur => ur.WaiterId == userId)
                    .Select(ur => ur.RoleId)
                    .ToArray();

                    if (userRoles.Contains(userRoleId))
                    {
                        return Visibility.Visible;
                    }
                }

            }
            return Visibility.Hidden;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("BoolToVisibilityMultiConverter ConvertBack not supported.");
        }
    }
}
