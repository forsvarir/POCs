using System.Collections.Generic;

namespace BookManagerBL.DAL
{
    public class Publisher
    {
        public Publisher()
        {
            PublishedBooks = new List<Book>();
        }
        public int PublisherId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Book> PublishedBooks { get; set; }
    }
}