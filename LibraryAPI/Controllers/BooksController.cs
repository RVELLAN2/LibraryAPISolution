using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryAPI.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Controllers
{
    public class BooksController : ControllerBase
    {
        LibraryDataContext _context;

        public BooksController(LibraryDataContext context)
        {
            _context = context;
        }

        [HttpGet("books")]
        public async Task<ActionResult> GetAllBooks()
        {
            var results = await _context.Books
                .Where(b => b.RemovedFromInventory == false)
                .ToListAsync();

            return Ok(results);
        }
    }
}
