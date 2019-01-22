using DocumentController.WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentController.WPF.Services
{
    public class FakeDocumentService : IDocumentService
    {
        private readonly IList<Document> documents;

        public FakeDocumentService()
        {
            documents = new List<Document>
            {
                new Document()
                {
                    Id = 1,
                    Title = "Procedure Test 1",
                    Type = DocumentType.Form,
                    Status = DocumentStatus.Active,
                    Department = Department.QualityCompliance
                },
                new Document()
                {
                    Id = 2,
                    Title = "Document 2",
                    Type = DocumentType.Policy,
                    Status = DocumentStatus.Active,
                    Department = Department.Admissions
                },
                new Document()
                {
                    Id = 3,
                    Title = "Document 3",
                    Type = DocumentType.Form,
                    Status = DocumentStatus.Active,
                    Department = Department.Academic
                }
            };
        }

        public async Task<Document> AddNewDocument(Document document)
        {
            return await Task.Run(() => document);
        }

        public async Task<IEnumerable<Document>> GetDocuments()
        {
            return await Task.Run(() => documents);
        }

        public void RemoveDocument(Document document)
        {
            throw new NotImplementedException();
        }
    }
}
