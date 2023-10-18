using LibraryManager.Core.Contracts;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace LibraryManager.Core
{
    public class BookService
    {
        private HttpClient _client;
        private string _url = "http://localhost:9000/api/";
        private List<Book> _inMemoryBooks = new List<Book>(); // In-memory data store

        public BookService()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_url);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<Result<Book, Error>> CreateBook(Book book)
        {
            //var newBook = new Book()
            //{
            //    Id = book.Id ?? new Random().Next(1, 1000),
            //    Title = book.Title ?? "Book Title",
            //    Description = book.Description ?? "Book Description",
            //    Author = book.Author ?? "Book Author",
            //};

            _inMemoryBooks.Add(book);

            HttpResponseMessage response = await _client.PostAsJsonAsync("books", book);
            return await ReadBook(response);
        }


        public async Task<Result<Book, Error>> GetBookById(int? bookId)
        {
            var book = _inMemoryBooks.FirstOrDefault(b => b.Id == bookId);

            if (book != null)
            {
                bookId = book.Id;
            }

            HttpResponseMessage response = await _client.GetAsync($"books/{bookId}");
            return await ReadBook(response);
        }


        public async Task<Book> UpdateBookAsync(int? bookId, Book book)
        {
            var getBook = _inMemoryBooks.FirstOrDefault(b => b.Id == bookId);
            var response = await _client.PutAsJsonAsync($"books/{book.Id}", book);

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Book>(jsonResponse);;
        }


        public async Task<Result<Book, Error>> DeleteBookAsync(int? bookId)
        {
            var book = _inMemoryBooks.FirstOrDefault(b => b.Id == bookId);

            HttpResponseMessage response = await _client.DeleteAsync($"books/{bookId}");
            return await ReadBook(response);
        }

        public async Task<Result<Book, Error>> ReadBook(HttpResponseMessage response)
        {
            try
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (response != null)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var bookResult = Newtonsoft.Json.JsonConvert.DeserializeObject<Book>(jsonResponse);
                        return Result<Book, Error>.SuccessResponse(bookResult, response.StatusCode);
                    }
                    else
                    {
                        var errorResponse = await response.Content.ReadFromJsonAsync<Error>();
                        return Result<Book, Error>.ErrorResponse(errorResponse, response.StatusCode);
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                var errorResponse = new Error { Message = ex.Message };
                return Result<Book, Error>.ErrorResponse(errorResponse, HttpStatusCode.InternalServerError);
            }

            return Result<Book, Error>.ErrorResponse(new Error { Message = "Failed to read book" }, HttpStatusCode.InternalServerError);
        }
    }
}
