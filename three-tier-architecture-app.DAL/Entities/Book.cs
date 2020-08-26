using System.Collections.Generic;

namespace three_tier_architecture_app.DAL.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Author Author { get; set; }
        public ICollection<BookGenre> BookGenres { get; set; }
        public int AuthorId { get; set; }
    }
}