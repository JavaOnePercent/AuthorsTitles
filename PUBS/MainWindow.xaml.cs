using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PUBS
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private DBClass dbClass;

        public MainWindow()
        {
            InitializeComponent();
            dbClass = new DBClass();
            init_authors();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            dbClass.closeConnection();
        }

        private void authors_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            sales.Columns.Clear();
            description.Content = "";
            string[] names = authors.SelectedItem.ToString().Split(' ');
            DataTable allTitles = dbClass.getDataTableTitlesByIdAuthor(names[2]);
            titles.DataContext = allTitles;

            viewAuthorLabel.Content = names[0] + "\n" + names[1];

            authorsLabel.Visibility = Visibility.Collapsed;
            viewAuthorLabel.Visibility = Visibility.Visible;
        }

        private void titles_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            object title = titles.SelectedItem;
            string title_id = (titles.SelectedCells[0].Column.GetCellContent(title) as TextBlock).Text;
            DataTable allSales = dbClass.getDataTableSalesByIdTitle(title_id);
            String notes = dbClass.getDescriptionByIdTitle(title_id);
            description.Content = notes;
            sales.DataContext = allSales;

        }

        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            titles.Columns.Clear();
            sales.Columns.Clear();
            description.Content = "";
            string[] names = authors.SelectedItem.ToString().Split(' ');
            dbClass.deleteAuthorById(names[2]);
            init_authors();
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            if (authors.IsVisible)
            {
                authors.Visibility = Visibility.Collapsed;
                authorsLabel.Visibility = Visibility.Collapsed;
                viewAuthorLabel.Visibility = Visibility.Collapsed;
                authorsPanel.Visibility = Visibility.Visible;
                newAuthorLabel.Visibility = Visibility.Visible;
                backButton.Visibility = Visibility.Visible;

                titles.Columns.Clear();
                sales.Columns.Clear();
                description.Content = "";

                return;
            }
            message.Content = "";
            if (name.Text == "" || surname.Text == "")
            {
                message.Content = "* заполнены не все обязательные поля";
                return;
            }
            if (zip.Text != "" && zip.Text.Length < 5)
            {
                message.Content = "Длина индекса меньше, чем нужно";
                return;
            }
            string key = rand_check_key();
            while (key == null)
            {
                key = rand_check_key();
            }
           
            String cur_phone = "UNKNOWN";
            String cur_address = null;
            String cur_city = null;
            String cur_state = null;
            String cur_zip = null;

            if (phone.Text != "")
                cur_phone = phone.Text;
            if (address.Text != "")
                cur_address = address.Text;
            if (city.Text != "")
                cur_city = city.Text;
            if (state.Text != "")
                cur_state = state.Text;
            if (zip.Text != "")
                cur_zip = zip.Text;

            Authors author = new Authors(key, name.Text, surname.Text, cur_phone, cur_address, cur_city, cur_state, cur_zip);
            dbClass.addAuthor(author);
           
            authors.Visibility = Visibility.Visible;
            authorsLabel.Visibility = Visibility.Visible;
            authorsPanel.Visibility = Visibility.Collapsed;
            newAuthorLabel.Visibility = Visibility.Collapsed;
            backButton.Visibility = Visibility.Collapsed;

            name.Text = "";
            surname.Text = "";
            phone.Text = "";
            address.Text = "";
            city.Text = "";
            state.Text = "";
            zip.Text = "";
            message.Content = "";

            init_authors();

        }

        public void init_authors()
        {
            authors.Items.Clear();

            DataTable allAuthors = dbClass.getDataTableAuthors();

            for (int i = 0; i < allAuthors.Rows.Count; i++)
            {
                authors.Items.Add(allAuthors.Rows[i]["au_lname"] + " " + allAuthors.Rows[i]["au_fname"] + " " + allAuthors.Rows[i]["au_id"]);
            }
        }
        private void phone_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key.ToString()[0] == 'D' && e.Key.ToString().Length == 2) || e.Key.ToString() == "OemMinus" || e.Key.ToString() == "Subtract")
            {
                e.Handled = false;
            }
            else
                e.Handled = true;
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
            if (dbClass.checkExistsAuthorById(key))
                return key;
            return null;
        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            dbClass.closeConnection();
            Environment.Exit(0);
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            authorsPanel.Visibility = Visibility.Collapsed;
            newAuthorLabel.Visibility = Visibility.Collapsed;
            updateAuthorLabel.Visibility = Visibility.Collapsed;
            backButton.Visibility = Visibility.Collapsed;
            authors.Visibility = Visibility.Visible;
            authorsLabel.Visibility = Visibility.Visible;

            if (!addButton.IsVisible)
            {
                addButton.Visibility = Visibility.Visible;
                viewAuthorLabel.Visibility = Visibility.Collapsed;
                name.IsReadOnly = false;
                surname.IsReadOnly = false;
                phone.IsReadOnly = false;
                address.IsReadOnly = false;
                city.IsReadOnly = false;
                state.IsReadOnly = false;
                zip.IsReadOnly = false;
            }
            if (updateButton.IsVisible)
            {
                updateButton.Visibility = Visibility.Collapsed;
                updateAuthorLabel.Visibility = Visibility.Collapsed;
                addButton.Visibility = Visibility.Visible;
            }

            name.Text = "";
            surname.Text = "";
            phone.Text = "";
            address.Text = "";
            city.Text = "";
            state.Text = "";
            zip.Text = "";
            message.Content = "";

            titles.Columns.Clear();
            sales.Columns.Clear();
            description.Content = "";
        }

        private void MenuItemUpdate_Click(object sender, RoutedEventArgs e)
        {
            authors.Visibility = Visibility.Collapsed;
            authorsLabel.Visibility = Visibility.Collapsed;
            viewAuthorLabel.Visibility = Visibility.Collapsed;
            addButton.Visibility = Visibility.Collapsed;
            authorsPanel.Visibility = Visibility.Visible;
            updateAuthorLabel.Visibility = Visibility.Visible;
            backButton.Visibility = Visibility.Visible;
            updateButton.Visibility = Visibility.Visible;

            titles.Columns.Clear();
            sales.Columns.Clear();
            description.Content = "";

            string[] names = authors.SelectedItem.ToString().Split(' ');
            DataTable author = dbClass.getDataTableAuthorsById(names[2]);
            name.Text = author.Rows[0]["au_lname"].ToString();
            surname.Text = author.Rows[0]["au_fname"].ToString();
            phone.Text = author.Rows[0]["phone"].ToString();
            address.Text = author.Rows[0]["address"].ToString();
            city.Text = author.Rows[0]["city"].ToString();
            state.Text = author.Rows[0]["state"].ToString();
            zip.Text = author.Rows[0]["zip"].ToString();
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            if (name.Text == "" || surname.Text == "")
            {
                message.Content = "* заполнены не все обязательные поля";
                return;
            }
            if (zip.Text != "" && zip.Text.Length < 5)
            {
                message.Content = "Длина индекса меньше, чем нужно";
                return;
            }
            string[] names = authors.SelectedItem.ToString().Split(' ');

            String cur_phone = "UNKNOWN";
            String cur_address = null;
            String cur_city = null;
            String cur_state = null;
            String cur_zip = null;

            if (phone.Text != "")
                cur_phone = phone.Text;
            if (address.Text != "")
                cur_address = address.Text;
            if (city.Text != "")
                cur_city = city.Text;
            if (state.Text != "")
                cur_state = state.Text;
            if (zip.Text != "")
                cur_zip = zip.Text;

            Authors author = new Authors(name.Text, surname.Text, cur_phone, cur_address, cur_city, cur_state, cur_zip);
            dbClass.updateAuthorById(author, names[2]);

            authors.Visibility = Visibility.Visible;
            authorsLabel.Visibility = Visibility.Visible;
            addButton.Visibility = Visibility.Visible;
            authorsPanel.Visibility = Visibility.Collapsed;
            updateAuthorLabel.Visibility = Visibility.Collapsed;
            backButton.Visibility = Visibility.Collapsed;
            updateButton.Visibility = Visibility.Collapsed;

            name.Text = "";
            surname.Text = "";
            phone.Text = "";
            address.Text = "";
            city.Text = "";
            state.Text = "";
            zip.Text = "";
            message.Content = "";

            init_authors();
        }

        private void MenuItemView_Click(object sender, RoutedEventArgs e)
        {
            authors.Visibility = Visibility.Collapsed;
            authorsLabel.Visibility = Visibility.Collapsed;
            addButton.Visibility = Visibility.Collapsed;
            authorsPanel.Visibility = Visibility.Visible;
            viewAuthorLabel.Visibility = Visibility.Visible;
            backButton.Visibility = Visibility.Visible;

            titles.Columns.Clear();
            sales.Columns.Clear();
            description.Content = "";

            string[] names = authors.SelectedItem.ToString().Split(' ');

            DataTable allAuthors = dbClass.getDataTableAuthorsById(names[2]);

            for (int i = 0; i < allAuthors.Rows.Count; i++)
            {
                name.Text = allAuthors.Rows[i]["au_lname"].ToString();
                surname.Text = allAuthors.Rows[i]["au_fname"].ToString();
                viewAuthorLabel.Content = allAuthors.Rows[i]["au_lname"] + "\n" + allAuthors.Rows[i]["au_fname"];
                phone.Text = allAuthors.Rows[i]["phone"].ToString();
                address.Text = allAuthors.Rows[i]["address"].ToString();
                city.Text = allAuthors.Rows[i]["city"].ToString();
                state.Text = allAuthors.Rows[i]["state"].ToString();
                zip.Text = allAuthors.Rows[i]["zip"].ToString();
            }

            name.IsReadOnly = true;
            surname.IsReadOnly = true;
            phone.IsReadOnly = true;
            address.IsReadOnly = true;
            city.IsReadOnly = true;
            state.IsReadOnly = true;
            zip.IsReadOnly = true;
        }
    }
}