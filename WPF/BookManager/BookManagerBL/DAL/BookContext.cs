using System.Data.Entity;

namespace BookManagerBL.DAL
{
    public class BookContext : DbContext
    {
        public BookContext() : base("BookDB") { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
    }
}
