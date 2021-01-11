using BookStore.Data;
using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        private UnitOfWork unitOfWork = new UnitOfWork();

        public ActionResult Search(String searchTerm) 
        {

            List<Book> searchResults = new List<Book>();
            searchResults.AddRange(unitOfWork.BookRepository.FindBy(x => x.Title.Contains(searchTerm)));
            List<Author> authors = unitOfWork.AuthorsRepository.FindBy(x => x.Name.Contains(searchTerm),null,"Books");

            if (authors != null) {

                authors.ForEach(x => searchResults.AddRange(x.Books));
            }

            return View(searchResults);
        }
    }
}