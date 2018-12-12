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
    public class DocumentVersionsController : ControllerBase
    {
        private readonly IDocumentVersionRepository documentVersionRepository;
        private readonly IUnitOfWork unitOfWork;
        public DocumentVersionsController(IDocumentVersionRepository documentVersionRepository, IUnitOfWork unitOfWork)
        {
            this.documentVersionRepository = documentVersionRepository;
            this.unitOfWork = unitOfWork;
        }

        [HttpGet("document/{documentId}")]
        public async Task<ActionResult<IEnumerable<DocumentVersion>>> GetDocumentVersionsByDocumentId(int documentId)
        {
            var documentVersions = await documentVersionRepository.GetAllDocumentVersionsByDocumentId(documentId);

            if (documentVersions == null)
                return NotFound();

            return Ok(documentVersions);
        }

        [HttpGet("{documentVersionId}")]
        public async Task<ActionResult<DocumentVersion>> GetDocumentVersion(int documentVersionId)
        {
            var documentVersion = await documentVersionRepository.GetDocumentVersion(documentVersionId);

            if (documentVersion == null)
                return NotFound();

            return Ok(documentVersion);
        }

        [HttpPost]
        public async Task<ActionResult<DocumentVersion>> AddNewDocumentVersion(DocumentVersion documentVersion)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await documentVersionRepository.AddNewDocumentVersion(documentVersion);
            await unitOfWork.CompleteAsync();

            return CreatedAtAction(nameof(GetDocumentVersion), new { documentVersionId = documentVersion.Id }, documentVersion);
        }

        [HttpPut("{documentVersionId}")]
        public async Task<ActionResult<DocumentVersion>> UpdateDocumentVersion(int documentVersionId, DocumentVersion documentVersion)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (documentVersionId != documentVersion.Id)
                return BadRequest();

            var result = await documentVersionRepository.UpdateDocumentVersion(documentVersion);
            if (result == null)
                return NotFound();

            await unitOfWork.CompleteAsync();

            return Ok(documentVersion);
        }

        [HttpDelete("{documentVersionId}")]
        public async Task<ActionResult<DocumentVersion>> RemoveDocumentVersion(int documentVersionId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await documentVersionRepository.RemoveDocumentVersion(documentVersionId);
            await unitOfWork.CompleteAsync();

            return Ok(result);
        }
    }
}