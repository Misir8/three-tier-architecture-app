using System.Linq;
using AutoMapper;
using three_tier_architecture_app.BLL.Dto;
using three_tier_architecture_app.DAL.Entities;

namespace three_tier_architecture_app.BLL.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Book, BookToReturnDto>()
                .ForMember(x => x.Firstname,
                    o =>
                        o.MapFrom(x => x.Author.Firstname))
                .ForMember(x => x.Lastname,
                    o =>
                        o.MapFrom(x => x.Author.Lastname))
                .ForMember(x => x.BirthDate,
                    o =>
                        o.MapFrom(x => x.Author.BirthDate))
                .ForMember(x => x.Genres,
                    o =>
                        o.MapFrom(x => x.BookGenres
                            .Select(bg => new GenreNameAndIdDto{Name = bg.Genre.Name, GenreId = bg.Genre.Id}).ToList()));

            CreateMap<BookCreateDto, Book>();
        }
    }
}