using Cafe.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cafe
{
    public partial class NomenclatureEdit : UserControl
    {
        public int? Id { get; set; }

        public event Action<bool,string?> Success;
        public Nomenclature? EditItem { get; set; }
        public NomenclatureEdit()
        {
            InitializeComponent();
        }

        public void Refresh()
        {
            if (Id != null)
            {
                using (var dbContext = Config.GetDbContext())
                {
                    EditItem = dbContext.Nomenclatures.Find(Id);  
                }
            }
            else
            {
                EditItem = new Nomenclature();
            }
        }

        private void Button_ClickOk(object sender, RoutedEventArgs e)
        {
            using (var dbContext = Config.GetDbContext())
            {
                if ( (NameTextBox.Text=="" || PriceTextBox.Text=="") || (NameTextBox.Text == "" && PriceTextBox.Text == "") )
                {
                    Success(false, "Object cannot be empty!");
                }
                else if (Id!=0)
                {
                    EditItem.Name = NameTextBox.Text;
                    EditItem.Price = double.Parse(PriceTextBox.Text);
                    dbContext.Nomenclatures.Update(EditItem);
                    dbContext.SaveChanges();

                    Success(true, string.Empty);
                }
                else
                {
                    dbContext.Nomenclatures.Add(
                        new Models.Nomenclature()
                        {
                            Name = NameTextBox.Text,
                            Price = double.Parse(PriceTextBox.Text)
                        });
                    dbContext.SaveChanges();

                    Success(true, string.Empty);
                }
            }
        }
        // Очищення вікна після завершення роботи та скидання UI-елементів до значень за замовчуванням


        private void Button_ClickCancel(object sender, RoutedEventArgs e)
        {
            Success?.Invoke(false, "Operation canceled!");
        }

        private void NomEditWindow_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Refresh();

            if (EditItem == null)
            {
                NameTextBox.Text = default;
                PriceTextBox.Text = default;
            }
            else
            {
                NameTextBox.Text = EditItem.Name ?? string.Empty;
                PriceTextBox.Text = EditItem.Price.ToString("0.00"); 
            }
       }
    }
}
