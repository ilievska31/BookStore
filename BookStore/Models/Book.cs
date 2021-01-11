using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class Book
    {
        public int Id { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public int Count { get; set; }
        public String ImageUrl { get; set; }
        public List<Author> Authors { get; set; }

        public Book() {
            this.Authors = new List<Author>();
        }
    }
}