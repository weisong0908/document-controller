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
            command = new OleDbCommand("SELECT ID, Document_No, Document_Title, Department, Document_type, Document_Status FROM Documents", dbConnection);

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
                    Type = (dataReader["Document_type"].ToString()),
                    Status = (dataReader["Document_Status"].ToString())
                });
            }

            dataReader.Close();
            dbConnection.Close();

            return documents.Where(d => d.Status == DocumentStatus.Active);
        }
    }
}
