using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using three_tier_architecture_app.BLL.Dto;
using three_tier_architecture_app.BLL.Helpers;
using three_tier_architecture_app.Controllers;
using three_tier_architecture_app.DAL.Data;
using three_tier_architecture_app.DAL.Entities;
using Xunit;

namespace three_tier_architecture_app.Tests
{
    public class BooksControllerTests:IDisposable
    {
        private DataContext _context;
        private IMapper _mapper;
        private BooksController _controller;
        private Book _book;
        private Author _author;
        private Genre _genre;
        private BookGenre _bookGenre;
        public BooksControllerTests()
        {
            _context = new DataContext();
            _mapper = SetUpMapper();
            _controller = new BooksController(_context, _mapper);
            _author = new Author{ Firstname = "sada", Lastname = "asdas", BirthDate = DateTime.Now};
            _genre = new Genre {Name = "Default Genre"};
            _book = new Book{ Name = "asdas", AuthorId = 1};
            _context.Authors.Add(_author);
            _context.Books.Add(_book);
            _context.Genres.Add(_genre);
            _context.SaveChanges();
            _bookGenre = new BookGenre{BookId = _book.Id, GenreId = _genre.Id};
            _context.BookGenres.Add(_bookGenre);
            _context.SaveChanges();
        }
        [Fact]
        public async Task GetBooksAsync_ReturnOk()
        {
            var result = await _controller.GetBooksAsync() as OkObjectResult;
            var value = result?.Value as IEnumerable<BookToReturnDto>;
            Assert.IsType<OkObjectResult>(result);
            Assert.True(value?.Count() > 0);
        }
        
        [Fact]
        public async Task GetBookByIdAsync_Id_ReturnOk()
        {
            var result = await _controller.GetBookByIdAsync(_book.Id) as OkObjectResult;
            var value = result?.Value as BookToReturnDto;
            Assert.IsType<OkObjectResult>(result);
            Assert.True(value != null);
        }
        
        [Fact]
        public async Task GetBookByIdAsync_Id_ReturnNotFound()
        {
            var result = await _controller.GetBookByIdAsync(-55) as NotFoundResult;
            Assert.IsType<NotFoundResult>(result);
        }
        
        [Fact]
        public async Task Create_bookCreateDto_ReturnOk()
        {
            var bookCreateDto = new BookCreateDto
            {
                AuthorId = _author.Id,
                Name = _book.Name,
                GenreIds = new List<int>{_genre.Id}
            };
            var result = await _controller.Create(bookCreateDto) as OkObjectResult;
            var value = result?.Value as Nullable<int>;

            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<int>(value);
        }


        [Fact]
        public async Task DeleteGenreFromBook_BookIdAndGenreId_ReturnOk()
        {
            var result = await _controller.DeleteGenreFromBook(_bookGenre.BookId, _bookGenre.GenreId) as OkObjectResult;
            var value = result?.Value as Nullable<int>;
            Assert.IsType<OkObjectResult>(result);
            Assert.True(value == _bookGenre.Id);
        }

        [Fact]
        public async Task Delete_Id_ReturnOk()
        {
            var result = await _controller.Delete(_book.Id) as OkObjectResult;
            var value = result?.Value as Nullable<int>;
            Assert.IsType<OkObjectResult>(result);
            Assert.True(value == _book.Id);

            _book = null;
        }


        [Fact]
        public async Task Edit_bookEditDto_ReturnOk()
        {
           
            var editBook = new BookEditDto
            {
                Id = _book.Id,
                AuthorId = _author.Id,
                Name = "llllllllll",
                GenreIds = new List<int>{1, _genre.Id}
            };
            var result = await _controller.Edit(editBook) as OkObjectResult;
            var value = result?.Value as Nullable<int>;

            Assert.IsType<OkObjectResult>(result);
            Assert.True(editBook.Id == value);
        }
        
        
        public void Dispose()
        {
            if (_genre != null) _context.Genres.Remove(_genre);
            
            if (_book != null) _context.Books.Remove(_book);
            
            if (_author != null) _context.Authors.Remove(_author);

            _context.SaveChanges();
        }
        
        
        
        private IMapper SetUpMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfiles>());
            var mapper = new Mapper(config);
            return mapper;
        }
        
    }
}