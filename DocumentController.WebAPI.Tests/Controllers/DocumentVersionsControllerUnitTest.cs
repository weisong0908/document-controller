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
            // mockRespository.Setup(r => r.AddNewDocumentVersion(It.IsAny<DocumentVersion>()));
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
    }
}