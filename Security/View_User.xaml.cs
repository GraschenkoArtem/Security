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
	/// Логика взаимодействия для View_User.xaml
	/// </summary>
	public partial class View_User : Window
	{
		public string login;
		SqlConnection sqlConnection;

		public View_User()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			string path = System.IO.Path.GetFullPath("Userdb.mdf");

			string connectionS = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + path + ";Integrated Security=True";
			sqlConnection = new SqlConnection(connectionS);

			sqlConnection.Open();

			SqlDataReader sqlReader = null;


			string command = "SELECT * FROM [Users] WHERE [Login]='" + login + "';";

			SqlCommand commandsql = new SqlCommand(command, sqlConnection);

			try
			{
				sqlReader = commandsql.ExecuteReader();

				while (sqlReader.Read())
				{
					Login_Box.Text = Convert.ToString(sqlReader["Login"]);
					Password_Box.Text = Convert.ToString(sqlReader["Password"]);
					Status_Box.Text = Convert.ToString(sqlReader["Status"]);
					State_Box.Text = Convert.ToString(sqlReader["State"]);
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
	}
}
