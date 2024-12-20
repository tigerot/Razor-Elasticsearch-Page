using Nest;
using Elasticsearch.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace SozcuMainPage.Services
{
    public class ElasticSearchService
    {
        private readonly ElasticLowLevelClient _client;

        public ElasticSearchService()
        {
            var settings = new ConnectionConfiguration(new Uri("http://localhost:9200"));
            _client = new ElasticLowLevelClient(settings);
        }

        public async Task<List<MyDocument>> GetDocumentsAsync()
        {
            var response = await _client.SearchAsync<StringResponse>("sozcu-main-page", PostData.Serializable(new { query = new { match_all = new { } } }));
            var documents = JsonConvert.DeserializeObject<SearchResponse>(response.Body);
            return documents?.Hits?.HitsList?.Select(hit => hit.Source).ToList() ?? new List<MyDocument>();
        }

        public async Task<List<MyDocument>> SearchDocumentsAsync(string query)
        {
            var response = await _client.SearchAsync<StringResponse>("sozcu-main-page", PostData.Serializable(new
            {
                query = new
                {
                    match = new
                    {
                        title = new { query = query }  
                    }
                }
            }));

            Console.WriteLine(response.Body);

            var documents = JsonConvert.DeserializeObject<SearchResponse>(response.Body);
            return documents?.Hits?.HitsList?.Select(hit => hit.Source).ToList() ?? new List<MyDocument>();
        }

        private class SearchResponse
        {
            [JsonProperty("hits")]
            public Hits Hits { get; set; }
        }

        private class Hits
        {
            [JsonProperty("total")]
            public Total Total { get; set; }

            [JsonProperty("hits")]
            public List<Hit> HitsList { get; set; }
        }

        private class Hit
        {
            [JsonProperty("_source")]
            public MyDocument Source { get; set; }
        }

        private class Total
        {
            [JsonProperty("value")]
            public long Value { get; set; }

            [JsonProperty("relation")]
            public string Relation { get; set; }
        }


        public class MyDocument
        {
            public string Title { get; set; }
            public string Link { get; set; }
            public DateTime Timestamp { get; set; }
        }
    }
}

