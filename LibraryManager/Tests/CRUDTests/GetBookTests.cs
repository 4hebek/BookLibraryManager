using LibraryManager.Core;
using LibraryManager.Core.Contracts;
using NUnit.Framework;
using System.Net;

namespace LibraryManager.Tests.CRUDTests
{
    [TestFixture]
    public class GetBookTests : TestBase
    {
        [Test]
        public async Task GetBookById_Ok() 
        {
            var book = new Book()
            {
                Id = new Random().Next(1, 1000),
                Title = "Test",
                Description = "Test",
                Author = "Test",
            };

            var createBook = await _bookService.CreateBook(book);
            Assert.AreEqual(HttpStatusCode.OK, createBook.StatusCode);

            var getBook = await _bookService.GetBookById(createBook.Success.Id);
            Assert.IsNotNull(getBook, "Book with this id was not found");
            Assert.AreEqual(HttpStatusCode.OK, getBook.StatusCode);
            AssertBookProperties(createBook.Success, getBook.Success);
        }

        [Test]
        public async Task GetBookById_AfterDeletion_NotFound()
        {
            var book = new Book()
            {
                Id = new Random().Next(1, 1000),
                Title = "Test",
                Description = "Test",
                Author = "Test",
            };

            var createBook = await _bookService.CreateBook(book);
            Assert.AreEqual(HttpStatusCode.OK, createBook.StatusCode);

            var deleteBook = await _bookService.DeleteBookAsync(createBook.Success.Id);
            Assert.AreEqual(HttpStatusCode.NoContent, deleteBook.StatusCode);

            var getBook = await _bookService.GetBookById(createBook.Success.Id);
            Assert.IsFalse(getBook.IsSuccess);
            Assert.AreEqual(HttpStatusCode.NotFound, getBook.StatusCode);
            Assert.AreEqual(string.Format(Constants.NoBookFound, book.Id), getBook.Error.Message);
        }

        [Test]
        public async Task GetBookById_DoesNotExist_NotFound()
        {
            var nonExistingId = 107678;
            var getBook = await _bookService.GetBookById(nonExistingId);

            Assert.IsFalse(getBook.IsSuccess);
            Assert.AreEqual(HttpStatusCode.NotFound, getBook.StatusCode);
            Assert.AreEqual(string.Format(Constants.NoBookFound, nonExistingId), getBook.Error.Message);
        }     
    }
}
