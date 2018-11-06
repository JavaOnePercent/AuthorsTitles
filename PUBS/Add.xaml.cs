using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SQLite;

namespace PUBS
{
    /// <summary>
    /// Логика взаимодействия для Add.xaml
    /// </summary>
    public partial class Add : Window
    {
        public SQLiteConnection DB;

        public Add()
        {
            InitializeComponent();

            DB = new SQLiteConnection("Data Source=PUBS.db; Version=3");

            DB.Open();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            message.Content = "";
            if (name.Text == "" || surname.Text == "" || phone.Text == "")
            {
                message.Content = "* заполнены не все обязательные поля";
                return;
            }
            if (zip.Text.Length < 5)
            {
                message.Content = "Длина индекса меньше, чем нужно";
                return;
            }
            string key = rand_check_key();
            while (key == null)
            {
                key = rand_check_key();
            }
            SQLiteCommand CMD = DB.CreateCommand();
            CMD.CommandText = "insert into authors values( @au_id , @au_lname , @au_fname , @phone , @address , @city , @state , @zip , 0 )";
            CMD.Parameters.Add("@au_id", System.Data.DbType.String).Value = key;
            CMD.Parameters.Add("@au_lname", System.Data.DbType.String).Value = name.Text;
            CMD.Parameters.Add("@au_fname", System.Data.DbType.String).Value = surname.Text;
            CMD.Parameters.Add("@phone", System.Data.DbType.String).Value = phone.Text;
            
            if (address.Text == "")
                CMD.Parameters.Add("@address", System.Data.DbType.String).Value = null;
            else
                CMD.Parameters.Add("@address", System.Data.DbType.String).Value = address.Text;
            if (city.Text == "")
                CMD.Parameters.Add("@city", System.Data.DbType.String).Value = null;
            else
                CMD.Parameters.Add("@city", System.Data.DbType.String).Value = city.Text;
            if (state.Text == "")
                CMD.Parameters.Add("@state", System.Data.DbType.String).Value = null;
            else
                CMD.Parameters.Add("@state", System.Data.DbType.String).Value = state.Text;
            if (zip.Text == "")
                CMD.Parameters.Add("@zip", System.Data.DbType.String).Value = null;
            else
                CMD.Parameters.Add("@zip", System.Data.DbType.String).Value = zip.Text;
            CMD.ExecuteNonQuery(); //выполняется запрос, который не возращает каких-либо значений
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void phone_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key.ToString()[0] == 'D' && e.Key.ToString().Length == 2) || e.Key.ToString() == "OemMinus" || e.Key.ToString() == "Subtract")
            {
                e.Handled = false;
            }
            else
                e.Handled = true;
            //message.Content = e.Key.ToString();
        }

        private void zip_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString()[0] == 'D' && e.Key.ToString().Length == 2)
            {
                e.Handled = false;
            }
            else
                e.Handled = true;
        }

        public string rand_check_key()
        {
            Random rnd = new Random();
            string value = rnd.Next(100000000, 999999999).ToString();
            string key = value.Substring(0, 3) + '-' + value.Substring(3, 2) + '-' + value.Substring(5, 4);
            SQLiteCommand CMD = DB.CreateCommand();
            CMD.CommandText = "select * from authors where au_id = @key";
            CMD.Parameters.Add("@key", System.Data.DbType.String).Value = key;
            SQLiteDataReader SQL = CMD.ExecuteReader();
            if (SQL.HasRows)
            {
                return null;
            }
            return key;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DB.Close();
        }
    }
}
