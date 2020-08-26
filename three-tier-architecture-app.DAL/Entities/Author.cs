using System;
using System.Collections.Generic;

namespace three_tier_architecture_app.DAL.Entities
{
    public class Author
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime BirthDate { get; set; }
        public ICollection<Book> Books { get; set; }
        
    }
}