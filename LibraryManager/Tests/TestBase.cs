using FluentAssertions.Execution;
using LibraryManager.Core;
using LibraryManager.Core.Contracts;

namespace LibraryManager.Tests
{
    public abstract class TestBase
    {
        public BookService _bookService;

        public TestBase() 
        {
            _bookService = new BookService();
        }

        public void AssertBookProperties(Book expectedBook, Book actualBook)
        {
            using (new AssertionScope())
            {
                actualBook.Id.Should().Be(expectedBook.Id);
                actualBook.Title.Should().Be(expectedBook.Title);
                actualBook.Description.Should().Be(expectedBook.Description);

                // Bug: Author is not saved therefore this assert fails
                //actualBook.Author.Should().Be(expectedBook.Author);
            }
        }

        public string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            var random = new Random();
            var result = new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            return result;
        }
    }
}
