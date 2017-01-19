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

namespace BookManager.Views.Book
{
    /// <summary>
    /// Interaction logic for AddBook.xaml
    /// </summary>
    public partial class AddBook : Window
    {
        List<BookManagerBL.DAL.Publisher> _publishers;
        List<BookManagerBL.DAL.Location> _locations;

        public AddBook()
        {
            InitializeComponent();
            using(var context = new BookContext())
            {
                _publishers = context.Publishers.ToList();
                comboPublisher.ItemsSource = _publishers;
                comboPublisher.DisplayMemberPath = "Name";
                comboPublisher.SelectedValuePath = "PublisherId";

                _locations = context.Locations.ToList();
                comboLocation.ItemsSource = _locations;
                comboLocation.DisplayMemberPath = "QuickName";
                comboLocation.SelectedValuePath = "LocationId";
            }
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            var service = new BookService();
            var location = comboLocation.SelectedValue;
            service.AddBook(new BookManagerBL.DAL.Book { Title = txtTitle.Text, PublishedYear = int.Parse(txtPublishedYear.Text), LocationId = (int)comboLocation.SelectedValue, PublisherId=(int)comboPublisher.SelectedValue});
            this.Close();
        }
    }
}
