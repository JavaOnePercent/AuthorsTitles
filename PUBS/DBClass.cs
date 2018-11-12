using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PUBS
{
    class DBClass
    {
        private SQLiteConnection DB; //объект подключения к базе

        public DBClass()
        {
            DB = new SQLiteConnection("Data Source=PUBS.db; Version=3"); //connect к базе
            DB.Open(); //открытие connect'а
        }

        public DataTable getDataTableAuthors()
        {
            DataTable authors = new DataTable();
            SQLiteCommand CMD = DB.CreateCommand();
            CMD.CommandText = "select au_id, au_lname, au_fname from authors";
            SQLiteDataAdapter SQL = new SQLiteDataAdapter(CMD); 
            SQL.Fill(authors);
            return authors;
        }

        public DataTable getDataTableAuthorsById(String id)
        {
            DataTable authors = new DataTable();
            SQLiteCommand CMD = DB.CreateCommand(); //создание команды
            CMD.CommandText = "select * from authors where au_id = @au_id"; //создание запроса
            CMD.Parameters.Add("@au_id", System.Data.DbType.String).Value = id; //добавление параметра
            SQLiteDataAdapter SQL = new SQLiteDataAdapter(CMD); //адаптер данных
            SQL.Fill(authors); //заполнение данными DataTable 
            return authors;
        }

        public bool checkExistsAuthorById(String id)
        {
            DataTable author = getDataTableAuthorsById(id);
            if (author.Rows.Count == 0)
                return true;
            return false;
        }

        public void addAuthor(Authors author)
        {
            SQLiteCommand CMD = DB.CreateCommand();
            CMD.CommandText = "insert into authors values( @au_id , @au_lname , @au_fname , @phone , @address , @city , @state , @zip , 0 )";

            CMD.Parameters.Add("@au_id", System.Data.DbType.String).Value = author.Id;
            CMD.Parameters.Add("@au_lname", System.Data.DbType.String).Value = author.Name;
            CMD.Parameters.Add("@au_fname", System.Data.DbType.String).Value = author.Surname;
            CMD.Parameters.Add("@phone", System.Data.DbType.String).Value = author.Phone;
            CMD.Parameters.Add("@address", System.Data.DbType.String).Value = author.Address;
            CMD.Parameters.Add("@city", System.Data.DbType.String).Value = author.City;
            CMD.Parameters.Add("@state", System.Data.DbType.String).Value = author.State;
            CMD.Parameters.Add("@zip", System.Data.DbType.String).Value = author.Zip;

            CMD.ExecuteNonQuery(); //выполняется запрос, который не возращает каких-либо значений
        }

        public void updateAuthorById(Authors authors, string Id)
        {
            SQLiteCommand CMD = DB.CreateCommand();
            CMD.CommandText = "update authors set au_lname = @au_lname, au_fname = @au_fname, phone = @phone, address = @address, city = @city, state = @state, zip = @zip WHERE au_id = @au_id";

            CMD.Parameters.Add("@au_lname", System.Data.DbType.String).Value = authors.Name;
            CMD.Parameters.Add("@au_fname", System.Data.DbType.String).Value = authors.Surname;
            CMD.Parameters.Add("@phone", System.Data.DbType.String).Value = authors.Phone;
            CMD.Parameters.Add("@address", System.Data.DbType.String).Value = authors.Address;
            CMD.Parameters.Add("@city", System.Data.DbType.String).Value = authors.City;
            CMD.Parameters.Add("@state", System.Data.DbType.String).Value = authors.State;
            CMD.Parameters.Add("@zip", System.Data.DbType.String).Value = authors.Zip;
            CMD.Parameters.Add("@au_id", System.Data.DbType.String).Value = Id;

            CMD.ExecuteNonQuery(); //выполняется запрос, который не возращает каких-либо значений

        }

        public void deleteAuthorById(string Id)
        {
            SQLiteCommand CMD = DB.CreateCommand();
            CMD.CommandText = "delete from authors where au_id = @au_id";
            CMD.Parameters.Add("@au_id", System.Data.DbType.String).Value = Id;
            CMD.ExecuteNonQuery();
        }

        public DataTable getDataTableTitlesByIdAuthor(string Id)
        {
            DataTable titles = new DataTable();
            SQLiteCommand CMD = DB.CreateCommand();
            CMD.CommandText = "select t.title_id, title, type, price, advance, royalty, ytd_sales, date(pubdate) from titles as t inner join titleauthor as ta on ta.title_id = t.title_id inner join authors as a on a.au_id = ta.au_id where ta.au_id = @au_id";
            CMD.Parameters.Add("@au_id", System.Data.DbType.String).Value = Id;
            SQLiteDataAdapter SQL = new SQLiteDataAdapter(CMD);
            SQL.Fill(titles);
            return titles;
        }

        public string getDescriptionByIdTitle(string Id)
        {
            string description = "";
            SQLiteCommand CMD = DB.CreateCommand();
            CMD.CommandText = "select notes from titles where title_id = @title_id";
            CMD.Parameters.Add("@title_id", System.Data.DbType.String).Value = Id;
            SQLiteDataReader SQL = CMD.ExecuteReader(); //адаптер данных, который читает результат запроса построчно
            if (SQL.HasRows) //если запрос возвращает строки
            {
                while (SQL.Read()) //пока есть строки
                {
                    description = SQL["notes"].ToString();
                }
            }
            return description;
        }

        public DataTable getDataTableSalesByIdTitle(string Id)
        {
            DataTable sales = new DataTable();
            SQLiteCommand CMD = DB.CreateCommand();
            CMD.CommandText = "select qty, date(ord_date) from sales where title_id = @title_id";
            CMD.Parameters.Add("@title_id", System.Data.DbType.String).Value = Id;
            SQLiteDataAdapter SQL = new SQLiteDataAdapter(CMD);
            SQL.Fill(sales);
            return sales;
        }

        public void closeConnection()
        {
            DB.Close(); //закрытие connect'а к базе
        }
    }
}
