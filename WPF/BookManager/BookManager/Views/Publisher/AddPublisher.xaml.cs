using BookManagerBL;
using BookManagerBL.DAL;
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

namespace BookManager.Views.Publisher
{
    /// <summary>
    /// Interaction logic for AddPublisher.xaml
    /// </summary>
    public partial class AddPublisher : Window
    {
        public AddPublisher()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            var service = new BookService();
            service.AddAuthor(new BookManagerBL.DAL.Publisher { Name = txtName.Text });
            this.Close();
        }
    }
}
