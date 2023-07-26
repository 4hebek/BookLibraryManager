﻿using LibraryManager.Core.Contracts;
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

        public async Task<Book> CreateBookAsync(Book book)
        {
            _inMemoryBooks.Add(book);

            HttpResponseMessage response = await _client.PostAsJsonAsync("books", book);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Book>(jsonResponse);
        }

        public async Task<Book> GetBookAsync(int? bookId) 
        {
            var book = _inMemoryBooks.FirstOrDefault(b => b.Id == bookId);
            HttpResponseMessage response = await _client.GetAsync(book.Id.ToString());

            if (response.IsSuccessStatusCode) 
            {
                book = await response.Content.ReadAsAsync<Book>();
            }
            return book;
        }

        public async Task<Book> UpdateBookAsync(int? bookId, Book book)
        {
            var getBook = _inMemoryBooks.FirstOrDefault(b => b.Id == bookId);
            var response = await _client.PutAsJsonAsync($"books/{book.Id.ToString()}", book);

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Book>(jsonResponse);;
        }

        public async Task<HttpResponseMessage> DeleteBookAsync(int? bookId)
        {
            var book = _inMemoryBooks.FirstOrDefault(b => b.Id == bookId);
            var response = await _client.DeleteAsync($"books/{book.Id.ToString()}");

            return response;
        }
    }
}