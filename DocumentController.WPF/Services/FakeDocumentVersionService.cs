﻿using DocumentController.WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentController.WPF.Services
{
    public class FakeDocumentVersionService : IDocumentVersionService
    {
        private readonly IList<DocumentVersion> documentVersions;

        public FakeDocumentVersionService()
        {
            documentVersions = new List<DocumentVersion>()
            {
                new DocumentVersion()
                {
                    Id = 1,
                    DocumentId = 1,
                    VersionNumber = "1",
                    Progress = Progress.Obsolete,
                    EffectiveDate = DateTime.Today.AddDays(-20)
                },
                new DocumentVersion()
                {
                    Id = 2,
                    DocumentId = 2,
                    VersionNumber = "2",
                    Progress = Progress.InEffect,
                    EffectiveDate = DateTime.Today.AddDays(-2)
                },
                new DocumentVersion()
                {
                    Id = 3,
                    DocumentId = 3,
                    VersionNumber = "3",
                    Progress = Progress.InEffect,
                    EffectiveDate = DateTime.Today.AddDays(-5)
                },
                new DocumentVersion()
                {
                    Id = 4,
                    DocumentId = 1,
                    VersionNumber = "2",
                    Progress = Progress.InEffect,
                    EffectiveDate = DateTime.Today.AddDays(-1)
                },
            };
        }

        public async Task<IEnumerable<DocumentVersion>> GetAllVersionsByDocumentId(int documentId)
        {
            return await Task.Run(() =>
            {
                var results = documentVersions.Where(dv => dv.DocumentId == documentId && dv.IsRemoved != "true");
                if (results == null)
                    return null;
                return results;
            });
        }

        public async Task<DocumentVersion> AddNewDocumentVersion(DocumentVersion documentVersion)
        {
            return await Task.Run(() =>
            {
                documentVersion.Id = documentVersions.Count + 1;
                documentVersions.Add(documentVersion);
                return documentVersion;
            });
        }

        public async Task<DocumentVersion> UpdateDocumentVersion(DocumentVersion documentVersion)
        {
            return await Task.Run(() => documentVersion);
        }

        public async void RemoveDocumentVersion(DocumentVersion documentVersion)
        {
        }
    }
}
