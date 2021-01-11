using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookStore.Data;
using BookStore.Models;
using BookStore.ViewModels;

namespace BookStore.Controllers
{
    public class BooksController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: Books
        public ActionResult Index()
        {
            return View(unitOfWork.BookRepository.GetAll());
        }

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = unitOfWork.BookRepository.FindBy(x => x.Id == (int)id, null, "Authors").FirstOrDefault();
          
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            var model = new EditCreateBookViewModel();
            model.Authors = unitOfWork.AuthorsRepository.GetAll();
            model.Book = new Book();

            return View(model);
            
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EditCreateBookViewModel model)
        {
            if (ModelState.IsValid)
            {
                foreach (string id in model.SelectedAuthors)
                {
                    model.Book.Authors.Add(unitOfWork.AuthorsRepository.GetById(int.Parse(id)));
                }
                unitOfWork.BookRepository.Insert(model.Book);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(model.Book);
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = unitOfWork.BookRepository.FindBy(x => x.Id == (int)id, null, "Authors").FirstOrDefault();
            if (book == null)
            {
                return HttpNotFound();
            }
            EditCreateBookViewModel model = new EditCreateBookViewModel();
            model.Book = book;
            model.Authors.AddRange(unitOfWork.AuthorsRepository.GetAll());
            return View(model);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditCreateBookViewModel model)
        {
            if (ModelState.IsValid)
            {

                //model.Book.Authors.Clear();
                foreach (string a in model.SelectedAuthors)
                {
                    int id = int.Parse(a);
                    model.Book.Authors.Add(unitOfWork.AuthorsRepository.FindBy(x => x.Id == id, null, "").FirstOrDefault());
                }
                unitOfWork.BookRepository.Update(b => b.Id == model.Book.Id, model.Book.Authors, unitOfWork.BookRepository.GetAll(), "Authors");
                unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(model.Book);
        }


        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = unitOfWork.BookRepository.GetById((int)id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = unitOfWork.BookRepository.GetById((int)id);
            unitOfWork.BookRepository.Delete(book);
            unitOfWork.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
