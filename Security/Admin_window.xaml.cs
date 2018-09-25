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
using System.Text.RegularExpressions;
namespace Security
{
    /// <summary>
    /// Логика взаимодействия для Admin_window.xaml
    /// </summary>
    public partial class Admin_window : Window
    {
		public string login, password;
		SqlConnection sqlConnection;

		public Admin_window()
        {
            InitializeComponent();
        }

		private void Base_Open(object sender, RoutedEventArgs e)
		{
			DB_Window Base_Window = new DB_Window();
			Base_Window.Owner = this;
			
			if (Base_Window.ShowDialog() == true)
			{
			}
		}

		private void Password_Change(object sender, RoutedEventArgs e)
		{
			if (New_Password_Box.Password != "" && New_Password_Box_Accept.Password != "")
			{
				Regex regex = new Regex(@"[0-9.,;:]");
				if (regex.IsMatch(New_Password_Box.Password))
				{
					MessageBox.Show("Нельзя вводит данные символы [0-9], [,.:;]");
				}
				else
				{
					if (Old_Password_Box.Password == password)
					{
						string path = System.IO.Path.GetFullPath("Userdb.mdf");

						string connectionS = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + path + ";Integrated Security=True";
						sqlConnection = new SqlConnection(connectionS);

						sqlConnection.Open();

						SqlDataReader sqlREader = null;

						SqlCommand commandsql = new SqlCommand("UPDATE [Users] SET [Password]=@Password WHERE [Login]='" + login + "'", sqlConnection);

						try
						{
							commandsql.Parameters.AddWithValue("Password", New_Password_Box.Password);
							commandsql.ExecuteNonQuery();


							MessageBox.Show("Пароль изменен!");
							Old_Password_Box.Password = "";
							New_Password_Box.Password = "";
							New_Password_Box_Accept.Password = "";
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
					else
						MessageBox.Show("Проверьте правильность данных!");
				}
			}
			else
				MessageBox.Show("Введите данные!");
		}

	}
}
