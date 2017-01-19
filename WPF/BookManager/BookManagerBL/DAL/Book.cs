using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Media.Imaging;

namespace BookManagerBL.DAL
{
    public class Book
    {
        [NotMapped]
        public object Image { get; set; }

        public int BookId { get; set; }

        public string Title { get; set; }
        public int PublishedYear { get; set; }

        public int PublisherId { get; set; }
        public virtual Publisher Publisher { get; set; }
        public int LocationId { get; set; }
        public virtual Location Location { get; set; }
    }
}