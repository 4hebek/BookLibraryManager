using LibraryManager.Core;
using LibraryManager.Core.Contracts;
using NUnit.Framework;
using System.Net;

namespace LibraryManager.Tests.CRUDTests
{
    [TestFixture]
    public class CreateBookTests : TestBase
    {
        [Test]
        public async Task CreateBook_Ok()
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
            AssertBookProperties(book, createBook.Success);
        }

        [Test]
        public async Task CreateBook_SameId_BadRequest()
        {
            var book = new Book()
            {
                Id = new Random().Next(1, 1000),
                Title = "Test",
                Description = "Test",
                Author = "Test",
            };

            var createBook = await _bookService.CreateBook(book);

            var secondBook = new Book()
            {
                Id = book.Id,
                Title = "Second book",
                Description = "Comedy",
                Author = "Author 1",
            };

            var secondCreateBook = await _bookService.CreateBook(secondBook);
            Assert.IsFalse(secondCreateBook.IsSuccess);
            Assert.AreEqual(HttpStatusCode.BadRequest, secondCreateBook.StatusCode);
            Assert.AreEqual(string.Format(Constants.IdAlreadyExists, book.Id), secondCreateBook.Error.Message);
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

            var createBook = await _bookService.CreateBook(book);
            Assert.IsFalse(createBook.IsSuccess);
            Assert.AreEqual(HttpStatusCode.BadRequest, createBook.StatusCode);
            Assert.AreEqual(Constants.AuthorRequired, createBook.Error.Message);
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

            var createBook = await _bookService.CreateBook(book);
            Assert.IsFalse(createBook.IsSuccess);
            Assert.AreEqual(HttpStatusCode.BadRequest, createBook.StatusCode);
            Assert.AreEqual(Constants.InvalidId, createBook.Error.Message);
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

            var createBook = await _bookService.CreateBook(book);
            Assert.IsFalse(createBook.IsSuccess);
            Assert.AreEqual(HttpStatusCode.BadRequest, createBook.StatusCode);
            Assert.AreEqual(Constants.TitleRequired, createBook.Error.Message);
        }

        [Test]
        public async Task CreateBook_WithoutDescription_OK()
        {
            var book = new Book()
            {
                Id = new Random().Next(1, 1000),
                Title = "Title",
                Description = null,
                Author = "Test",
            };

            var createBook = await _bookService.CreateBook(book);
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

            var createBook = await _bookService.CreateBook(book);
            Assert.IsFalse(createBook.IsSuccess);
            Assert.AreEqual(HttpStatusCode.BadRequest, createBook.StatusCode);
            Assert.AreEqual(Constants.AuthorRequired, createBook.Error.Message);
        }

        [Test]
        // Bug: This also fails when char length is 100 but shouldn't as that's the max allowed according to the error message
        public async Task CreateBook_TitleMaxCharacters_BadRequest()
        {
            var book = new Book()
            {
                Id = new Random().Next(1, 1000),
                Title = GenerateRandomString(101),
                Description = "Description",
                Author = "Author",
            };

            var createBook = await _bookService.CreateBook(book);
            Assert.IsFalse(createBook.IsSuccess);
            Assert.AreEqual(HttpStatusCode.BadRequest, createBook.StatusCode);
            Assert.AreEqual(Constants.TitleMaxCharacters, createBook.Error.Message);
        }

        [Test]
        // Bug: This throws a BadRequest when char length is 30 but shouldn't as that's the max allowed according to the error message
        public async Task CreateBook_AuthorMaxCharacters_BadRequest()
        {
            var book = new Book()
            {
                Id = new Random().Next(1, 1000),
                Title = "Title",
                Description = "Description",
                Author = GenerateRandomString(30),
            };

            var createBook = await _bookService.CreateBook(book);
            Assert.IsFalse(createBook.IsSuccess);
            Assert.AreEqual(HttpStatusCode.BadRequest, createBook.StatusCode);
            Assert.AreEqual(Constants.AuthorMaxCharacters, createBook.Error.Message);
        }
    }
}

