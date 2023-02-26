using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace BooksAPI.Models
{
    public class BookModel
    {
        [Key]
        public string Id { get; set; }
        [Required(ErrorMessage = "Author is a required field")]
        public string Author { get; set; }
        [Required(ErrorMessage = "Title is a required field")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Genre is a required field")]
        public string Genre { get; set; }
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Price field has an invalida value")]
        public decimal Price { get; set; }
        [JsonProperty("publish_date")]
        [Required(ErrorMessage = "Publish date is a required field")]
        public string PublishDateStr { get; set; }
        [JsonIgnore]
        public DateTime PublishDate { get; set; }
        public string Description { get; set; }
    }
}
