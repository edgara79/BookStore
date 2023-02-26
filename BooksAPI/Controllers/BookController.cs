using BooksAPI.Data;
using BooksAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace BooksAPI.Controllers
{
    [ApiController]
    [Route("api/book")]
    public class BookController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public BookController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public async Task<ActionResult<List<BookModel>>> Get()
        {
            try
            {
                var bookObj = new BookData();
                var bookList = await bookObj.GetBooks("JSON");
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                return Ok(bookList);
            }
            catch(JsonReaderException ex)
            {
                return BadRequest(string.Format("Data source is corrupted or bad formed, give this information to your administrator: {0}", ex.InnerException));
            }
            catch(WebException ex)
            {
                return NotFound(ex);
            }
            
        }
    }
}
