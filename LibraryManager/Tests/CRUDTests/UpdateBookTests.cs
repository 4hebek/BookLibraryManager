using LibraryManager.Core;
using LibraryManager.Core.Contracts;
using NUnit.Framework;
using System.Net;

namespace LibraryManager.Tests.CRUDTests
{
    [TestFixture]
    public class UpdateBookTests : TestBase
    {
        [Test]
        public async Task UpdateBook_Ok()
        {
            var bookId = new Random().Next(1, 10000);
            var createBook = await _bookService.CreateBook(id: bookId);

            var updatedBook = new Book()
            {
                Id = bookId,
                Title = "TestUpdated",
                Description = "TestUpdated",
                Author = "TestUpdated",
            };

            var updateBook = await _bookService.UpdateBook(createBook.Success.Id, updatedBook);
            Assert.IsNotNull(updateBook, "Book with this id was not found");
            Assert.AreEqual(HttpStatusCode.OK, updateBook.StatusCode);
            AssertBookProperties(updatedBook, updateBook.Success);
        }

        [Test]
        public async Task UpdateBooksId_BadRequest()
        {
            var bookId = new Random().Next(1, 1000);
            var createBook = await _bookService.CreateBook(id: bookId);

            var updatedBook = new Book()
            {
                Id = new Random().Next(1, 10000),
                Title = "Test",
                Description = "Test",
                Author = "Test",
            };

            var updateBook = await _bookService.UpdateBook(createBook.Success.Id, updatedBook);
            Assert.IsFalse(updateBook.IsSuccess);
            Assert.AreEqual(HttpStatusCode.BadRequest, updateBook.StatusCode);
            Assert.AreEqual(string.Format(Constants.BookIdCantBeUpdated, bookId), updateBook.Error.Message);
        }
    }
}
