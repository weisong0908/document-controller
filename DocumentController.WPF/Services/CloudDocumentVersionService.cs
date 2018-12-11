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
    public class CloudDocumentVersionService : IDocumentVersionService
    {
        private HttpClient client;

        public CloudDocumentVersionService()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri((Application.Current as App).ApiEndPointBaseAddress)
            };
        }

        public async Task<IEnumerable<DocumentVersion>> GetAllVersionsByDocumentId(int documentId)
        {
            var documentVersions = new List<DocumentVersion>();

            var response = await client.GetAsync($"documentversions/document/{documentId}");

            if(response.IsSuccessStatusCode)
            {
                documentVersions = JsonConvert.DeserializeObject<List<DocumentVersion>>(await response.Content.ReadAsStringAsync());
            }

            return documentVersions;
        }

        public async Task<DocumentVersion> UpdateDocumentVersion(DocumentVersion documentVersion)
        {
            var content = new StringContent(JsonConvert.SerializeObject(documentVersion), Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"documentversions/{documentVersion.Id}", content);

            response.EnsureSuccessStatusCode();

            var result = JsonConvert.DeserializeObject<DocumentVersion>(await response.Content.ReadAsStringAsync());

            return result;
        }

        public async Task<DocumentVersion> AddNewDocumentVersion(DocumentVersion documentVersion)
        {
            var setting = new JsonSerializerSettings();
            setting.DefaultValueHandling = DefaultValueHandling.Ignore;

            var content = new StringContent(JsonConvert.SerializeObject(documentVersion, setting), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("documentversions", content);

            response.EnsureSuccessStatusCode();

            var result = JsonConvert.DeserializeObject<DocumentVersion>(await response.Content.ReadAsStringAsync());

            return result;
        }

        public async void RemoveDocumentVersion(DocumentVersion documentVersion)
        {
            var response = await client.DeleteAsync($"documentversions/{documentVersion.Id}");

            response.EnsureSuccessStatusCode();
        }
    }
}
