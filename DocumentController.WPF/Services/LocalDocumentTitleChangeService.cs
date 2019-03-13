using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Windows;
using DocumentController.WPF.Models;

namespace DocumentController.WPF.Services
{
    public class LocalDocumentTitleChangeService : IDocumentTitleChangeService
    {
        private OleDbConnection dbConnection;
        private OleDbDataReader dataReader;
        private OleDbCommand command;

        public LocalDocumentTitleChangeService()
        {
            dbConnection = new OleDbConnection((Application.Current as App).ConnectionString);
        }

        public async Task<IEnumerable<DocumentTitleChange>> GetDocumentTitleChangeByDocumentId(int documentId)
        {
            dbConnection.Open();
            command = new OleDbCommand("SELECT * FROM DocumentTitleChanges WHERE document_id =" + documentId, dbConnection);
            dataReader = command.ExecuteReader();

            var documentTitleChanges = new List<DocumentTitleChange>();

            while (await dataReader.ReadAsync())
            {
                documentTitleChanges.Add(new DocumentTitleChange()
                {
                    Id = int.Parse(dataReader["id"].ToString()),
                    DocumentId = int.Parse(dataReader["id"].ToString()),
                    DocumentVersionId = int.Parse(dataReader["id"].ToString()),
                    OriginalDocumentTitle = dataReader["original_document_title"].ToString(),
                    NewDocumentTitle = dataReader["new_document_title"].ToString()
                });
            }

            dataReader.Close();
            dbConnection.Close();

            return documentTitleChanges;
        }

        public async Task<DocumentTitleChange> AddNewDocumentTitleChange(DocumentTitleChange documentTitleChange)
        {
            string sql = "INSERT INTO DocumentTitleChanges " +
                "(document_id, version_id, original_document_title, new_document_title) VALUES " +
                "(@document_id, @version_id, @original_document_title, @new_document_title)";

            dbConnection.Open();
            command = new OleDbCommand(sql, dbConnection);

            command.Parameters.AddWithValue("@document_id", documentTitleChange.DocumentId);
            command.Parameters.AddWithValue("@version_id", documentTitleChange.DocumentVersionId);
            command.Parameters.AddWithValue("@original_document_title", documentTitleChange.OriginalDocumentTitle);
            command.Parameters.AddWithValue("@new_document_title", documentTitleChange.NewDocumentTitle);

            await command.ExecuteNonQueryAsync();
            dbConnection.Close();

            return documentTitleChange;
        }
    }
}
