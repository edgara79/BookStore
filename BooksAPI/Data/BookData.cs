using BooksAPI.DataAccess;
using BooksAPI.Entity;
using BooksAPI.Models;
using Grpc.Core;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static BooksAPI.Data.BookData;

namespace BooksAPI.Data
{
    public class BookData
    {
        private string _filePath;
        ConnectionDB _oConn = new ConnectionDB();

        public async Task<List<BookModel>> GetBooks(string _accessType)
        {
            if (_accessType == AccessType.DB.ToString())
            {
                var bookList = new List<BookModel>();

                // TODO: Implement database access code

                return bookList;
            }
            else if (_accessType == AccessType.JSON.ToString())
            {
                return (List<BookModel>)await GetDataFromJson();
            }
            else
            {
                throw new ArgumentException("Invalid access type", nameof(_accessType));
            }
        }
        private async Task<List<BookModel>> GetDataFromJson()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            _filePath = builder.GetValue<string>("BooksJsonFilePath");
            string booksJson = await File.ReadAllTextAsync(_filePath);
            // Deserialize the JSON into a list of the book model
            List<BookModel> bookList = JsonConvert.DeserializeObject<List<BookModel>>(booksJson);
            List<ValidationResult> validationResult = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(bookList, new ValidationContext(bookList), validationResult, true);
            if (isValid)
            {
                // Convert the string date to a DateTime object
                foreach (var book in bookList)
                {
                    book.PublishDate = DateTime.ParseExact(book.PublishDateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                }
                
            }
            else
            {
                // Si el modelo no es válido, muestra los errores de validación
                string errorMessage = string.Join(", ", validationResult.Select(r => r.ErrorMessage));
            }
            return bookList;
        }
    }
}
