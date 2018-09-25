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
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Security
{
	/// <summary>
	/// Логика взаимодействия для Add_User.xaml
	/// </summary>
	public partial class Add_User : Window
	{
		SqlConnection sqlConnection;
		public Add_User()
		{
			InitializeComponent();
		}

		private void Accept_Click(object sender, RoutedEventArgs e)
		{
			if (New_Login_Box.Text != "")
			{
				Regex regex = new Regex(@"[0-9.,;:]");
				if (regex.IsMatch(New_Password_Box.Password)) {
					MessageBox.Show("Нельзя вводит данные символы [0-9], [,.:;]");
				}
				else
				{
					try
					{
						string path = System.IO.Path.GetFullPath("Userdb.mdf");

						string connectionS = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + path + ";Integrated Security=True";
						sqlConnection = new SqlConnection(connectionS);

						sqlConnection.Open();

						SqlCommand commandsql = new SqlCommand("INSERT INTO [Users] (Login, Password, Status, State) VALUES (@Login, @Password, @Status, @State)", sqlConnection);

						commandsql.Parameters.AddWithValue("Login", New_Login_Box.Text);
						commandsql.Parameters.AddWithValue("Password", New_Password_Box.Password);
						commandsql.Parameters.AddWithValue("Status", "user");
						commandsql.Parameters.AddWithValue("State", "active");

						commandsql.ExecuteNonQuery();

						DialogResult = true;
					}
					catch (Exception ex)
					{
						MessageBox.Show(Convert.ToString(ex));
					}
				}
			}
			else
				MessageBox.Show("Введите данные!");
		}
	}
}
