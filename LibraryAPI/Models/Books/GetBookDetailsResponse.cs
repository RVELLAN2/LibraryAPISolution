using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Models.Books
{
    public class GetBookDetailsResponse
    {
        public int Id { get; set; }
        [Required][MaxLength(200)]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        public string Genre { get; set; }
    }
}
