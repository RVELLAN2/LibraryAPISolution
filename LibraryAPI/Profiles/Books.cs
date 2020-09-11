using AutoMapper;
using LibraryAPI.Domain;
using LibraryAPI.Models.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Profiles
{
    public class Books : Profile
    {
        public Books()
        {
            CreateMap<Book, GetBooksresponseItem>();
            CreateMap<Book, GetBookDetailsResponse>();

            CreateMap<BookCreateRequest, Book>()
                .ForMember(dest => dest.RemovedFromInventory, d => d.MapFrom(_ => false));
        }
    }
}
