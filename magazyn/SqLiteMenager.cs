using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Diagnostics;

namespace magazyn
{
    internal class SqLiteMenager
    {
        string path = @"URI=file:" + Application.StartupPath + "\\SuppliesDB.db";
        SQLiteDataReader dataReader;
        public void WriteDB(string name, string description, string category, int amount, double price)
        {
            //string query = "INSERT INTO supplies VALUES(null,'" + name + "','" + description + "','" + category + "'," + amount + "," + price+");";
            
            var con = new SQLiteConnection(path);
            con.Open();
            var cmd = new SQLiteCommand(con);
            cmd.CommandText = "INSERT INTO supplies(id,name,description,category,amount,price) VALUES(@Id,@Name,@Description,@Category,@Amount,@Price)";

            cmd.Parameters.AddWithValue("@Id", null);
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@Description", description);
            cmd.Parameters.AddWithValue("@Category", category);
            cmd.Parameters.AddWithValue("@Amount", amount);
            cmd.Parameters.AddWithValue("@Price", price);

            cmd.ExecuteNonQuery();
              
        }
        public SQLiteDataReader ReadDB()
        {
            var con = new SQLiteConnection(@"URI=file:" + Application.StartupPath + "\\SuppliesDB.db");
            con.Open();

            string stm = "SELECT * FROM supplies";
            var cmd = new SQLiteCommand(stm, con);
            dataReader = cmd.ExecuteReader();

            return dataReader;
        }
        public SQLiteDataReader ReadDB(string query)
        {
            var con = new SQLiteConnection(@"URI=file:" + Application.StartupPath + "\\SuppliesDB.db");
            con.Open();

            string stm = "SELECT * FROM supplies WHERE category = '" + query + "'";
            var cmd = new SQLiteCommand(stm, con);
            dataReader = cmd.ExecuteReader();

            return dataReader;
        }
        public void CreateDB()
        {
            if (!System.IO.File.Exists("SuppliesDB.db"))
            {
                SQLiteConnection.CreateFile("SuppliesDB.db");
                using (var sqlite = new SQLiteConnection(@"Data Source=" + "SuppliesDB.db"))
                {
                    sqlite.Open();
                    string sql = "CREATE TABLE 'supplies' ('Id'INTEGER NOT NULL UNIQUE," +
                        "'Name'TEXT NOT NULL,'Description'TEXT,'Category'TEXT NOT NULL," +
                        "'Amount'INTEGER NOT NULL,'Price'NUMERIC NOT NULL," +
                        "PRIMARY KEY('Id' AUTOINCREMENT));";
                    SQLiteCommand command = new SQLiteCommand(sql, sqlite);
                    command.ExecuteNonQuery();
                }
            }
        }

    }
}
