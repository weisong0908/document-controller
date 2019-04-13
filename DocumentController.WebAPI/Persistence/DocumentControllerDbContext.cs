using System;
using DocumentController.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentController.WebAPI.Persistence
{
    public class DocumentControllerDbContext : DbContext
    {
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentVersion> DocumentVersions { get; set; }

        public DocumentControllerDbContext(DbContextOptions options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Document>().ToTable("documents");
            modelBuilder.Entity<Document>().Property(d => d.DocumentNumber).HasColumnName("document_number");
            modelBuilder.Entity<Document>().Property(d => d.Department).HasColumnName("department");
            modelBuilder.Entity<Document>().Property(d => d.Function).HasColumnName("function");
            modelBuilder.Entity<Document>().Property(d => d.Id).HasColumnName("id");
            modelBuilder.Entity<Document>().Property(d => d.Location).HasColumnName("location");
            modelBuilder.Entity<Document>().Property(d => d.Status).HasColumnName("status");
            modelBuilder.Entity<Document>().Property(d => d.Title).HasColumnName("title");
            modelBuilder.Entity<Document>().Property(d => d.Type).HasColumnName("type");
            modelBuilder.Entity<Document>().Property(d => d.IsRemoved).HasColumnName("is_removed");

            modelBuilder.Entity<DocumentVersion>().ToTable("document_versions");
            modelBuilder.Entity<DocumentVersion>().Property(dv => dv.DescriptionOfChange).HasColumnName("description_of_change");
            modelBuilder.Entity<DocumentVersion>().Property(dv => dv.DocumentId).HasColumnName("document_id");
            modelBuilder.Entity<DocumentVersion>().Property(dv => dv.EffectiveDate).HasColumnName("effective_date").HasColumnType("date");
            modelBuilder.Entity<DocumentVersion>().Property(dv => dv.Id).HasColumnName("id");
            modelBuilder.Entity<DocumentVersion>().Property(dv => dv.IsRemoved).HasColumnName("is_removed");
            modelBuilder.Entity<DocumentVersion>().Property(dv => dv.Location_Editable).HasColumnName("location_editable");
            modelBuilder.Entity<DocumentVersion>().Property(dv => dv.Location_PDF).HasColumnName("location_pdf");
            modelBuilder.Entity<DocumentVersion>().Property(dv => dv.Progress).HasColumnName("progress");
            modelBuilder.Entity<DocumentVersion>().Property(dv => dv.PurposeOfChange).HasColumnName("purpose_of_change");
            modelBuilder.Entity<DocumentVersion>().Property(dv => dv.Remarks).HasColumnName("remarks");
            modelBuilder.Entity<DocumentVersion>().Property(dv => dv.Requestor).HasColumnName("requestor");
            modelBuilder.Entity<DocumentVersion>().Property(dv => dv.VersionNumber).HasColumnName("version_number");

            SeedDatabase(modelBuilder);
        }

        private static void SeedDatabase(ModelBuilder modelBuilder)
        {
            var document = new Document()
            {
                Id = 1,
                DocumentNumber = "DOC-01"
            };

            var documentVersion1 = new DocumentVersion()
            {
                Id = 1,
                VersionNumber = "1",
                EffectiveDate = DateTime.Today.AddDays(-10),
                DocumentId = 1
            };

            var documentVersion2 = new DocumentVersion()
            {
                Id = 2,
                VersionNumber = "2",
                EffectiveDate = DateTime.Today,
                DocumentId = 1
            };

            modelBuilder.Entity<Document>().HasData(document);
            modelBuilder.Entity<DocumentVersion>().HasData(documentVersion1);
            modelBuilder.Entity<DocumentVersion>().HasData(documentVersion2);
        }
    }
}