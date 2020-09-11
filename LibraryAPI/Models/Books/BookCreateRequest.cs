using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Models.Books
{
    public class BookCreateRequest : IValidatableObject
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public string Genre { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(Title.ToLower() == "it" && Author.ToLower() == "king")
            {
                yield return new ValidationResult("This book is too darned long", new string[] { "Title", "Author" });
            }
        }
    }
}
