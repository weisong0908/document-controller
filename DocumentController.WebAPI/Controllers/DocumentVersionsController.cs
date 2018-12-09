using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentController.WebAPI.Migrations;
using DocumentController.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DocumentController.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentVersionsController : ControllerBase
    {
        private readonly DocumentControllerDbContext dbContext;
        public DocumentVersionsController(DocumentControllerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet("document/{documentId}")]
        public async Task<ActionResult<IEnumerable<DocumentVersion>>> GetDocumentVersionsByDocumentId(int documentId)
        {
            var documentVersions = await dbContext.DocumentVersions.Where(dv => dv.DocumentId == documentId).ToListAsync();

            if (documentVersions == null)
                return NotFound();

            return documentVersions;
        }

        [HttpGet("{documentVersionId}")]
        public async Task<ActionResult<DocumentVersion>> GetDocumentVersion(int documentVersionId)
        {
            var documentVersion = await dbContext.DocumentVersions.SingleOrDefaultAsync(dv => dv.Id == documentVersionId);

            if (documentVersion == null)
                return NotFound();

            return documentVersion;
        }

        [HttpPost]
        public async Task<ActionResult<DocumentVersion>> AddNewDocumentVersion(DocumentVersion documentVersion)
        {
            dbContext.DocumentVersions.Add(documentVersion);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDocumentVersion), new { documentVersionId = documentVersion.Id }, documentVersion);
        }

        [HttpPut("{documentVersionId}")]
        public async Task<ActionResult<DocumentVersion>> UpdateDocumentVersion(int documentVersionId, DocumentVersion documentVersion)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (documentVersionId != documentVersion.Id)
                return BadRequest();

            var documentVersionInDb = await dbContext.DocumentVersions.SingleOrDefaultAsync(dv => dv.Id == documentVersionId);
            if (documentVersionInDb == null)
                return NotFound();
            dbContext.Entry(documentVersionInDb).State = EntityState.Detached;

            dbContext.DocumentVersions.Update(documentVersion);
            await dbContext.SaveChangesAsync();

            return documentVersion;
        }

        [HttpDelete("{documentVersionId}")]
        public async Task<ActionResult<DocumentVersion>> RemoveDocumentVersion(int documentVersionId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var documentVersion = await dbContext.DocumentVersions.SingleOrDefaultAsync(dv => dv.Id == documentVersionId);

            if (documentVersion == null)
                return NotFound();

            dbContext.DocumentVersions.Remove(documentVersion);
            await dbContext.SaveChangesAsync();

            return documentVersion;
        }
    }
}