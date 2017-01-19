using BookManagerBL.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagerBL
{
    public class BookService
    {
        public void AddAuthor(Author author)
        {
            using (var db = new BookContext())
            {
                db.Authors.Add(author);
                db.SaveChanges();
            }
        }
        public void AddBook(Book book)
        {
            using (var db = new BookContext())
            {
                db.Books.Add(book);
                db.SaveChanges();
            }
        }
        public void AddLocation(Location location)
        {
            using (var db = new BookContext())
            {
                db.Locations.Add(location);
                db.SaveChanges();
            }
        }
        public void AddAuthor(Publisher publisher)
        {
            using (var db = new BookContext())
            {
                db.Publishers.Add(publisher);
                db.SaveChanges();
            }
        }
    }
}
