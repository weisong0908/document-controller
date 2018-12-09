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
    public class DocumentsController : ControllerBase
    {
        private readonly DocumentControllerDbContext dbContext;
        public DocumentsController(DocumentControllerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Document>>> GetDocuments()
        {
            var documents = await dbContext.Documents.ToListAsync();

            if(documents==null)
                return NotFound();

            return documents;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Document>> GetDocument(int id)
        {
            if(!ModelState.IsValid)
                return BadRequest();

            var document = await dbContext.Documents.Include(d => d.DocumentVersions).SingleOrDefaultAsync(d => d.Id == id);
            if (document == null)
                return NotFound();

            return document;
        }
    }
}