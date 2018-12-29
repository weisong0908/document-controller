using Xunit;
using Moq;
using DocumentController.WPF.Services;
using DocumentController.WPF.ViewModels;
using AutoMapper;
using DocumentController.WPF.Mapping;
using System.Threading.Tasks;
using System.Collections.Generic;
using DocumentController.WPF.Models;
using System.Linq;

namespace DocumentController.WPF.Tests.ViewModels
{
    public class DocumentsWindowViewModelUnitTest
    {
        private readonly DocumentsWindowViewModel documentsWindowViewModel;
        private readonly Mock<IDocumentService> mockDocumentService;
        private readonly Mock<IDocumentVersionService> mockDocumentVersonService;

        private IList<Document> documents;

        public DocumentsWindowViewModelUnitTest()
        {
            documents = new List<Document>();
            documents.Add(new Document() { Id = 1, Title = "Document 1" });
            documents.Add(new Document() { Id = 2, Title = "Document 2" });

            mockDocumentService = new Mock<IDocumentService>();
            mockDocumentService.Setup(ds => ds.GetDocuments()).Returns(Task.FromResult<IEnumerable<Document>>(documents));

            mockDocumentVersonService = new Mock<IDocumentVersionService>();
            Mapper.Reset();
            Mapper.Initialize(c => c.AddProfile<MappingProfile>());

            documentsWindowViewModel = new DocumentsWindowViewModel(mockDocumentService.Object, mockDocumentVersonService.Object);
        }

        [Fact]
        public void OnActivated_WhenCalled_ReturnsDocumentsFromDocumentService()
        {
            documentsWindowViewModel.OnActivated();

            mockDocumentService.Verify(ds => ds.GetDocuments());
        }

        [Fact]
        public void OnActivated_WhenCalled_SetFilteredDocumentsToDatabaseDocuments()
        {
            documentsWindowViewModel.OnActivated();

            var result = documentsWindowViewModel.FilteredDocuments;

            Assert.Equal(result.Count(), documents.Count());
        }

    }
}
