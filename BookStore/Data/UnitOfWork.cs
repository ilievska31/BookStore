using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Data
{
    public class UnitOfWork : IDisposable
    {
        private BookStoreContext context = new BookStoreContext();
        private Repository<Book> bookRepository;
        private Repository<Author> authorsRepository;


        public Repository<Book> BookRepository
        {
            get 
            {
                if (this.bookRepository == null)
                {
                    this.bookRepository = new Repository<Book>(context);
                }
                return this.bookRepository;
            }
        }

        public Repository<Author> AuthorsRepository
        {
            get
            {
                if (this.authorsRepository == null)
                {
                    this.authorsRepository = new Repository<Author>(context);
                }
                return this.authorsRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }
        private bool isDisposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.isDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
