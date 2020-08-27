using System.Collections.Generic;

namespace three_tier_architecture_app.BLL.Dto
{
    public class GenreToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Books { get; set; }
    }
}