using System.Collections.Generic;

namespace three_tier_architecture_app.BLL.Dto
{
    public class GenreCreateDto
    {
        public string Name { get; set; }
        public List<int> BooksId { get; set; }
    }
}