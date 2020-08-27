using System.Collections.Generic;
using System.Linq;
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

        [HttpDelete("{id}/{genreId}")]
        public async Task<IActionResult> DeleteGenreFromBook(int id, int genreId)
        {
            var bookGenre = _context.BookGenres.FirstOrDefault(x => x.BookId == id && x.GenreId == genreId);
            if (bookGenre == null) return NotFound();
            _context.BookGenres.Remove(bookGenre);
            await _context.SaveChangesAsync();
            return Ok(bookGenre.Id);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var book = _context.Books.FirstOrDefault(x => x.Id == id);
            if (book == null) return NotFound();
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return Ok(book.Id);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BookCreateDto bookCreateDto)
        
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            var book = _mapper.Map<BookCreateDto, Book>(bookCreateDto);
            
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            
            foreach (var genreId in bookCreateDto.GenreIds)
            {
                var bookGenre = new BookGenre {BookId = book.Id, GenreId = genreId};
                await _context.BookGenres.AddAsync(bookGenre);
            }

            await _context.SaveChangesAsync();
            
            return Ok(book);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(BookEditDto bookEditDto)
        {
            //To replace a collection of related data correctly, it must be included in the query
            var book = await _context.Books.Include(b => b.BookGenres)
                .SingleOrDefaultAsync(b => b.Id == bookEditDto.Id);
            //Here you just need to replace the entities with new ones and you're done.
            book.BookGenres = bookEditDto.GenreIds.Select(id => new BookGenre { GenreId = id }).ToList();

            _mapper.Map(bookEditDto, book);
            await _context.SaveChangesAsync();

            return Ok(book);
        }
    }
}