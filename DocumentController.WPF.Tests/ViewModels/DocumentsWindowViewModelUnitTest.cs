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
using DocumentController.WPF.Helpers;
using System;

namespace DocumentController.WPF.Tests.ViewModels
{
    public class DocumentsWindowViewModelUnitTest
    {
        private readonly DocumentsWindowViewModel documentsWindowViewModel;
        private readonly Mock<IDocumentService> mockDocumentService;
        private readonly Mock<IDocumentVersionService> mockDocumentVersonService;
        private readonly Mock<IFileHelper> stubFileHelper;
        private readonly Mock<IWindowHelper> stubWindowHelper;
        private readonly IMapper mapper;

        private IList<Document> documents;

        public DocumentsWindowViewModelUnitTest()
        {
            documents = new List<Document>();
            documents.Add(new Document() { Id = 1, Title = "Document 1" });
            documents.Add(new Document() { Id = 2, Title = "Document 2" });

            mockDocumentService = new Mock<IDocumentService>();
            mockDocumentService.Setup(ds => ds.GetDocuments()).Returns(Task.FromResult<IEnumerable<Document>>(documents));

            mockDocumentVersonService = new Mock<IDocumentVersionService>();

            stubFileHelper = new Mock<IFileHelper>();

            stubWindowHelper = new Mock<IWindowHelper>();

            mapper = new MapperConfiguration(c => c.AddProfile<MappingProfile>()).CreateMapper();

            documentsWindowViewModel = new DocumentsWindowViewModel(mockDocumentService.Object, mockDocumentVersonService.Object, stubFileHelper.Object, stubWindowHelper.Object, mapper);
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

            Assert.Equal(documents.Count(), result.Count());
        }

        [Fact]
        public void SelectDocument_WhenCalled_ReturnsDocumentVersionFromDatabase()
        {
            var document = new DocumentViewModel() { Id = 1 };

            documentsWindowViewModel.SelectDocument(document);

            mockDocumentVersonService.Verify(dvs => dvs.GetAllVersionsByDocumentId(document.Id));
        }

        [Fact]
        public void SelectDocument_WhenCalled_SetsSelectedDocument()
        {
            var document = new DocumentViewModel() { Id = 1 };

            documentsWindowViewModel.SelectDocument(document);

            Assert.Equal(document, documentsWindowViewModel.SelectedDocument);
        }
    }
}
