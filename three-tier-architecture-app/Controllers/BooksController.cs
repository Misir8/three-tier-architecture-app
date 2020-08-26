using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using three_tier_architecture_app.BLL.Dto;
using three_tier_architecture_app.DAL.Data;
using three_tier_architecture_app.DAL.Entities;

namespace three_tier_architecture_app.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public BooksController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        // GET
        [HttpGet]
        public async Task<IActionResult> GetBooksAsync()
        {
            var books = await _context.Books.Include(x => x.Author)
                .Include(x => x.BookGenres)
                .ThenInclude(x => x.Genre).ToListAsync();

            var mappingBooks = _mapper.Map<IEnumerable<Book>, IEnumerable<BookToReturnDto>>(books);
            
            return Ok(mappingBooks);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookByIdAsync(int id)
        {
            var book = await _context.Books.Include(x => x.Author)
                .Include(x => x.BookGenres)
                .ThenInclude(x => x.Genre).FirstOrDefaultAsync(x => x.Id == id);
            if (book == null) return NotFound();
            var mappingBook = _mapper.Map<Book, BookToReturnDto>(book);
            
            return Ok(mappingBook);
        }
    }
}