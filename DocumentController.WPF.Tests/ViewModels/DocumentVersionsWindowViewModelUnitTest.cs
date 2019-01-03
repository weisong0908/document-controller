using Xunit;
using Moq;
using DocumentController.WPF.ViewModels;
using DocumentController.WPF.Services;
using DocumentController.WPF.Helpers;
using AutoMapper;
using DocumentController.WPF.Mapping;
using DocumentController.WPF.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using System.Threading.Tasks;

namespace DocumentController.WPF.Tests.ViewModels
{
    public class DocumentVersionsWindowViewModelUnitTest
    {
        private readonly DocumentVersionsWindowViewModel documentVersionsWindowViewModel;
        private readonly Mock<IDocumentVersionService> mockDocumentVersionService;
        private readonly Mock<IFileHelper> stubFileHelper;
        private readonly Mock<IWindowHelper> stubWindowHelper;
        private readonly IMapper mapper;
        private IList<DocumentVersionViewModel> documentVersions;

        public DocumentVersionsWindowViewModelUnitTest()
        {
            mockDocumentVersionService = new Mock<IDocumentVersionService>();
            stubFileHelper = new Mock<IFileHelper>();
            stubWindowHelper = new Mock<IWindowHelper>();
            mapper = new MapperConfiguration(c => c.AddProfile<MappingProfile>()).CreateMapper();

            documentVersionsWindowViewModel = new DocumentVersionsWindowViewModel(mockDocumentVersionService.Object, stubFileHelper.Object, stubWindowHelper.Object, mapper);

            documentVersions = new List<DocumentVersionViewModel>()
            {
                new DocumentVersionViewModel(1){ Id = 1 , VersionNumber = "Version 1", Progress="Some progress", EffectiveDate=DateTime.Today },
                new DocumentVersionViewModel(1){ Id = 2 , VersionNumber = "Version 2", Progress="Some progress", EffectiveDate=DateTime.Today },
                new DocumentVersionViewModel(1){ Id = 3 , VersionNumber = "Version 3", Progress="Some progress", EffectiveDate=DateTime.Today }
            };

            mockDocumentVersionService.Setup(dvs => dvs.UpdateDocumentVersion(It.IsAny<DocumentVersion>())).Returns(Task.FromResult(mapper.Map<DocumentVersion>(documentVersions[0])));

            documentVersionsWindowViewModel.DocumentVersions = new ObservableCollection<DocumentVersionViewModel>(documentVersions);
        }

        [Fact]
        public void OnStartUp_WhenCalled_SetsSelectedDocument()
        {
            var document = new DocumentViewModel();

            documentVersionsWindowViewModel.OnStartUp(document);

            Assert.Equal(document, documentVersionsWindowViewModel.SelectedDocument);
        }

        [Fact]
        public void OnStartUp_WhenCalled_ReturnsDocumentVersionsFromDatabase()
        {
            var document = new DocumentViewModel() { Id = 1 };

            documentVersionsWindowViewModel.OnStartUp(document);

            mockDocumentVersionService.Verify(dvs => dvs.GetAllVersionsByDocumentId(document.Id));
        }

        [Fact]
        public void CreateNewDocumentVersion_WhenCalled_InitialiseNewSelectedDocumentVersion()
        {
            var document = new DocumentViewModel() { Id = 1 };
            documentVersionsWindowViewModel.SelectedDocument = document;

            documentVersionsWindowViewModel.CreateNewDocumentVersion();

            Assert.True(documentVersionsWindowViewModel.SelectedDocumentVersion.Id == 0 && documentVersionsWindowViewModel.SelectedDocumentVersion.DocumentId == document.Id);
        }

        [Fact]
        public void RemoveDocumentVersion_WhenCalled_RemoveSelectedDocumentVersionFromDatabase()
        {
            var documentVersionToBeRemoved = documentVersions[0];
            documentVersionsWindowViewModel.SelectedDocumentVersion = documentVersionToBeRemoved;
            documentVersionsWindowViewModel.RemoveDocumentVersion();

            Assert.DoesNotContain(documentVersionsWindowViewModel.DocumentVersions, dv => dv.Id == documentVersionToBeRemoved.Id);
        }

        [Fact]
        public void UploadDocument_WhenCalled_UploadsDocumentsUsingFileHelper()
        {
            var document = new DocumentViewModel() { Id = 1 };
            var documentVersion = new DocumentVersionViewModel(document.Id);

            documentVersionsWindowViewModel.SelectedDocument = document;
            documentVersionsWindowViewModel.SelectedDocumentVersion = documentVersion;

            documentVersionsWindowViewModel.UploadDocument();
            stubFileHelper.Verify(fh => fh.UpdateFiles(documentVersionsWindowViewModel.SelectedDocument, documentVersionsWindowViewModel.SelectedDocumentVersion));
        }
    }
}
