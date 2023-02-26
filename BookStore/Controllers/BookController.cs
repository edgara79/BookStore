using BookStore.Business;
using BookStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    public class BookController : Controller
    {
        private string apiURL = string.Empty;
        private readonly IConfiguration _configuration;
        /// <summary>
        /// Class controller
        /// </summary>
        /// <param name="configuration"></param>
        public BookController(IConfiguration configuration) { 
            //apiURL= "https://localhost:44386/api/book";
            _configuration = configuration;
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
            apiURL = builder.GetSection("MyConfiguredValues")["ApiUrl"].ToString();
        }
        // GET: BookController
        public async Task<ActionResult<List<Book>>> Index()
        {
            try
            {
                BusinessLogicBook businessObj = new BusinessLogicBook(apiURL);
                var _bookList = await businessObj.ConnectToAPI();
                return View(_bookList);
            }
            catch
            {
                return null;
            }
            
        }
        /// <summary>
        /// Method to export to CSV file
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<FileContentResult> ExportToCSV()
        {
            BusinessLogicBook businessObj = new BusinessLogicBook(apiURL);
            string csvString = await businessObj.GenerateCSVString();
            char[] result = csvString.ToCharArray();
            var fileName = "BookList_" + DateTime.Today.ToString("MM-dd-yyyy", CultureInfo.InvariantCulture) + ".csv";
            return File(new System.Text.UTF8Encoding().GetBytes(result), "text/csv", fileName);
        }
    }
}
