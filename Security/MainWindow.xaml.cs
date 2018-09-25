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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;

namespace Security
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		SqlConnection sqlConnection;
		int enter_try = 0;

		public MainWindow()
		{
			InitializeComponent();
		}

		bool Check_State(string state)
		{
			if (state == "active")
				return true;
			else
				return false;
			
		}

		void Admin_Enter()
		{
			Admin_window Admin_Window = new Admin_window();
			Admin_Window.Owner = this;

			this.Hide();
			Admin_Window.login = Login_Box.Text;
			Admin_Window.password = Password_Box.Password;
			if (Admin_Window.ShowDialog() == true)
			{
				this.Show();
			}
			else
				this.Show();
		}

		void Check_Status()
		{
			string path = System.IO.Path.GetFullPath("Userdb.mdf");

			string connectionS = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + path + ";Integrated Security=True";
			sqlConnection = new SqlConnection(connectionS);

			sqlConnection.Open();

			SqlDataReader sqlReader = null;


			string command = "SELECT * FROM [Users] WHERE Login = '" + Login_Box.Text + "' AND Password = '" + Password_Box.Password + "';";

			SqlCommand commandsql = new SqlCommand(command, sqlConnection);

			try
			{
				sqlReader = commandsql.ExecuteReader();
				if (sqlReader.Read())
					if (Convert.ToString(sqlReader["Status"]) == "admin")
					{
						bool t_f = Check_State(Convert.ToString(sqlReader["State"]));
						if (t_f)
							Admin_Enter();
						else
							MessageBox.Show("Доступ заблокирован");
					}
					else
					{
						bool t_f = Check_State(Convert.ToString(sqlReader["State"]));
						if (t_f)
							User_Enter();
						else
							MessageBox.Show("Доступ заблокирован");
					}
				else
				{
					MessageBox.Show("Неверная пара логин/пароль!");
					enter_try++;
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

			Password_Box.Password = "";
			Login_Box.Text = "";
			if (enter_try == 3)
				Close();
		}

		void User_Enter()
		{
			string path = System.IO.Path.GetFullPath("Userdb.mdf");

			string connectionS = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + path + ";Integrated Security=True";
			sqlConnection = new SqlConnection(connectionS);

			sqlConnection.Open();

			SqlDataReader sqlReader = null;

			string command = "SELECT * FROM [Users] WHERE [Login]='" + Login_Box.Text + "'";

			SqlCommand commandsql = new SqlCommand(command, sqlConnection);
			try
			{
				sqlReader = commandsql.ExecuteReader();

				if (sqlReader.Read())
				{
					MessageBox.Show(Convert.ToString(sqlReader["Login"]) + " " + Convert.ToString(sqlReader["Password"]) + " " + Convert.ToString(sqlReader["Status"]) + " " + Convert.ToString(sqlReader["State"]) + " " + Convert.ToString(sqlReader["Date"]));
					if (Convert.ToString(sqlReader["Date"]) == "new_user")
					{
						Password_Change_Window password_change = new Password_Change_Window();
						password_change.Owner = this;
						this.Hide();

						password_change.name = Login_Box.Text;
						if (password_change.ShowDialog() == true)
						{
							User_Window user_Window = new User_Window();
							user_Window.Owner = this;

							this.Hide();
							user_Window.login = Login_Box.Text;
							user_Window.password = Password_Box.Password;
							if (user_Window.ShowDialog() == true)
							{
								this.Show();
							}
							else
								this.Show();
						}
					}
					else
					{
						User_Window user_Window = new User_Window();
						user_Window.Owner = this;

						this.Hide();
						user_Window.login = Login_Box.Text;
						user_Window.password = Password_Box.Password;
						if (user_Window.ShowDialog() == true)
						{
							this.Show();
						}
						else
							this.Show();
					}
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

		private void Enter_Click(object sender, RoutedEventArgs e)
		{
			Regex regex = new Regex(@"[0-9.,;:]");
			if (regex.IsMatch(Password_Box.Password))
				MessageBox.Show("Нельзя вводит данные символы [0-9], [,.:;]");
			else
				Check_Status();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			string path = System.IO.Path.GetFullPath("Userdb.mdf");

			string connectionS = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + path + ";Integrated Security=True";
			sqlConnection = new SqlConnection(connectionS);

			sqlConnection.Open();

			SqlDataReader sqlReader = null;

			try
			{
				SqlCommand commandsql1 = new SqlCommand("UPDATE [Users] SET [State]=@State WHERE Status='admin'", sqlConnection);
				commandsql1.Parameters.AddWithValue("State", "active");
				commandsql1.ExecuteNonQuery();				
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

			Password_Box.Password = "";
			Login_Box.Text = "";
			
		}
	}
}
