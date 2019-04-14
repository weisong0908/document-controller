using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DocumentController.WPF.Models;
using Newtonsoft.Json;

namespace DocumentController.WPF.Services
{
    public class CloudDocumentService : IDocumentService
    {
        private HttpClient client;

        public CloudDocumentService()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri((Application.Current as App).ApiEndPointBaseAddress)
            };
        }

        public async Task<IEnumerable<Document>> GetDocuments()
        {
            var documents = new List<Document>();

            var response = await client.GetAsync("documents");
            if(response.IsSuccessStatusCode)
            {
                documents = JsonConvert.DeserializeObject<List<Document>>(await response.Content.ReadAsStringAsync());
            }

            return documents.Where(d => d.Status == DocumentStatus.Active);
        }

        public async Task<Document> AddNewDocument(Document document)
        {
            var setting = new JsonSerializerSettings();
            setting.DefaultValueHandling = DefaultValueHandling.Ignore;

            var content = new StringContent(JsonConvert.SerializeObject(document), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("documents", content);

            response.EnsureSuccessStatusCode();

            var result = JsonConvert.DeserializeObject<Document>(await response.Content.ReadAsStringAsync());

            return result;
        }

        public async void RemoveDocument(Document document)
        {
            var response = await client.DeleteAsync($"documents/{document.Id}");

            response.EnsureSuccessStatusCode();
        }
    }
}
