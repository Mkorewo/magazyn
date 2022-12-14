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
        string path = @"URI=file:" + Application.StartupPath + "\\SuppliesDB.db"; //sciezka pliku bazy danych
        SQLiteDataReader dataReader;
        public void WriteDB(string name, string description, string category, int amount, double price) //funkcja do zapisania danych w tabeli
        {
            
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
        public SQLiteDataReader ReadDB() //funkcja do odczytania danych z tabeli 
        {
            var con = new SQLiteConnection(@"URI=file:" + Application.StartupPath + "\\SuppliesDB.db");
            con.Open();

            string stm = "SELECT * FROM supplies";
            var cmd = new SQLiteCommand(stm, con);
            dataReader = cmd.ExecuteReader();

            return dataReader;
        }
        public SQLiteDataReader ReadDB(string query) //funkcja do odczytania danych z tabeli w podanej kategorii
        {
            var con = new SQLiteConnection(@"URI=file:" + Application.StartupPath + "\\SuppliesDB.db");
            con.Open();

            string stm = "SELECT * FROM supplies WHERE category = '" + query + "';";
            var cmd = new SQLiteCommand(stm, con);
            dataReader = cmd.ExecuteReader();

            return dataReader;
        }
        public void CreateDB() //utworzenie bazy danych jesli nie istnieje
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
        public void DeleteFromDB(string id) //usuwanie wybranego rekordu tabeli
        {
            
            var con = new SQLiteConnection(path);
            con.Open();
            var cmd = new SQLiteCommand(con);
            cmd.CommandText = "DELETE FROM supplies WHERE id='"+id+"';";

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (InvalidCastException e)
            {
                MessageBox.Show("err");
            }
            
        }
        public void EditFromDB(string name, string description, string category, int amount, double price, string id) //edytowanie danego rekordu nowo podanymi wartosciami
        {

            var con = new SQLiteConnection(path);
            con.Open();
            var cmd1 = new SQLiteCommand(con);
            //
            string stm = "SELECT * FROM supplies WHERE id = '" + id + "'";
            var cmd2 = new SQLiteCommand(stm, con);
            var dr = cmd2.ExecuteReader();
            //
            while (dr.Read())
            { 
                if (dr.GetString(1) != name)
                {
                    cmd1.CommandText = "UPDATE supplies SET Name ='" + name +"' WHERE Id = " + id + ";";
                    cmd1.ExecuteNonQuery();
                }
                if (dr.GetString(2) != description)
                {
                    cmd1.CommandText = "UPDATE supplies SET Description ='" + description + "' WHERE Id = " + id + ";";
                    cmd1.ExecuteNonQuery();
                }
                if(dr.GetString(3) != category)
                {
                    cmd1.CommandText = "UPDATE supplies SET Category ='" + category + "' WHERE Id = " + id + ";";
                    cmd1.ExecuteNonQuery();
                }
                if(dr.GetInt16(4) != amount)
                {
                    cmd1.CommandText = "UPDATE supplies SET Amount ='" + amount + "' WHERE Id = " + id + ";";
                    cmd1.ExecuteNonQuery();
                }
                if(dr.GetDouble(5) != price)
                {
                    cmd1.CommandText = "UPDATE supplies SET Price ='" + price + "' WHERE Id = " + id + ";";
                    cmd1.ExecuteNonQuery();
                }

            }

        }

    }
}