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

        public async Task<DocumentTitleChange> GetDocumentTitleChangeByDocumentIdAndDocumentVersionId(int documentId, int documentVersionId)
        {
            dbConnection.Open();
            command = new OleDbCommand("SELECT * FROM DocumentTitleChanges WHERE document_id =" + documentId + " AND version_id =" + documentVersionId, dbConnection);
            dataReader = command.ExecuteReader();

            var documentTitleChange = new DocumentTitleChange();

            while (await dataReader.ReadAsync())
            {
                documentTitleChange.Id = int.Parse(dataReader["id"].ToString());
                documentTitleChange.DocumentId = int.Parse(dataReader["document_id"].ToString());
                documentTitleChange.DocumentVersionId = int.Parse(dataReader["version_id"].ToString());
                documentTitleChange.OriginalDocumentTitle = dataReader["original_document_title"].ToString();
                documentTitleChange.NewDocumentTitle = dataReader["new_document_title"].ToString();
            }

            dataReader.Close();
            dbConnection.Close();

            return documentTitleChange;
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
