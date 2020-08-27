using System;
using System.Collections.Generic;
using three_tier_architecture_app.DAL.Entities;

namespace Utilities.Builders
{
    public class BookBuilder
    {
        private readonly Book _book;

        public BookBuilder(string bookName = "", int authorId = 1)
        {
            _book = new Book
            {
                Name = string.IsNullOrWhiteSpace(bookName) ? "Default Campaign" : bookName,
                AuthorId = authorId,
                BookGenres = new List<BookGenre>()
            };
        }

        public BookBuilder SetBookGenre()
        {
            
        }
    }
}