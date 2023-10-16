using LibraryManager.Core;
using LibraryManager.Core.Contracts;
using NUnit.Framework;

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
            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedBook.Id, actualBook.Id, "Id was not as expected");
                Assert.AreEqual(expectedBook.Author, actualBook.Author, "Author was not as expected");
                Assert.AreEqual(expectedBook.Title, actualBook.Title, "Title was not as expected");
                Assert.AreEqual(expectedBook.Description, actualBook.Description, "Description was not as expected");
            });
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
