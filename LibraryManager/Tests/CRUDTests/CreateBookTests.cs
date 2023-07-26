using LibraryManager.Core.Contracts;
using NUnit.Framework;

namespace LibraryManager.Tests.CRUDTests
{
    [TestFixture]
    public class CreateBookTests : TestBase
    {
        //Book should be positive integer
        //Book with this id already exisits
        [Test]
        public async Task CreateBook_IsSuccessful()
        {
            var book = new Book()
            {
                Id = new Random().Next(1, 1000),
                Title = "Test",
                Description = "Test",
                Author = "Test",
            };

            var createBook = await _bookService.CreateBookAsync(book);
            AssertBookProperties(book, createBook);
        }

        [Test]
        public async Task CreateBook_SameId_Error()
        {
            var book = new Book()
            {
                Id = new Random().Next(1, 1000),
                Title = "Test",
                Description = "Test",
                Author = "Test",
            };

            var createBook = await _bookService.CreateBookAsync(book);
            AssertBookProperties(book, createBook);

            var secondBook = new Book()
            {
                Id = book.Id,
                Title = "Test",
                Description = "Test",
                Author = "Test",
            };

            var secondCreateBook = await _bookService.CreateBookAsync(secondBook);
        }
    }
}

