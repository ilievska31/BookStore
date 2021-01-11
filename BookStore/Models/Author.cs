using System;
using System.Collections.Generic;

namespace BookStore.Models
{
    public class Author
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Biography { get; set; }
        public List<Book> Books { get; set; }
    }
}