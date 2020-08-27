using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using three_tier_architecture_app.DAL.Entities;

namespace three_tier_architecture_app.DAL.Data
{
    public class DataContext : DbContext
    {
        private readonly IConfiguration _config;

        public DataContext()
        {
            Database.EnsureCreated();
        }

        public DataContext(DbContextOptions<DataContext> options, IConfiguration config) : base(options)
        {
            _config = config;
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<BookGenre> BookGenres { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Author>().HasData(
                new Author{Id = 1, Firstname = "Лев", Lastname = "Толстой", BirthDate = new DateTime(1828, 08, 09)},
                new Author{Id = 2, Firstname = "Александр", Lastname = "Пушкин", BirthDate = new DateTime(1799, 05, 26)});

            modelBuilder.Entity<Book>().HasData(
                new Book{Id = 1, AuthorId = 1, Name = "Война и мир"},
                new Book{Id = 2, AuthorId = 2, Name = "Евгений Онегин"});


            modelBuilder.Entity<Genre>().HasData(
                new Genre{Id = 1, Name = "Роман"},
                new Genre{Id = 2, Name = "Исторический"},
                new Genre{Id = 3, Name = "Онегинская строфа"},
                new Genre{Id = 4, Name = "Военная проза"});

            modelBuilder.Entity<BookGenre>().HasData(
                new BookGenre{Id = 1, BookId = 1, GenreId = 1},
                new BookGenre{Id = 2, BookId = 1, GenreId = 2},
                new BookGenre{Id = 3, BookId = 1, GenreId = 4},
                new BookGenre{Id = 4, BookId = 2, GenreId = 1},
                new BookGenre{Id = 5, BookId = 2, GenreId = 3}
            );
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseInMemoryDatabase(nameof(DataContext));
            //optionsBuilder.UseSqlServer(_config.GetConnectionString("DefaultConnection"));
        }
    }
}