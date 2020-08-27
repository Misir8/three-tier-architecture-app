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
    public class GenresControllerTests:IDisposable
    {
        private DataContext _context;
        private IMapper _mapper;
        private GenresController _controller;
        private Book _book;
        private Author _author;
        private Genre _genre;
        private BookGenre _bookGenre;
        
        public GenresControllerTests()
        {
            _context = new DataContext();
            _mapper = SetUpMapper();
            _controller = new GenresController(_context, _mapper);
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
        public async Task GetGenresAsync_ReturnOk()
        {
            var result = await _controller.GetGenresAsync() as OkObjectResult;
            var value = result.Value as IEnumerable<GenreToReturnDto>;
            Assert.IsType<OkObjectResult>(result);
            Assert.True(value.Count() > 0);
        }
        
        [Fact]
        public async Task GetGenreByIdAsync_ReturnOk()
        {
            var result = await _controller.GetGenreByIdAsync(_genre.Id) as OkObjectResult;
            var value = result.Value as GenreToReturnDto;
            Assert.IsType<OkObjectResult>(result);
            Assert.True(value.Id == _genre.Id);
            Assert.True(value.Name == _genre.Name);
        }

        [Fact]
        public async Task CreateAsync_genreCreateDto_ReturnOk()
        {
            var genreCreateDto = new GenreCreateDto
            {
                Name = "new genre",
                BooksId = new List<int>{_book.Id}
            };
            var result = await _controller.CreateAsync(genreCreateDto) as OkObjectResult;
            var value = result.Value as Nullable<int>;
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<int>(value);
        }

        [Fact]
        public async Task Delete_id_ReturnOk()
        {
            var result = await _controller.Delete(_genre.Id) as OkObjectResult;
            var value = result.Value as Nullable<int>;
            Assert.IsType<OkObjectResult>(result);
            Assert.True(value == _genre.Id);
            _genre = null;
        }

        [Fact]
        public async Task Edit_GenreEditDto_ReturnOk()
        {
            var genreEditDto = new GenreEditDto
            {
                Id = _genre.Id,
                Name = "LLLLLLLLLLLLLLLLL",
                BooksId = new List<int>{_book.Id}
            };
            var result = await _controller.Edit(genreEditDto) as OkObjectResult;
            var value = result.Value as Nullable<int>;
            Assert.IsType<OkObjectResult>(result);
            Assert.True(value == genreEditDto.Id);
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