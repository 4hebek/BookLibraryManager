using LibraryManager.Core;
using LibraryManager.Core.Contracts;

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

        [Test]
        // Bug: This throws a BadRequest when char length is 100 but shouldn't as that's the max allowed according to the error message
        public async Task UpdateBook_TitleMaxCharacters_BadRequest()
        {
            var bookId = new Random().Next(1, 10000);
            var createBook = await _bookService.CreateBook(id: bookId);

            var updatedBook = new Book()
            {
                Id = bookId,
                Title = GenerateRandomString(101),
                Description = "TestUpdated",
                Author = "TestUpdated",
            };

            var updateBook = await _bookService.UpdateBook(createBook.Success.Id, updatedBook);
            updateBook.IsSuccess.Should().BeFalse();
            updateBook.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            updateBook.Error.Message.Should().Be(Constants.TitleMaxCharacters);
        }

        [Test]
        // Bug: This throws a BadRequest when char length is 30 but shouldn't as that's the max allowed according to the error message
        public async Task UpdateBook_AuthorMaxCharacters_BadRequest()
        {
            var bookId = new Random().Next(1, 10000);
            var createBook = await _bookService.CreateBook(id: bookId);

            var updatedBook = new Book()
            {
                Id = bookId,
                Title = "TestUpdated",
                Description = "TestUpdated",
                Author = GenerateRandomString(31)
            };

            var updateBook = await _bookService.UpdateBook(createBook.Success.Id, updatedBook);
            updateBook.IsSuccess.Should().BeFalse();
            updateBook.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            updateBook.Error.Message.Should().Be(Constants.AuthorMaxCharacters);
        }
    }
}
