namespace LibraryManager.Core
{
    public class Constants
    {
        // Error messages
        public const string IdAlreadyExists = @"Book with id {0} already exists!";
        public const string InvalidId = "Book.Id should be a positive integer!\r\nParameter name: book.Id";
        public const string AuthorRequired = "Book.Author is a required field.\r\nParameter name: book.Author";
        public const string TitleRequired = "Book.Title is a required field\r\nParameter name: Book.Title";
        public const string TitleMaxCharacters = "Book.Title should not exceed 100 characters!\r\nParameter name: Book.Title";
        public const string AuthorMaxCharacters = "Book.Author should not exceed 30 characters!\r\nParameter name: Book.Author";
        public const string NoBookFound = @"Book with id {0} not found!";
    }
}
