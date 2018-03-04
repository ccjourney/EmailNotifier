using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmailNotifier.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace EmailNotifier.Data
{
    public class NewUserRepository : INewUserRepository
    {
        private readonly Uri _uri;
        private readonly DocumentClient _client;
        private readonly string _database;
        private readonly string _collection;

        public NewUserRepository(string database, string collection, string cosmosDbEndpoint, string cosmosDbKey)
        {
            _collection = collection;
            _database = database;
            _client = new DocumentClient(new Uri(cosmosDbEndpoint), cosmosDbKey);
            //CreateDatabaseIfNotExists().Wait();
            //CreateCollectionIfNotExists().Wait();
            _uri = UriFactory.CreateDocumentCollectionUri(_database, _collection);
        }

        public async Task<IEnumerable<NewUser>> GetAllUsers()
        {
            var query = _client.CreateDocumentQuery<NewUser>(_uri, new FeedOptions { MaxItemCount = -1 })
                .AsDocumentQuery();

            var newUsers = new List<NewUser>();
            while (query.HasMoreResults)
            {
                newUsers.AddRange(await query.ExecuteNextAsync<NewUser>());
            }

            return newUsers;
        }

        public async Task<NewUser> GetNewUser(string id)
        {
            var document = await _client.ReadDocumentAsync<NewUser>(UriFactory.CreateDocumentUri(_database, _collection, id));
            return document;
        }
        public async Task CreateNewUser(NewUser newUser)
        {
            await _client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(_database, _collection), newUser);
        }

        public async Task UpdateNewUser(string id, NewUser newUser)
        {
            await _client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(_database, _collection, id), newUser);
        }

        private async Task CreateDatabaseIfNotExists()
        {
            try
            {
                await _client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(_database));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await _client.CreateDatabaseAsync(new Database { Id = _database });
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task CreateCollectionIfNotExists()
        {
            try
            {
                await _client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(_database, _collection));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await _client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(_database),
                        new DocumentCollection { Id = _collection },
                        new RequestOptions { OfferThroughput = 400 });
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
