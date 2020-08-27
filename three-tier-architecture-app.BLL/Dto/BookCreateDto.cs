using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace three_tier_architecture_app.BLL.Dto
{
    public class BookCreateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AuthorId { get; set; }
        public List<int> GenreIds { get; set; }
    }
}