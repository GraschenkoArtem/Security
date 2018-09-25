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
	/// Логика взаимодействия для Password_Change_Window.xaml
	/// </summary>
	public partial class Password_Change_Window : Window
	{
		public string name;
		SqlConnection sqlConnection;
		public Password_Change_Window()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Введите новый пароль и подтвердите его!");
		}

		private void Accept_Click(object sender, RoutedEventArgs e)
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
					string path = System.IO.Path.GetFullPath("Userdb.mdf");

					string connectionS = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + path + ";Integrated Security=True";

					sqlConnection = new SqlConnection(connectionS);

					sqlConnection.Open();

					SqlCommand commandsql = new SqlCommand("UPDATE [Users] SET [Password]=@Password, [Date]=@Date WHERE [Login]='" + name + "'", sqlConnection);

					try
					{
						string a = "old_user";
						commandsql.Parameters.AddWithValue("Password", New_Password_Box.Password);
						commandsql.Parameters.AddWithValue("Date", a);
						commandsql.ExecuteNonQuery();
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message.ToString());
						this.DialogResult = true;
					}
					finally
					{
						sqlConnection.Close();
						this.Owner.Show();
						this.Close();
					}
				}
			}
		}
	}
}
