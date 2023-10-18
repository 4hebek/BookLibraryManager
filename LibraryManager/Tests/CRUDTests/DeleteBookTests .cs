﻿using LibraryManager.Core;
using LibraryManager.Core.Contracts;
using NUnit.Framework;
using System.Net;

namespace LibraryManager.Tests.CRUDTests
{
    [TestFixture]
    public class DeleteBookTests : TestBase
    {
        [Test]
        public async Task DeleteBook_NoContent()
        {
            var book = new Book()
            {
                Id = new Random().Next(1, 1000),
                Title = "Test",
                Description = "Test",
                Author = "Test",
            };

            var createBook = await _bookService.CreateBook(book);

            var deleteBook = await _bookService.DeleteBookAsync(createBook.Success.Id);
            Assert.AreEqual(HttpStatusCode.NoContent, deleteBook.StatusCode);
        }

        [Test]
        public async Task DeleteBook_AlreadyDeleted_NotFound()
        {
            var book = new Book()
            {
                Id = new Random().Next(1, 1000),
                Title = "Test",
                Description = "Test",
                Author = "Test",
            };

            var createBook = await _bookService.CreateBook(book);

            var deleteBook = await _bookService.DeleteBookAsync(createBook.Success.Id);
            Assert.AreEqual(HttpStatusCode.NoContent, deleteBook.StatusCode);

            var secondDeleteBook = await _bookService.DeleteBookAsync(createBook.Success.Id);
            Assert.IsFalse(secondDeleteBook.IsSuccess);
            Assert.AreEqual(HttpStatusCode.NotFound, secondDeleteBook.StatusCode);
            Assert.AreEqual(string.Format(Constants.NoBookFound, book.Id), secondDeleteBook.Error.Message);
        }
    }
}