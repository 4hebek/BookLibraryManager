using LibraryManager.Core.Contracts;
using NUnit.Framework;
using System.Net;

namespace LibraryManager.Tests.CRUDTests
{
    [TestFixture]
    public class CreateBookTests : TestBase
    {
        //Book should be positive integer

        [Test]
        // This test fails because the Author is not being recorded and is not as expected
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
            Assert.AreEqual(HttpStatusCode.OK, createBook.StatusCode);
            AssertBookProperties(book, createBook.Success);
        }

        [Test]
        // I believe this test should error as it should be impossible to create a book with the same id but different properties
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

            var secondBook = new Book()
            {
                Id = book.Id,
                Title = "Second book",
                Description = "Comedy",
                Author = "Author 1",
            };

            var secondCreateBook = await _bookService.CreateBookAsync(secondBook);
            Assert.IsFalse(createBook.IsSuccess);
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public async Task CreateBook_WithoutAuthor_BadRequest(string author)
        {
            var book = new Book()
            {
                Id = new Random().Next(1, 1000),
                Title = "Test",
                Description = "Test",
                Author = author,
            };

            var createBook = await _bookService.CreateBookAsync(book);
            Assert.IsFalse(createBook.IsSuccess);
            Assert.AreEqual(HttpStatusCode.BadRequest, createBook.StatusCode);
            Assert.AreEqual("Book.Author is a required field.\r\nParameter name: book.Author", createBook.Error.Message);
        }

        [Test]
        [TestCase(-1)]
        [TestCase(0)]
        public async Task CreateBook_InvalidId_BadRequest(int bookId)
        {
            var book = new Book()
            {
                Id = bookId,
                Title = "Test",
                Description = "Test",
                Author = "Test",
            };

            var createBook = await _bookService.CreateBookAsync(book);
            Assert.IsFalse(createBook.IsSuccess);
            Assert.AreEqual(HttpStatusCode.BadRequest, createBook.StatusCode);
            Assert.AreEqual("Book.Id should be a positive integer!\r\nParameter name: book.Id", createBook.Error.Message);
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public async Task CreateBook_InvalidTitle_BadRequest(string title)
        {
            var book = new Book()
            {
                Id = new Random().Next(1, 1000),
                Title = title,
                Description = "Test",
                Author = "Test",
            };

            var createBook = await _bookService.CreateBookAsync(book);
            Assert.IsFalse(createBook.IsSuccess);
            Assert.AreEqual(HttpStatusCode.BadRequest, createBook.StatusCode);
            Assert.AreEqual("Book.Title is a required field\r\nParameter name: Book.Title", createBook.Error.Message);
        }

        [Test]
        // This test fails because the Author is not being recorded and is not as expected
        public async Task CreateBook_WithoutDescription_OK()
        {
            var book = new Book()
            {
                Id = new Random().Next(1, 1000),
                Title = "Title",
                Description = null,
                Author = "Test",
            };

            var createBook = await _bookService.CreateBookAsync(book);
            Assert.AreEqual(HttpStatusCode.OK, createBook.StatusCode);
            AssertBookProperties(book, createBook.Success);
        }

        [Test]
        public async Task CreateBook_WithoutAuthor_BadRequest()
        {
            var book = new Book()
            {
                Id = new Random().Next(1, 1000),
                Title = "Title",
                Description = "Description",
                Author = null,
            };

            var createBook = await _bookService.CreateBookAsync(book);
            Assert.IsFalse(createBook.IsSuccess);
            Assert.AreEqual(HttpStatusCode.BadRequest, createBook.StatusCode);
            Assert.AreEqual("Book.Author is a required field.\r\nParameter name: book.Author", createBook.Error.Message);
        }

        [Test]
        // This also fails when char length is 100 but shouldn't as that's the max allowed according to the error message
        public async Task CreateBook_TitleMaxCharacters_BadRequest()
        {
            var book = new Book()
            {
                Id = new Random().Next(1, 1000),
                Title = GenerateRandomString(101),
                Description = "Description",
                Author = "Author",
            };

            var createBook = await _bookService.CreateBookAsync(book);
            Assert.IsFalse(createBook.IsSuccess);
            Assert.AreEqual(HttpStatusCode.BadRequest, createBook.StatusCode);
            Assert.AreEqual("Book.Title should not exceed 100 characters!\r\nParameter name: Book.Title", createBook.Error.Message);
        }

        [Test]
        // This throws a BadRequest when char length is 30 but shouldn't as that's the max allowed according to the error message
        public async Task CreateBook_AuthorMaxCharacters_BadRequest()
        {
            var book = new Book()
            {
                Id = new Random().Next(1, 1000),
                Title = "Title",
                Description = "Description",
                Author = GenerateRandomString(30),
            };

            var createBook = await _bookService.CreateBookAsync(book);
            Assert.IsFalse(createBook.IsSuccess);
            Assert.AreEqual(HttpStatusCode.BadRequest, createBook.StatusCode);
            Assert.AreEqual("Book.Author should not exceed 30 characters!\r\nParameter name: Book.Author", createBook.Error.Message);
        }
    }
}

