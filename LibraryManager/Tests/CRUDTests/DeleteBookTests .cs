using LibraryManager.Core;
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
            var createBook = await _bookService.CreateBook();
            Assert.AreEqual(HttpStatusCode.OK, createBook.StatusCode);

            var deleteBook = await _bookService.DeleteBook(createBook.Success.Id);
            Assert.AreEqual(HttpStatusCode.NoContent, deleteBook.StatusCode);
        }

        [Test]
        public async Task DeleteBook_AlreadyDeleted_NotFound()
        {
            var bookId = new Random().Next(1, 10000);

            var createBook = await _bookService.CreateBook(id: bookId);
            Assert.AreEqual(HttpStatusCode.OK, createBook.StatusCode);

            var deleteBook = await _bookService.DeleteBook(createBook.Success.Id);
            Assert.AreEqual(HttpStatusCode.NoContent, deleteBook.StatusCode);

            var secondDeleteBook = await _bookService.DeleteBook(createBook.Success.Id);
            Assert.IsFalse(secondDeleteBook.IsSuccess);
            Assert.AreEqual(HttpStatusCode.NotFound, secondDeleteBook.StatusCode);
            Assert.AreEqual(string.Format(Constants.NoBookFound, bookId), secondDeleteBook.Error.Message);
        }

        [Test]
        public async Task DeleteBook_NonExistingId_NotFound()
        {
            var nonExistingId = 1234;
            var deleteBook = await _bookService.DeleteBook(nonExistingId);
          
            Assert.IsFalse(deleteBook.IsSuccess);
            Assert.AreEqual(HttpStatusCode.NotFound, deleteBook.StatusCode);
            Assert.AreEqual(string.Format(Constants.NoBookFound, nonExistingId), deleteBook.Error.Message);
        }
    }
}