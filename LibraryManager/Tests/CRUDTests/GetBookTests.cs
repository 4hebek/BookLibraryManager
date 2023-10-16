using LibraryManager.Core.Contracts;
using NUnit.Framework;

namespace LibraryManager.Tests.CRUDTests
{
    [TestFixture]
    public class GetBookTests : TestBase
    {
        [Test]
        public async Task GetBook_IsSuccessful() 
        {
            var book = new Book()
            {
                Id = new Random().Next(1, 1000),
                Title = "Test",
                Description = "Test",
                Author = "Test",
            };

            var createBook = await _bookService.CreateBookAsync(book);

            var getBook = await _bookService.GetBookAsync(createBook.Success.Id);
            Assert.IsNotNull(getBook, "Book with this id was not found");
            AssertBookProperties(createBook.Success, getBook);
        }
    }
}
