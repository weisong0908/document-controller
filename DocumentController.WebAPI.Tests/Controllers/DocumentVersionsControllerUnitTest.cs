using Xunit;
using Moq;
using DocumentController.WebAPI.Controllers;
using DocumentController.WebAPI.Persistence;
using System.Collections.Generic;
using DocumentController.WebAPI.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DocumentController.WebAPI.Tests.Controllers
{
    public class DocumentVersionsControllerUnitTest
    {
        private DocumentVersionsController controller;
        private Mock<IDocumentVersionRepository> mockRespository;
        private Mock<IUnitOfWork> stubUnitOfWork;

        public DocumentVersionsControllerUnitTest()
        {
            mockRespository = new Mock<IDocumentVersionRepository>();
            mockRespository.Setup(r => r.GetAllDocumentVersionsByDocumentId(It.IsAny<int>())).Returns(Task.FromResult<IEnumerable<DocumentVersion>>(new List<DocumentVersion>()));
            mockRespository.Setup(r => r.GetAllDocumentVersionsByDocumentId(999)).Returns(Task.FromResult<IEnumerable<DocumentVersion>>(null));
            mockRespository.Setup(r => r.GetDocumentVersion(It.IsAny<int>())).Returns(Task.FromResult<DocumentVersion>(new DocumentVersion()));
            mockRespository.Setup(r => r.GetDocumentVersion(999)).Returns(Task.FromResult<DocumentVersion>(null));
            mockRespository.Setup(r => r.UpdateDocumentVersion(It.IsAny<DocumentVersion>())).Returns(Task.FromResult<DocumentVersion>(new DocumentVersion()));
            mockRespository.Setup(r => r.RemoveDocumentVersion(It.IsAny<int>())).Returns(Task.FromResult<DocumentVersion>(new DocumentVersion()));

            stubUnitOfWork = new Mock<IUnitOfWork>();

            controller = new DocumentVersionsController(mockRespository.Object, stubUnitOfWork.Object);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetDocumentVersionsByDocumentId_WhenCalled_ReturnsDocumentVersionsFromDatabase(int documentId)
        {
            await controller.GetDocumentVersionsByDocumentId(documentId);

            mockRespository.Verify(r => r.GetAllDocumentVersionsByDocumentId(documentId));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetDocumentVersionsByDocumentId_WhenCalledAndHasData_ReturnsOkObjectResult(int documentId)
        {
            var result = await controller.GetDocumentVersionsByDocumentId(documentId);

            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async void GetDocumentVersionsByDocumentId_WhenCalledAndDoesNotHaveData_ReturnsNotFound()
        {
            var result = await controller.GetDocumentVersionsByDocumentId(999);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetDocumentVersion_WhenCalled_ReturnsDocumentVersionsFromDatabase(int documentId)
        {
            await controller.GetDocumentVersion(documentId);

            mockRespository.Verify(r => r.GetDocumentVersion(documentId));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetDocumentVersion_WhenCalledAndHasData_ReturnsOkObjectResult(int documentId)
        {
            var result = await controller.GetDocumentVersion(documentId);

            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async void GetDocumentVersion_WhenCalledAndDoesNotHaveData_ReturnsNotFound()
        {
            var result = await controller.GetDocumentVersion(999);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async void AddNewDocumentVersion_WhenCalled_AddsDocumentVersionToDatabase()
        {
            var documentVersion = new DocumentVersion();

            await controller.AddNewDocumentVersion(documentVersion);

            mockRespository.Verify(r => r.AddNewDocumentVersion(documentVersion));
        }

        [Fact]
        public async void AddNewDocumentVersion_WhenCalled_ReturnsObjectCreatedResult()
        {
            var documentVersion = new DocumentVersion();

            var result = await controller.AddNewDocumentVersion(documentVersion);

            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        [Fact]
        public async void UpdateDocumentVersion_WhenCalled_UpdatesDocumentVersionInDatabase()
        {
            var documentVersion = new DocumentVersion();

            await controller.UpdateDocumentVersion(documentVersion.Id, documentVersion);

            mockRespository.Verify(r => r.UpdateDocumentVersion(documentVersion));
        }

        [Fact]
        public async void UpdateDocumentVersion_WhenCalled_ReturnsOkResult()
        {
            var documentVersion = new DocumentVersion();

            var result = await controller.UpdateDocumentVersion(documentVersion.Id, documentVersion);

            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Theory]
        [InlineData(1)]
        public async void RemoveDocumentVersion_WhenCalled_RemovesDocumentVersionFromDatabase(int documentVersionId)
        {
            await controller.RemoveDocumentVersion(documentVersionId);

            mockRespository.Verify(r => r.RemoveDocumentVersion(documentVersionId));
        }

        [Theory]
        [InlineData(1)]
        public async void RemoveDocumentVersion_WhenCalled_ReturnsOkResult(int documentVersionId)
        {
            var result = await controller.RemoveDocumentVersion(documentVersionId);

            Assert.IsType<OkObjectResult>(result.Result);
        }
    }
}