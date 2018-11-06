using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Data.SQLite;

namespace PUBS
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public SQLiteConnection DB;

        public MainWindow()
        {
            InitializeComponent();
            DB = new SQLiteConnection("Data Source=PUBS.db; Version=3");
            DB.Open();

            init_authors();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DB.Close();
        }

        private void authors_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            sales.Columns.Clear();
            description.Content = "";
            SQLiteCommand CMD = DB.CreateCommand();
            CMD.CommandText = "select * from titles as t inner join titleauthor as ta on ta.title_id = t.title_id inner join authors as a on a.au_id = ta.au_id where ta.au_id = @au_id";
            string[] names = authors.SelectedItem.ToString().Split(' ');
            CMD.Parameters.Add("@au_id", System.Data.DbType.String).Value = names[3];
            SQLiteDataReader SQL = CMD.ExecuteReader();
            List<Titles> result = new List<Titles>();
            if (SQL.HasRows)
            {
                while (SQL.Read())
                {
                    result.Add(new Titles(SQL["title"].ToString(), SQL["type"].ToString(), SQL["price"].ToString(), SQL["advance"].ToString(), SQL["royalty"].ToString(), SQL["ytd_sales"].ToString(), SQL["pubdate"].ToString()));
                }
            }
            titles.DataContext = result;
        }

        private void titles_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                titles = sender as DataGrid;
                if (titles != null && titles.SelectedItems != null && titles.SelectedItems.Count == 1)
                {

                    DataGridRow dgr = titles.ItemContainerGenerator.ContainerFromItem(titles.SelectedItem) as DataGridRow;
                    Titles tit = (Titles)dgr.DataContext;

                    SQLiteCommand CMD = DB.CreateCommand();
                    CMD.CommandText = "select *, t.notes from sales as s inner join titles as t on t.title_id = s.title_id where t.title = @title";
                    CMD.Parameters.Add("@title", System.Data.DbType.String).Value = tit.Title;
                    SQLiteDataReader SQL = CMD.ExecuteReader();
                    List<Sales> result = new List<Sales>();
                    String notes = "";
                    if (SQL.HasRows)
                    {
                        while (SQL.Read())
                        {
                            result.Add(new Sales(SQL["ord_date"].ToString(), SQL["qty"].ToString()));
                            notes = SQL["notes"].ToString();
                        }
                    }
                    description.Content = notes;
                    sales.ItemsSource = result;
                }
            }
        }

        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            titles.Columns.Clear();
            sales.Columns.Clear();
            description.Content = "";
            SQLiteCommand CMD = DB.CreateCommand();
            CMD.CommandText = "delete from authors where au_id = @au_id";
            string[] names = authors.SelectedItem.ToString().Split(' ');
            CMD.Parameters.Add("@au_id", System.Data.DbType.String).Value = names[3];
            CMD.ExecuteNonQuery();
            authors.Items.Clear();
            init_authors();
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            Add add = new Add();
            add.Show();
            this.Close();
        }

        public void init_authors()
        {
            SQLiteCommand CMD = DB.CreateCommand();
            CMD.CommandText = "select * from authors";
            SQLiteDataReader SQL = CMD.ExecuteReader();
            if (SQL.HasRows)
            {
                while (SQL.Read())
                {
                    ListBoxItem itm = new ListBoxItem();
                    itm.Content = SQL["au_lname"] + " " + SQL["au_fname"] + " " + SQL["au_id"];
                    authors.Items.Add(itm);
                    itm.ToolTip = "Phone: " + SQL["phone"] + "\nAddress: " + SQL["address"] + "\nCity: " + SQL["city"] + "\nState: " + SQL["state"] + "\nZip: " + SQL["zip"] + "\nContract: " + SQL["contract"];
                }
            }
        }
    }
}
