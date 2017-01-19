using BookManagerBL.DAL;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for ViewBooks.xaml
    /// </summary>
    public partial class ViewBooks : Window
    {
        List<BookManagerBL.DAL.Book> _books;
        object _image;
        BitmapImage _simage;
        BitmapImage _bimage;

        public ViewBooks()
        {
            InitializeComponent();

            _image = System.Drawing.Image.FromFile(@"c:\temp\TestImage.jpg");
            
            _simage = new System.Windows.Media.Imaging.BitmapImage(new Uri(@"file://c:\temp\TestImage.jpg"));

            var bytes = ConvertToBytes(_simage);

            _bimage = FromBytes(bytes);

            SelectBooks("");
        }

        public static BitmapImage FromBytes(byte[] buffer)
        {
            var image = new BitmapImage();
            using (var mem = new MemoryStream(buffer))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }

        public static byte[] ConvertToBytes(BitmapImage bitmapImage)
        {
            byte[] data;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }
            return data;
        }

        private void SelectBooks(string filter)
        {
            using (var db = new BookContext())
            {
                var q = db.Books.Include("Publisher").Include("Location");

                _books = string.IsNullOrEmpty(filter) ? q.ToList() : q.Where(x => x.Title.Contains(filter)).ToList();
            }
            _books.ForEach(x => x.Image = _bimage);// @"c:\temp\TestImage.jpg");

            lvBooks.ItemsSource = _books;
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            SelectBooks(txtFilterText.Text);
        }
        private void btnViewBook_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            new ViewBook(button.DataContext as BookManagerBL.DAL.Book).ShowDialog();
        }
        private void btnEditBook_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
