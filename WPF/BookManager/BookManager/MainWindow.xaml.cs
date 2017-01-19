using BookManager.Views.Book;
using BookManager.Views.Location;
using BookManager.Views.Publisher;
using BookManager.Views.Author;
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

namespace BookManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_LocationAdd_Click(object sender, RoutedEventArgs e)
        {
            new AddLocation().ShowDialog();
        }

        private void MenuItem_AuthorAdd_Click(object sender, RoutedEventArgs e)
        {
            new AddAuthor().ShowDialog();
        }

        private void MenuItem_PublisherAdd_Click(object sender, RoutedEventArgs e)
        {
            new AddPublisher().ShowDialog();
        }

        private void MenuItem_BookAdd_Click(object sender, RoutedEventArgs e)
        {
            new AddBook().ShowDialog();
        }

        private void MenuItem_BookView_Click(object sender, RoutedEventArgs e)
        {
            new ViewBooks().ShowDialog();
        }

    }
}
