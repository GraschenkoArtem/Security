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
	/// Логика взаимодействия для Password_Window.xaml
	/// </summary>
	public partial class Password_Window : Window
	{
		SqlConnection sqlConnection;
		public string name;

		public Password_Window()
		{
			InitializeComponent();
		}

		private void Accept_Click(object sender, RoutedEventArgs e)
		{

			if (New_Password_Box.Text != "")
			{
				Regex regex = new Regex(@"[0-9.,;:]");
				if (regex.IsMatch(New_Password_Box.Text))
				{
					MessageBox.Show("Нельзя вводит данные символы [0-9], [,.:;]");
				}
				else
				{
					string path = System.IO.Path.GetFullPath("Userdb.mdf");

					string connectionS = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + path + ";Integrated Security=True";

					sqlConnection = new SqlConnection(connectionS);

					sqlConnection.Open();

					SqlCommand commandsql = new SqlCommand("UPDATE [Users] SET [Password]=@Password WHERE [Login]='" + name + "'", sqlConnection);

					try
					{
						commandsql.Parameters.AddWithValue("Password", New_Password_Box.Text);
						commandsql.ExecuteNonQuery();
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message.ToString());
					}
					finally
					{
						this.DialogResult = true;
					}
				}
			}
		}
	}
}
