using System;
using DocumentController.WebAPI.Controllers;
using Xunit;
using Moq;
using DocumentController.WebAPI.Persistence;

namespace DocumentController.WebAPI.Tests
{
    public class DocumentsControllerUnitTest
    {
        public DocumentsControllerUnitTest()
        {
            var stubDocumentRepository = new Mock<IDocumentRepository>();
            var stubUnitOfWork = new Mock<IUnitOfWork>();
            var documentController = new DocumentsController(stubDocumentRepository.Object, stubUnitOfWork.Object);
        }

        [Fact]
        public void Test1()
        {

        }
    }
}
