using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using three_tier_architecture_app.BLL.Dto;
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
        public BooksControllerTests()
        {
            _context = new DataContext();
            _mapper = SetUpMapper();
            _controller = new BooksController(_context, _mapper);
            //_author = new Author{Id = 1, Firstname = "sada", Lastname = "asdas", BirthDate = DateTime.Now};
            //_book = new Book{Id = 1, Name = "asdas", AuthorId = 1};
            //_context.Authors.Add(_author);
            //_context.Books.Add(_book);
            //_context.SaveChanges();
        }
        [Fact]
        public async Task GetBooksAsync_ReturnOk()
        {
            var result = await _controller.GetBooksAsync() as OkObjectResult;
            var value = result?.Value as IEnumerable<BookToReturnDto>;
            Assert.IsType<OkObjectResult>(result);
            Assert.True(value?.Count() > 0);
        }

        public void Dispose()
        {
            
        }
        
        private IMapper SetUpMapper()
        {
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<Book, BookToReturnDto>().ReverseMap());
            return new Mapper(mapperConfiguration);
        }

    }
}