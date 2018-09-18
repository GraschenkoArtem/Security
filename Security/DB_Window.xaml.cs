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
using System.Data.SqlClient;
using System.Data;

namespace Security
{
    /// <summary>
    /// Логика взаимодействия для DB_Window.xaml
    /// </summary>
    public partial class DB_Window : Window
    {
		SqlConnection sqlConnection;

		public DB_Window()
        {
            InitializeComponent();
        }

		private void Lock()
		{
			string path = System.IO.Path.GetFullPath("Userdb.mdf");

			string connectionS = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + path + ";Integrated Security=True";
			sqlConnection = new SqlConnection(connectionS);

			sqlConnection.Open();

			SqlDataReader sqlREader = null;
				
			SqlCommand commandsql = new SqlCommand("UPDATE [Users] SET [State]=@State WHERE [Login]='" + User_List.SelectedValue + "' AND [Status] = 'user'", sqlConnection);

			try
			{
				commandsql.Parameters.AddWithValue("State", "inactive");
				commandsql.ExecuteNonQuery();
				MessageBox.Show("Пользователь заблокирован");
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message.ToString());
			}
			finally
			{
				if (sqlREader != null)
					sqlREader.Close();
			}
		}

		private void Unlock()
		{
			string path = System.IO.Path.GetFullPath("Userdb.mdf");

			string connectionS = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + path + ";Integrated Security=True";
			sqlConnection = new SqlConnection(connectionS);

			sqlConnection.Open();

			SqlDataReader sqlREader = null;

			SqlCommand commandsql = new SqlCommand("UPDATE [Users] SET [State]=@State WHERE [Login]='" + User_List.SelectedValue + "'", sqlConnection);

			try
			{
				commandsql.Parameters.AddWithValue("State", "active");
				commandsql.ExecuteNonQuery();
				MessageBox.Show("Пользователь разблокирован");
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message.ToString());
			}
			finally
			{
				if (sqlREader != null)
					sqlREader.Close();
				
			}
		}
		
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			Win_Load();
		}

		private void Win_Load()
		{
			User_List.Items.Clear();

			string path = System.IO.Path.GetFullPath("Userdb.mdf");

			string connectionS = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + path + ";Integrated Security=True";
			sqlConnection = new SqlConnection(connectionS);

			sqlConnection.Open();

			SqlDataReader sqlReader = null;


			string command = "SELECT * FROM [Users];";

			SqlCommand commandsql = new SqlCommand(command, sqlConnection);

			try
			{
				sqlReader = commandsql.ExecuteReader();

				while (sqlReader.Read())
				{
					User_List.Items.Add(Convert.ToString(sqlReader["Login"]));
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message.ToString());
			}
			finally
			{
				if (sqlReader != null)
					sqlReader.Close();
			}
		}

		private void Password_Change(object sender, RoutedEventArgs e)
		{
			Password_Window Password_Change_Window = new Password_Window();
			Password_Change_Window.Owner = this;


			if(User_List.SelectedIndex != -1)
			{
				Password_Change_Window.name = Convert.ToString(User_List.SelectedValue);

				if (Password_Change_Window.ShowDialog() == true)
				{
					MessageBox.Show("Пароль изменён");
					Win_Load();
				}
				else
				{
					MessageBox.Show("Пароль не изменён");
					Win_Load();
				}
				}
			else
			{
				MessageBox.Show("Выберите Пользователя!");
			}

		}

		private void Lock_User(object sender, RoutedEventArgs e)
		{
			if (User_List.SelectedIndex != -1)
				Lock();
			else
				MessageBox.Show("Выберите Пользователя!");
		}

		private void Unlock_User(object sender, RoutedEventArgs e)
		{
			if (User_List.SelectedIndex != -1)
				Unlock();
			else
				MessageBox.Show("Выберите Пользователя!");
		}

		private void Add_User(object sender, RoutedEventArgs e)
		{
			Add_User Add_User_Window = new Add_User();
			Add_User_Window.Owner = this;

			if (Add_User_Window.ShowDialog() == true) {
				Win_Load();
			}
			else
				Win_Load();
		}

		private void Open_User(object sender, RoutedEventArgs e)
		{
			View_User View_User_Window = new View_User();
			View_User_Window.Owner = this;
			if (User_List.SelectedIndex != -1)
			{
				View_User_Window.login = Convert.ToString(User_List.SelectedValue);
				if (View_User_Window.ShowDialog() == true) {
					Win_Load();
				}
				else
					Win_Load();
			}
			else
			{
				MessageBox.Show("Выберите Пользователя!");
			}
		}
	}
}
