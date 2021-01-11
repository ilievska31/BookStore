using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.ViewModels
{
    public class EditCreateBookViewModel
    {
        public List<Author> Authors { get; set; }
        public Book Book { get; set; }
        public List<string> SelectedAuthors { get; set; }

        public EditCreateBookViewModel() {
            this.SelectedAuthors = new List<string>();
            this.Authors = new List<Author>();
        }
    }
}