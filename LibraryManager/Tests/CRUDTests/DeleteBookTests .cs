using FluentAssertions;
using LibraryManager.Core;
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
            createBook.StatusCode.Should().Be(HttpStatusCode.OK);

            var deleteBook = await _bookService.DeleteBook(createBook.Success.Id);
            deleteBook.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Test]
        public async Task DeleteBook_AlreadyDeleted_NotFound()
        {
            var bookId = new Random().Next(1, 10000);
            var errorMessage = string.Format(Constants.NoBookFound, bookId);
            var createBook = await _bookService.CreateBook(id: bookId);
            createBook.StatusCode.Should().Be(HttpStatusCode.OK);

            var deleteBook = await _bookService.DeleteBook(createBook.Success.Id);
            deleteBook.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var secondDeleteBook = await _bookService.DeleteBook(createBook.Success.Id);
            secondDeleteBook.IsSuccess.Should().BeFalse();
            secondDeleteBook.StatusCode.Should().Be(HttpStatusCode.NotFound);
            secondDeleteBook.Error.Message.Should().Be(errorMessage);
        }

        [Test]
        public async Task DeleteBook_NonExistingId_NotFound()
        {
            var nonExistingId = 1234;
            var errorMessage = string.Format(Constants.NoBookFound, nonExistingId);
            var deleteBook = await _bookService.DeleteBook(nonExistingId);

            deleteBook.IsSuccess.Should().BeFalse();
            deleteBook.StatusCode.Should().Be(HttpStatusCode.NotFound);
            deleteBook.Error.Message.Should().Be(errorMessage);
        }
    }
}