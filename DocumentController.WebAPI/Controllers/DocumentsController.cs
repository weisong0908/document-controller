using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentController.WebAPI.Migrations;
using DocumentController.WebAPI.Models;
using DocumentController.WebAPI.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace DocumentController.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentRepository documentRepository;
        public DocumentsController(IDocumentRepository documentRepository)
        {
            this.documentRepository = documentRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Document>>> GetDocuments()
        {
            var documents = await documentRepository.GetAllDocuments();

            if (documents == null)
                return NotFound();

            return Ok(documents);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Document>> GetDocument(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var document = await documentRepository.GetDocument(id);

            if (document == null)
                return NotFound();

            return Ok(document);
        }
    }
}