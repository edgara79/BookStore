using System.Collections.Generic;

namespace BookStore.Shared
{
    public class ValidationError
    {
        public string Field { get; set; }
        public List<string> Errors { get; set; }
    }
}
