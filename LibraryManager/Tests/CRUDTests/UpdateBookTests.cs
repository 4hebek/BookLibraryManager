using FluentAssertions;
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
            updateBook.IsSuccess.Should().BeTrue();
            updateBook.StatusCode.Should().Be(HttpStatusCode.OK);
            AssertBookProperties(updatedBook, updateBook.Success);
        }

        [Test]
        public async Task UpdateBooksId_BadRequest()
        {
            var bookId = new Random().Next(1, 1000);
            var errorMessage = string.Format(Constants.BookIdCantBeUpdated, bookId);
            var createBook = await _bookService.CreateBook(id: bookId);

            var updatedBook = new Book()
            {
                Id = new Random().Next(1, 10000),
                Title = "Test",
                Description = "Test",
                Author = "Test",
            };

            var updateBook = await _bookService.UpdateBook(createBook.Success.Id, updatedBook);
            updateBook.IsSuccess.Should().BeFalse();
            updateBook.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            updateBook.Error.Message.Should().Be(errorMessage);
        }
    }
}
