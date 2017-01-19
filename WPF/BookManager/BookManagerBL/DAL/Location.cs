using System.Collections.Generic;

namespace BookManagerBL.DAL
{
    public class Location
    {
        public Location()
        {
            Contents = new List<Book>();
        }

        public int LocationId { get; set; }
        public string QuickName { get; set; }
        public string Room { get; set; }
        public string Shelf { get; set; }

        public virtual ICollection<Book> Contents { get; set; }
    }

}