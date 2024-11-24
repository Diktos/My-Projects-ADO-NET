using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Login_to_the_program__Admin__User_
{
    public partial class ControlMenu : Window
    {
        public ControlMenu()
        {
            InitializeComponent();
        }

        private void editingDataUsersButton_Click(object sender, RoutedEventArgs e)
        {
            DataEditWindow dataEditWindow = new DataEditWindow();
            dataEditWindow.ShowDialog();
        }

        private void rightsEditorButton_Click(object sender, RoutedEventArgs e)
        {
            RightsEditWindow rightsEditWindow = new RightsEditWindow();
            rightsEditWindow.ShowDialog();
        }

    }
}
