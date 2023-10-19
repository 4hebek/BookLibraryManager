using LibraryManager.Core;

namespace LibraryManager.Tests.CRUDTests
{
    [TestFixture]
    public class GetBookTests : TestBase
    {
        [Test]
        public async Task GetBookById_Ok() 
        {
            var bookId = new Random().Next(1, 10000);
            var createBook = await _bookService.CreateBook(id: bookId);
            createBook.StatusCode.Should().Be(HttpStatusCode.OK);

            var getBook = await _bookService.GetBookById(createBook.Success.Id);
            getBook.Should().NotBeNull();
            getBook.StatusCode.Should().Be(HttpStatusCode.OK);
            AssertBookProperties(createBook.Success, getBook.Success);
        }

        [Test]
        public async Task GetBookById_AfterDeletion_NotFound()
        {
            var bookId = new Random().Next(1, 1000);
            var errorMessage = string.Format(Constants.NoBookFound, bookId);

            var createBook = await _bookService.CreateBook(id: bookId);
            createBook.StatusCode.Should().Be(HttpStatusCode.OK);

            var deleteBook = await _bookService.DeleteBook(createBook.Success.Id);
            deleteBook.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var getBook = await _bookService.GetBookById(createBook.Success.Id);
            getBook.IsSuccess.Should().BeFalse();
            getBook.StatusCode.Should().Be(HttpStatusCode.NotFound);
            getBook.Error.Message.Should().Be(errorMessage);
        }

        [Test]
        public async Task GetBookById_DoesNotExist_NotFound()
        {
            var nonExistingId = 107678;
            var errorMessage = string.Format(Constants.NoBookFound, nonExistingId);
            var getBook = await _bookService.GetBookById(nonExistingId);

            getBook.IsSuccess.Should().BeFalse();
            getBook.StatusCode.Should().Be(HttpStatusCode.NotFound);
            getBook.Error.Message.Should().Be(errorMessage);
        }     
    }
}
