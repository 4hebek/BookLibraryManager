using LibraryManager.Core;
using LibraryManager.Core.Contracts;
using NUnit.Framework;
using System.Net;

namespace LibraryManager.Tests.CRUDTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.None)]
    public class CreateBookTests : TestBase
    {
        [Test]
        public async Task CreateBook_Ok()
        {
            var book = new Book()
            {
                Id = new Random().Next(1, 10000),
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
            var bookId = new Random().Next(1, 10000);

            var createBook = await _bookService.CreateBook(id: bookId);
            Assert.AreEqual(HttpStatusCode.OK, createBook.StatusCode);

            var secondCreateBook = await _bookService.CreateBook(id: bookId);
            Assert.IsFalse(secondCreateBook.IsSuccess);
            Assert.AreEqual(HttpStatusCode.BadRequest, secondCreateBook.StatusCode);
            Assert.AreEqual(string.Format(Constants.IdAlreadyExists, bookId), secondCreateBook.Error.Message);
        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        public async Task CreateBook_WithoutAuthor_BadRequest(string author)
        {
            var book = new Book()
            {
                Id = new Random().Next(1, 10000),
                Title = "Title",
                Description = "Description",
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
            var createBook = await _bookService.CreateBook(id: bookId);
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
            var createBook = await _bookService.CreateBook(title: title);
            Assert.IsFalse(createBook.IsSuccess);
            Assert.AreEqual(HttpStatusCode.BadRequest, createBook.StatusCode);
            Assert.AreEqual(Constants.TitleRequired, createBook.Error.Message);
        }

        [Test]
        public async Task CreateBook_WithoutDescription_OK()
        {
            var createBook = await _bookService.CreateBook(description: null);
            Assert.AreEqual(HttpStatusCode.OK, createBook.StatusCode);
            Assert.AreEqual(null, createBook.Success.Description);
        }

        [Test]
        // Bug: This also fails when char length is 100 but shouldn't as that's the max allowed according to the error message
        public async Task CreateBook_TitleMaxCharacters_BadRequest()
        {
            var title = GenerateRandomString(101);
            var createBook = await _bookService.CreateBook(title: title);

            Assert.IsFalse(createBook.IsSuccess);
            Assert.AreEqual(HttpStatusCode.BadRequest, createBook.StatusCode);
            Assert.AreEqual(Constants.TitleMaxCharacters, createBook.Error.Message);
        }

        [Test]
        // Bug: This throws a BadRequest when char length is 30 but shouldn't as that's the max allowed according to the error message
        public async Task CreateBook_AuthorMaxCharacters_BadRequest()
        {
            var author = GenerateRandomString(30);
            var createBook = await _bookService.CreateBook(author: author);

            Assert.IsFalse(createBook.IsSuccess);
            Assert.AreEqual(HttpStatusCode.BadRequest, createBook.StatusCode);
            Assert.AreEqual(Constants.AuthorMaxCharacters, createBook.Error.Message);
        }
    }
}

