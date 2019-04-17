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
    public class LocalAdminUserService : IAdminUserService
    {
        private OleDbConnection dbConnection;
        private OleDbDataReader dataReader;
        private OleDbCommand command;

        public LocalAdminUserService()
        {
            dbConnection = new OleDbConnection((Application.Current as App).ConnectionString);
        }

        public async Task<bool> IsAdmin(string username)
        {
            dbConnection.Open();
            command = new OleDbCommand("SELECT * from AdminUsers", dbConnection);

            dataReader = command.ExecuteReader();

            var adminUsers = new List<AdminUser>();

            while (await dataReader.ReadAsync())
            {
                adminUsers.Add(new AdminUser()
                {
                    Id = int.Parse(dataReader["ID"].ToString()),
                    Username = dataReader["Username"].ToString(),
                    IsAdmin = dataReader["IsAdmin"].ToString()
                });
            }

            dataReader.Close();
            dbConnection.Close();

            var result = adminUsers.SingleOrDefault(au => au.Username.ToLower() == username.ToLower() && au.IsAdmin.ToLower() == "true");

            if (result == null)
                return false;

            return true;
        }
    }
}
