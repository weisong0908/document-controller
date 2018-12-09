using DocumentController.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentController.WebAPI.Migrations
{
    public class DocumentControllerDbContext : DbContext
    {
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentVersion> DocumentVersions { get; set; }

        public DocumentControllerDbContext(DbContextOptions options) : base(options)
        { }
    }
}