using FluentAssertions;
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
            createBook.StatusCode.Should().Be(HttpStatusCode.OK);
            AssertBookProperties(book, createBook.Success);
        }

        [Test]
        public async Task CreateBook_SameId_BadRequest()
        {
            var bookId = new Random().Next(1, 10000);
            var errorMessage = string.Format(Constants.IdAlreadyExists, bookId);
            var createBook = await _bookService.CreateBook(id: bookId);
            createBook.StatusCode.Should().Be(HttpStatusCode.OK);

            var secondCreateBook = await _bookService.CreateBook(id: bookId);

            secondCreateBook.IsSuccess.Should().BeFalse();
            secondCreateBook.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            secondCreateBook.Error.Message.Should().Be(errorMessage);
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

            createBook.IsSuccess.Should().BeFalse();
            createBook.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            createBook.Error.Message.Should().Be(Constants.AuthorRequired);
        }

        [Test]
        [TestCase(-1)]
        [TestCase(0)]
        public async Task CreateBook_InvalidId_BadRequest(int bookId)
        {
            var createBook = await _bookService.CreateBook(id: bookId);

            createBook.IsSuccess.Should().BeFalse();
            createBook.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            createBook.Error.Message.Should().Be(Constants.InvalidId);
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public async Task CreateBook_InvalidTitle_BadRequest(string title)
        {
            var createBook = await _bookService.CreateBook(title: title);

            createBook.IsSuccess.Should().BeFalse();
            createBook.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            createBook.Error.Message.Should().Be(Constants.TitleRequired);
        }

        [Test]
        public async Task CreateBook_WithoutDescription_OK()
        {
            var createBook = await _bookService.CreateBook(description: null);
            createBook.StatusCode.Should().Be(HttpStatusCode.OK);
            createBook.Success.Description.Should().BeNull();
        }

        [Test]
        // Bug: This also fails when char length is 100 but shouldn't as that's the max allowed according to the error message
        public async Task CreateBook_TitleMaxCharacters_BadRequest()
        {
            var title = GenerateRandomString(101);
            var createBook = await _bookService.CreateBook(title: title);

            createBook.IsSuccess.Should().BeFalse();
            createBook.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            createBook.Error.Message.Should().Be(Constants.TitleMaxCharacters);
        }

        [Test]
        // Bug: This throws a BadRequest when char length is 30 but shouldn't as that's the max allowed according to the error message
        public async Task CreateBook_AuthorMaxCharacters_BadRequest()
        {
            var author = GenerateRandomString(30);
            var createBook = await _bookService.CreateBook(author: author);

            createBook.IsSuccess.Should().BeFalse();
            createBook.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            createBook.Error.Message.Should().Be(Constants.AuthorMaxCharacters);
        }
    }
}

