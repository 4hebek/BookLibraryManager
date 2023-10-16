using LibraryManager.Core.Contracts;
using NUnit.Framework;

namespace LibraryManager.Tests.CRUDTests
{
    [TestFixture]
    public class UpdateBookTests : TestBase
    {
        [Test]
        public async Task UpdateBook_IsSuccessful()
        {
            var book = new Book()
            {
                Id = new Random().Next(1, 1000),
                Title = "Test",
                Description = "Test",
                Author = "Test",
            };

            var createBook = await _bookService.CreateBookAsync(book);

            var updatedBook = new Book()
            {
                Id = book.Id,
                Title = "TestUpdated",
                Description = "TestUpdated",
                Author = "TestUpdated",
            };

            var updateBook = await _bookService.UpdateBookAsync(createBook.Success.Id, updatedBook);
            Assert.IsNotNull(updateBook, "Book with this id was not found");
            AssertBookProperties(updatedBook, updateBook);
        }
    }
}
