using System.Collections.Generic;

namespace three_tier_architecture_app.BLL.Dto
{
    public class BookEditDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AuthorId { get; set; }
        public List<int> GenreIds { get; set; }
    }
}