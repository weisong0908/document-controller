using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Windows;

namespace DocumentController.WPF.Services
{
    public class LocalDocumentTitleChangeService
    {
        private OleDbConnection dbConnection;
        private OleDbDataReader dataReader;
        private OleDbCommand command;

        public LocalDocumentTitleChangeService()
        {
            dbConnection = new OleDbConnection((Application.Current as App).ConnectionString);
        }
    }
}
