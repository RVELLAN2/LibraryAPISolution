using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using LibraryAPI.Domain;
using LibraryAPI.Models.Books;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Controllers
{
    public class BooksController : ControllerBase
    {
        LibraryDataContext _context;

        IMapper _mapper;
        MapperConfiguration _config;

        public BooksController(LibraryDataContext context, IMapper mapper, MapperConfiguration config)
        {
            _context = context;
            _mapper = mapper;
            _config = config;
        }

        [HttpGet("books")]
        [Produces("application/json")]
        public async Task<ActionResult<GetBooksResponse>> GetAllBooks([FromQuery] string genre = "All")
        {
            var response = new GetBooksResponse();
            var results = _context.Books
                .Where(b => b.RemovedFromInventory == false);
            //       .Select(b => _mapper.Map<GetBooksresponseItem>(b))

            if (genre != "All")
            {
                results = results.Where(b => b.Genre == genre);
            }

            response.Data = await results.ProjectTo<GetBooksresponseItem>(_config).ToListAsync();

            response.Genre = genre;
            response.Count = response.Data.Count;

            return Ok(response);
        }

        /// <summary>
        /// Retrieve a single book.
        /// </summary>
        /// <param name="bookId">The id of the book you want to retrieve</param>
        /// <returns>A book or 404 Not Found</returns>
        [HttpGet("books/{bookId:int}", Name = "books#getbookbyid")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetBookDetailsResponse>> GetBookById(int bookId)
        {
            var response = await _context.Books
                .Where(b => b.RemovedFromInventory == false && b.Id == bookId)
                .ProjectTo<GetBookDetailsResponse>(_config)
                .SingleOrDefaultAsync();

            if (response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }

        }

        [HttpPost("books")]
        public async Task<ActionResult<GetBookDetailsResponse>> AddBook([FromBody] BookCreateRequest bookToAdd)
        {
            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);
            }
            else
            {
                var book = _mapper.Map<Book>(bookToAdd);
                _context.Books.Add(book);
                await _context.SaveChangesAsync();
                var response = _mapper.Map<GetBookDetailsResponse>(book);

                return CreatedAtRoute("books#getbookbyid", new { bookId = response.Id }, response);
            }
        }

        [HttpDelete("books/{bookId:int}")]
        public async Task<ActionResult> RemoveBookFromInventory(int bookId)
        {
            var book = await _context.Books.SingleOrDefaultAsync(b => b.Id == bookId && b.RemovedFromInventory == false);

            if (book != null)
            {
                book.RemovedFromInventory = true;
                await _context.SaveChangesAsync();
            }

            return NoContent();
        }

        [HttpPut("books/{bookId:int}/genre")]
        public async Task<ActionResult> UpdateGenre(int bookId, [FromBody] string newGenre)
        {
            var book = await _context.Books.Where(b => b.Id == bookId).SingleOrDefaultAsync();

            if (book == null)
            {
                return NotFound();
            }
            else
            {
                book.Genre = newGenre;
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }

        [HttpPut("books/{bookId:int}")]
        public async Task<ActionResult> UpdateBook(int bookId, [FromBody] GetBookDetailsResponse book)
        {
            var storedBook = await _context.Books.Where(b => b.Id == bookId && b.RemovedFromInventory == false).SingleOrDefaultAsync();

            if (storedBook == null)
            {
                return NotFound();
            }
            else
            {
                storedBook.Title = book.Title;
                storedBook.Author = book.Author;
                storedBook.Genre = book.Genre;
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }
    }
}
