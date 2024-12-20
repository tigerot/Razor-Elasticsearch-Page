using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SozcuMainPage.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static SozcuMainPage.Services.ElasticSearchService;

namespace SozcuMainPage.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly ElasticSearchService _elasticsearchService;

        public IndexModel(ElasticSearchService elasticsearchService)
        {
            _elasticsearchService = elasticsearchService;
        }

        public List<MyDocument> Documents { get; set; }
        public string Query { get; set; }

        public async Task OnGetAsync(string query)
        {
            Query = query;

            if (!string.IsNullOrWhiteSpace(query))
            {
                Documents = await _elasticsearchService.SearchDocumentsAsync(query);
            }
            else
            {
                Documents = await _elasticsearchService.GetDocumentsAsync();
            }
        }

    }
}