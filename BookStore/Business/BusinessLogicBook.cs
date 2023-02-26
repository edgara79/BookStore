using BookStore.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Net;
using BookStore.Shared;

namespace BookStore.Business
{
    public class BusinessLogicBook
    {
        private string _apiURL = string.Empty;
        public BusinessLogicBook(string apiURL)
        {
            _apiURL= apiURL;
        }
        /// <summary>
        /// Method to generate the CSV data
        /// </summary>
        /// <returns></returns>
        public async Task<string> GenerateCSVString()
        {
            var bookList = await ConnectToAPI();
            StringBuilder sb = new StringBuilder();
            // Adding the CSV header
            sb.AppendLine("Author,Title,Genre,Price,PublishDate,Description");
            // Adding each book in the CSV file
            foreach (Book bookItem in bookList.OrderBy(x => x.Title))
            {
                //Implement a replace for the commas to avoid issues when the data is putted into the file
                sb.AppendLine($"{bookItem.Author.Replace(",", " ")},{bookItem.Title.Replace(",", " ")},{bookItem.Genre.Replace(",", " ")},{bookItem.Price},{bookItem.PublishDate:yyyy-MM-dd},{bookItem.Description.Replace(",", " ")}");
            }
            sb.Append("\r\n");
            return sb.ToString();
        }
        /// <summary>
        /// Method to connect with the API
        /// </summary>
        /// <returns></returns>
        public async Task<List<Book>> ConnectToAPI()
        {
            var httpClient = new HttpClient();
            try
            {
                var bookResponse = await httpClient.GetAsync(_apiURL);
                bookResponse.EnsureSuccessStatusCode();
                if (bookResponse.StatusCode == HttpStatusCode.OK)
                {
                    var booksJson = await bookResponse.Content.ReadAsStringAsync();
                    var finalBookList = FilterBookInformation(JsonConvert.DeserializeObject<Book[]>(booksJson));
                    return finalBookList;
                }
                else if(bookResponse.StatusCode == HttpStatusCode.BadRequest)
                {
                    var booksJson = await bookResponse.Content.ReadAsStringAsync();
                    var errortList = JsonConvert.DeserializeObject<List<ValidationError>>(booksJson);
                    //TODO: log error in a designed repository (EventLog, DB or other needed)
                }
                else
                {
                    //TODO: log error in a designed repository (EventLog, DB or other needed)
                    //TODO: Implement code to manage other error type
                    throw new Exception(bookResponse.StatusCode.ToString());
                }
                return null;
            }
            catch (Exception ex)
            {
                //TODO: log error in a designed repository (EventLog, DB or other needed)
                return null;
            }
            finally
            {
                // Dispose of the HttpClient instance
                httpClient.Dispose();
            }
        }
        /// <summary>
        /// Method to validate the information obtained from the API and filtered it according the requirements
        /// </summary>
        /// <param name="bookList">Book list data</param>
        /// <returns></returns>
        private List<Book> FilterBookInformation(Book[] bookList)
        {
            // Create a new list to hold the filtered books
            var filteredBooks = bookList.Where(book =>
            {
                // Check if the book was published on a Sunday or if the author name contains "shrub"
                if (book.PublishDate.DayOfWeek != DayOfWeek.Sunday && !book.Author.ToLower().Contains("shrub"))
                {
                    // Round up the price to up
                    decimal roundedPrice = Math.Ceiling(book.Price);
                    book.Price = roundedPrice;
                    return true;
                }
                return false;
            }
            ).ToList();
            return filteredBooks;
        }
    }
}
