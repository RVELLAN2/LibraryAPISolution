using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace LibraryAPI.Models.Books
{
    public class GetBooksResponse : Collection<GetBooksresponseItem>
    {
        public string Genre { get; set; }
        public int Count { get; set; }
    }

    public class GetBooksresponseItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
    }
}
