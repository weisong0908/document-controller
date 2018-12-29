using System;
using DocumentController.WebAPI.Controllers;
using Xunit;
using Moq;
using DocumentController.WebAPI.Persistence;
using DocumentController.WebAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DocumentController.WebAPI.Tests.Controllers
{
    public class DocumentsControllerUnitTest
    {
        private DocumentsController controller;
        private Mock<IDocumentRepository> mockRepository;
        private Mock<IUnitOfWork> stubUnitOfWork;

        public DocumentsControllerUnitTest()
        {
            var documents = new List<Document>();
            documents.Add(new Document() { Id = 1, Title = "Document 1" });
            documents.Add(new Document() { Id = 2, Title = "Document 2" });

            mockRepository = new Mock<IDocumentRepository>();
            mockRepository.Setup(dr => dr.GetAllDocuments()).Returns(Task.FromResult<IEnumerable<Document>>(documents));
            mockRepository.Setup(dr => dr.GetDocument(It.IsAny<int>())).Returns(Task.FromResult<Document>(new Document()));
            mockRepository.Setup(dr => dr.GetDocument(999)).Returns(Task.FromResult<Document>(null));
            stubUnitOfWork = new Mock<IUnitOfWork>();

            controller = new DocumentsController(mockRepository.Object, stubUnitOfWork.Object);
        }

        [Fact]
        public async void GetDocuments_WhenCalled_ReturnsAllDocumentsFromDatabase()
        {
            await controller.GetDocuments();

            mockRepository.Verify(dr => dr.GetAllDocuments());
        }

        [Fact]
        public async void GetDocuments_WhenCalled_ReturnsOkObjectResult()
        {
            var result = await controller.GetDocuments();

            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetDocument_WhenCalled_ReturnsDocumentFromDatabase(int documentId)
        {
            await controller.GetDocument(documentId);

            mockRepository.Verify(dr => dr.GetDocument(documentId));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetDocument_WhenCalledAndHasData_ReturnsOkObkectResult(int documentId)
        {
            var result = await controller.GetDocument(documentId);

            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async void GetDocument_WhenCalledAndDocumentIsNotFound_ReturnsNotFoundResult()
        {
            var result = await controller.GetDocument(999);

            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}
