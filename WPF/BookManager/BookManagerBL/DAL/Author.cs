using System.Collections.Generic;

namespace BookManagerBL.DAL
{
    public class Author
    {
        public Author()
        {
            Authored = new List<Book>();
        }
        public int AuthorId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Book> Authored { get; set; }
    }
}