using System;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Data.SqlClient;

namespace Lab03
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window

    {

        SqlConnection connection = new SqlConnection("Data Source=LAPTOP-URF2LF9O\\SQLEXPRESS02;Initial Catalog=Customers;" +
            "Integrated Security=True; TrustServerCertificate=True;");



        public MainWindow()
        {
            InitializeComponent();
        }

        private void datatable_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();
                DataTable dataTable = new DataTable();
                SqlCommand command = new SqlCommand("SELECT * FROM Customer", connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dataTable);
                connection.Close();
                dataGrid.ItemsSource = dataTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

        }

      

        private void datareader_Click(object sender, RoutedEventArgs e)
        {
            List<Customer> Customers = new List<Customer>();

            connection.Open();
            SqlCommand command = new SqlCommand("Select * from Customer", connection);
            SqlDataReader dataReader = command.ExecuteReader();


            while (dataReader.Read())
            {
                Customers.Add(new Customer
                {
                    CustomerID = Convert.ToInt32(dataReader[0]),
                    FirstName = dataReader[1].ToString(),
                    LastName = dataReader[2].ToString(),
                    DNI = dataReader[3].ToString(),
                });

            }

            connection.Close();
            dataGrid.ItemsSource = Customers;
        }

        private void search_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Customer> customers = new List<Customer>();
                string name = txtSearch.Text.Trim();

                connection.Open();

                SqlCommand command = new SqlCommand("sp_SearchCustomerByName", connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@FirstName";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Size = 100;
                parameter.Value = name;

                command.Parameters.Add(parameter);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    customers.Add(new Customer
                    {
                        CustomerID = Convert.ToInt32(reader[0]),
                        FirstName = reader[1].ToString(),
                        LastName = reader[2].ToString(),
                        DNI = reader[3].ToString()
                    });
                }

                connection.Close();
                dataGrid.ItemsSource = customers;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                connection.Close();
            }
        }
    }
}