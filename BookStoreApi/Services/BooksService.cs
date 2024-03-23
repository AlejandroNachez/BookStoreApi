using BookStoreApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BookStoreApi.Services
{
    public class BooksService
    {
        private readonly IMongoCollection<Book> _booksCollection;

        public BooksService(IOptions<BookStoreDataBaseSettings> bookStoreDataBaseSettings)
        {
            var mongoClient = new MongoClient(bookStoreDataBaseSettings.Value.ConnectionString);

            var mongoDataBase = mongoClient.GetDatabase(bookStoreDataBaseSettings.Value.DatabaseName);

            _booksCollection = mongoDataBase.GetCollection<Book>(bookStoreDataBaseSettings.Value.BooksCollectionName);
        }

        public async Task<List<Book>> GetAsync() =>
            await _booksCollection.Find(_ => true).ToListAsync();

        public async Task<Book> GetAsync(string id) =>
            await _booksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Book newBook) =>
            await _booksCollection.InsertOneAsync(newBook);

        public async Task UpdateAsync(string id, Book newBook) =>
            await _booksCollection.ReplaceOneAsync(x => x.Id == id, newBook);

        public async Task RemoveAsync(string id) =>
            await _booksCollection.DeleteOneAsync(x => x.Id == id);
    }
}
