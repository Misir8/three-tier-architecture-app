namespace three_tier_architecture_app.DAL.Entities
{
    public class BookGenre
    {
        public int Id { get; set; }
        public Book Book { get; set; }
        public Genre Genre { get; set; }
        public int BookId { get; set; }
        public int GenreId { get; set; }
    }
}