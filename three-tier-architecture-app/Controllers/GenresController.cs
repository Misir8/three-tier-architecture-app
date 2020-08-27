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
    public class GenresController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public GenresController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        // GET
        [HttpGet]
        public async Task<IActionResult> GetGenresAsync()
        {
            var genres = await _context.Genres.Include(x => x.BookGenres)
                .ThenInclude(x => x.Book).ToListAsync();
            var mapGenres = _mapper.Map<IEnumerable<Genre>, IEnumerable<GenreToReturnDto>>(genres);
            return Ok(mapGenres);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGenreByIdAsync(int id)
        {
            var genre = await _context.Genres.Include(x => x.BookGenres)
                .ThenInclude(x => x.Book).FirstOrDefaultAsync(x => x.Id == id);
            if (genre == null) return NotFound();
            var mapGenre = _mapper.Map<Genre, GenreToReturnDto>(genre);
            return Ok(mapGenre);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(GenreCreateDto genreCreateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            var genre = _mapper.Map<GenreCreateDto, Genre>(genreCreateDto);
            
            await _context.Genres.AddAsync(genre);
            await _context.SaveChangesAsync();
            
            foreach (var bookId in genreCreateDto.BooksId)
            {
                var bookGenre = new BookGenre {BookId = bookId, GenreId = genre.Id};
                await _context.BookGenres.AddAsync(bookGenre);
            }

            await _context.SaveChangesAsync();
            
            return Ok(genre.Id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(x => x.Id == id);
            if (genre == null) return NotFound();
            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();
            return Ok(genre.Id);
        }
        
        [HttpPut]
        public async Task<IActionResult> Edit(GenreEditDto genreEditDto)
        {
            var genre = await _context.Genres.Include(b => b.BookGenres)
                .SingleOrDefaultAsync(b => b.Id == genreEditDto.Id);
            if (genre == null) return NotFound();
            genre.BookGenres = genreEditDto.BooksId.Select(id => new BookGenre { BookId = id }).ToList();

            _mapper.Map(genreEditDto, genre);
            await _context.SaveChangesAsync();

            return Ok(genre.Id);
        }
    }
}