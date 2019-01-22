using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DocumentController.WPF.Models;

namespace DocumentController.WPF.Services
{
    public class LocalDocumentService : IDocumentService
    {
        private OleDbConnection dbConnection;
        private OleDbDataReader dataReader;
        private OleDbCommand command;

        public LocalDocumentService()
        {
            dbConnection = new OleDbConnection((Application.Current as App).ConnectionString);
        }

        public async Task<IEnumerable<Document>> GetDocuments()
        {
            dbConnection.Open();
            command = new OleDbCommand("SELECT ID, Document_No, Document_Title, Department, Function, Document_Type, Document_Status FROM Documents", dbConnection);

            dataReader = command.ExecuteReader();

            var documents = new List<Document>();

            while(await dataReader.ReadAsync())
            {
                documents.Add(new Document()
                {
                    Id = int.Parse(dataReader["ID"].ToString()),
                    DocumentNumber = (dataReader["Document_No"].ToString()),
                    Title = (dataReader["Document_Title"].ToString()),
                    Department = (dataReader["Department"].ToString()),
                    Function = (dataReader["Function"].ToString()),
                    Type = (dataReader["Document_Type"].ToString()),
                    Status = (dataReader["Document_Status"].ToString())
                });
            }

            dataReader.Close();
            dbConnection.Close();

            return documents.Where(d => d.Status == DocumentStatus.Active);
        }

        public async Task<Document> AddNewDocument(Document document)
        {
            string sql = "INSERT INTO Documents " +
                "(Document_No, Document_Title, Department, Function, Document_Type, Document_Status) VALUES " +
                "(@DocumentNumber, @DocumentTitle, @Department, @Function, @DocumentType, @DocumentStatus)";

            dbConnection.Open();
            command = new OleDbCommand(sql, dbConnection);

            OleDbParameter documentNumber = new OleDbParameter("@DocumentNumber", document.DocumentNumber ?? string.Empty);
            OleDbParameter documentTitle = new OleDbParameter("@DocumentTitle", document.Title);
            OleDbParameter department = new OleDbParameter("@Department", document.Department);
            OleDbParameter function = new OleDbParameter("@Function", document.Function ?? string.Empty);
            OleDbParameter documentType = new OleDbParameter("@DocumentType", document.Type);
            OleDbParameter documentStatus = new OleDbParameter("@DocumentStatus", document.Status);

            command.Parameters.Add(documentNumber);
            command.Parameters.Add(documentTitle);
            command.Parameters.Add(department);
            command.Parameters.Add(function);
            command.Parameters.Add(documentType);
            command.Parameters.Add(documentStatus);

            await command.ExecuteNonQueryAsync();

            var result = await GetDocument(document);

            dbConnection.Close();

            return result;
        }

        private async Task<Document> GetDocument(Document document)
        {
            string sql = $"SELECT ID, Document_No, Document_Title, Department, Function, Document_Type, Document_Status FROM Documents WHERE Document_Title = '{document.Title}' AND Document_type = '{document.Type}'";
            command = new OleDbCommand(sql, dbConnection);
            dataReader = command.ExecuteReader();

            var result = new Document();

            while(await dataReader.ReadAsync())
            {
                result.Id = int.Parse(dataReader["ID"].ToString());
                result.DocumentNumber = (dataReader["Document_No"].ToString());
                result.Title = (dataReader["Document_Title"].ToString());
                result.Department = (dataReader["Department"].ToString());
                result.Function = (dataReader["Function"].ToString());
                result.Type = (dataReader["Document_Type"].ToString());
                result.Status = (dataReader["Document_Status"].ToString());
            }

            dataReader.Close();

            return result;
        }

        public async void RemoveDocument(Document document)
        {
            string sql = $"UPDATE Documents SET Document_Status = @Status WHERE ID = {document.Id}";

            dbConnection.Open();
            command = new OleDbCommand(sql, dbConnection);

            command.Parameters.AddWithValue("@Status", DocumentStatus.Rescinded);

            await command.ExecuteNonQueryAsync();
            dbConnection.Close();
        }
    }
}
