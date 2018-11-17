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
                    Title = "Document 1",
                    Type = DocumentType.Procedure,
                    Status = DocumentStatus.Active,
                    Department = Department.Academic
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

        public async Task<IEnumerable<Document>> GetDocuments()
        {
            return await Task.Run(() => documents);
        }
    }
}
