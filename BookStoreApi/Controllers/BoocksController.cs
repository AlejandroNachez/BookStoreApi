using BookStoreApi.Models;
using BookStoreApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BoocksController : ControllerBase
    {
        private readonly BooksService _booksService;

        public BoocksController(BooksService booksService) => _booksService = booksService;

        [HttpGet]
        public async Task<List<Book>> Get() =>
            await _booksService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Book>> Get(string id)
        {
            var book = await _booksService.GetAsync(id);

            if (book is null) {
                return NotFound();
            }

            return book;
        }

        [HttpPost]
        public async Task<ActionResult> Post(Book newBook) { 
            await _booksService.CreateAsync(newBook);

            return CreatedAtAction(nameof(Get), new { id = newBook.Id, newBook });
        
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Book updateBook)
        { 
            var book = await _booksService.GetAsync(id);
            if (book is null) {
                return NotFound();
            }

            updateBook.Id = book.Id;

            await _booksService.UpdateAsync(id, updateBook);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        { 
            var book = await _booksService.GetAsync(id);

            if (book is null)
            {
                return NotFound();
            }

            await _booksService.RemoveAsync(id);

            return NoContent();
        
        }


    }
}
