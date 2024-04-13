using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Net.Http;
using System.Threading.Tasks;
using WPF06._04._24.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using Newtonsoft.Json;

namespace WPF06._04._24
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<Users> usersCollection;
        private ObservableCollection<Tickets> ticketsCollection;
        private ObservableCollection<TicketType> tickettypeCollection;
        private ObservableCollection<Equipments> equipmentsCollection; 
        private ObservableCollection<EquipmentType> equipmenttypeCollection;
        private ObservableCollection<Rental> rentalCollection;
        private ObservableCollection<Booking> bookingCollection;
        private ObservableCollection<Schedule> scheduleCollection;
        private ObservableCollection<Pass> passCollection;
        private ObservableCollection<Qualification> qualificationCollection;
        private ObservableCollection<Coaches> coachesCollection;
        private ObservableCollection<Training> trainingCollection;

        private readonly HttpClient client;
        private string minimalApiBaseUrl = "http://localhost:5045";
        private string connectionString = @"Data Source=SUPERCOMP; Initial Catalog=IceRink; Integrated Security=True; Trusted_Connection=True; TrustServerCertificate=True";


        public MainWindow()
        {
            InitializeComponent();
            client = new HttpClient();
            DataGrid.CellEditEnding += DataGrid_CellEditEnding;
            LoadDataFromApi("Users");
        }

        public async Task<T> InsertData<T>(string endpoint, T item)
        {
            string serializedItem = JsonConvert.SerializeObject(item);
            HttpResponseMessage response = await client.PostAsync($"{minimalApiBaseUrl}/{endpoint}", new StringContent(serializedItem, System.Text.Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(responseBody);
            }
            else
            {
                MessageBox.Show("Error occurred while inserting data");
                return default;
            }
        }

        public async Task<T> UpdateData<T>(string endpoint, T item)
        {
            string serializedItem = JsonConvert.SerializeObject(item);
            HttpResponseMessage response = await client.PutAsync($"{minimalApiBaseUrl}/{endpoint}", new StringContent(serializedItem, System.Text.Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(responseBody);
            }
            else
            {
                MessageBox.Show("Error occurred while updating data");
                return default;
            }
        }

        public async Task DeleteData(string endpoint)
        {
            HttpResponseMessage response = await client.DeleteAsync($"{minimalApiBaseUrl}/{endpoint}");

            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show("Error occurred while deleting data");
            }
        }

        private async Task LoadDataFromApi(string endpoint)
        {
            try
            {
                DataTable dt = await GetDataFromApi(endpoint);
                DataGrid.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading data from API: {ex.Message}");
            }
        }

        private async void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                TabControl tabControl = sender as TabControl;
                TabItem selectedTab = tabControl.SelectedItem as TabItem;
                string tabName = selectedTab.Header.ToString();

                switch (tabName)
                {
                    case "Users":
                        await LoadDataFromApi("Users");
                        break;
                    case "Tickets":
                        await LoadDataFromApi("Tickets");
                        break;
                    case "TicketType":
                        await LoadDataFromApi("TicketType");
                        break;
                    case "Equipments":
                        await LoadDataFromApi("Equipments");
                        break;
                    case "EquipmentType":
                        await LoadDataFromApi("EquipmentType");
                        break;
                    case "Rental":
                        await LoadDataFromApi("Rental");
                        break;
                    case "Booking":
                        await LoadDataFromApi("Booking");
                        break;
                    case "Schedule":
                        await LoadDataFromApi("Schedule");
                        break;
                    case "Pass":
                        await LoadDataFromApi("Pass");
                        break;
                    case "Qualification":
                        await LoadDataFromApi("Qualification");
                        break;
                    case "Coaches":
                        await LoadDataFromApi("Coaches");
                        break;
                    case "Training":
                        await LoadDataFromApi("Training");
                        break;


                    default:
                        MessageBox.Show("Choose the tab");
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading data: {ex.Message}");
            }
        }

        private async Task<DataTable> GetDataFromApi(string endpoint)
        {
            DataTable dt = new DataTable();
            HttpResponseMessage response = await client.GetAsync($"{minimalApiBaseUrl}/{endpoint}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsAsync<DataTable>();
                dt = data;
            }
            else
            {
                MessageBox.Show($"Error getting data from API. Status Code: {response.StatusCode}");
            }

            return dt;
        }

        private DataTable GetDataFromDatabase(string query)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    connection.Open();
                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка доступа к базе данных\r\n: {ex.Message}");
            }
            return dt;
        }

        private void InsertButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView newRowView = AddNewRow(DataGrid.Items);
            if (newRowView != null)
            {
                newRowView.BeginEdit();
                newRowView.EndEdit();
                // InsertDataToDatabase(newRowView.Row); // Этот вызов может быть удалён, так как метод InsertDataToDatabase не существует
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)DataGrid.SelectedItem;

            // Обновление данных в базе данных
            // UpdateDataInDatabase(selectedRow.Row); // Аналогично, этот вызов не существует и может быть удалён
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)DataGrid.SelectedItem;

            // Удаляем выбранную строку из DataGrid
            if (selectedRow != null)
            {
                DataView dataView = (DataView)DataGrid.ItemsSource;
                dataView.Table.Rows.Remove(selectedRow.Row);

                // Также удаляем строку из базы данных
                // DeleteDataFromDatabase(selectedRow.Row); // Этот вызов также не существует и может быть удалён
            }
        }

        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            if (grid != null && e.EditAction == DataGridEditAction.Commit)
            {
                var editedData = e.Row.Item as DataRowView;
                DataRow editedRow = editedData.Row;

                // Обновление измененных данных в SQL Server
                UpdateDataInDatabase(editedRow); // Добавьте этот код для обновления данных в базе данных в соответствии с изменениями в DataGrid
            }
        }

        private void UpdateDataInDatabase(DataRow row)
        {
            try
            {
                string tableName = "Users"; // Замените на имя таблицы, если редактируемые данные относятся к другой таблице
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand($"SELECT * FROM {tableName} WHERE Id = @id", connection);
                    command.Parameters.AddWithValue("@id", row["Id"]); // Предполагается, что у вас есть идентификатор строки

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    if (dt.Rows.Count == 1)
                    {
                        DataRow dbRow = dt.Rows[0];
                        dbRow.BeginEdit();
                        foreach (DataColumn column in dt.Columns)
                        {
                            dbRow[column.ColumnName] = row[column.ColumnName];
                        }
                        dbRow.EndEdit();

                        SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                        adapter.Update(dt); // Обновление данных в базе данных
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при обновлении данных в базе данных: {ex.Message}");
            }
        }

        private DataRowView AddNewRow(ItemCollection collection)
        {
            DataRowView newRow = null;
            var itemType = collection.GetType().GetGenericArguments()[0]; // Определение типа элемента в коллекции
            var addNewMethod = collection.GetType().GetMethod("AddNew");
            if (addNewMethod != null)
            {
                addNewMethod.Invoke(collection, null); // Вызов метода AddNew для добавления новой строки
                newRow = collection[collection.Count - 1] as DataRowView; // Получение последнего добавленного элемента
            }
            return newRow;
        }


        private async Task InsertDataToDatabase(string tableName, DataRow row)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand($"INSERT INTO {tableName} VALUES(/*добавьте значения из DataRow row*/)", connection);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Data inserted successfully into the database.");
                    }
                    else
                    {
                        MessageBox.Show("Error inserting data into the database.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inserting data into the database: {ex.Message}");
            }
        }


        // Метод для обновления данных в базе данных
        private async Task UpdateDataInDatabase(string tableName, DataRow row)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand($"UPDATE {tableName} SET /*установите значения для обновления*/ WHERE Id = @id", connection);
                    command.Parameters.AddWithValue("@id", row["Id"]);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Data updated successfully in the database.");
                    }
                    else
                    {
                        MessageBox.Show("Error updating data in the database.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating data in the database: {ex.Message}");
            }
        }


        // Метод для удаления данных из базы данных
        private async Task DeleteDataFromDatabase(string tableName, int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand($"DELETE FROM {tableName} WHERE Id = @id", connection);
                    command.Parameters.AddWithValue("@id", id);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Data deleted successfully from the database.");
                    }
                    else
                    {
                        MessageBox.Show("Error deleting data from the database.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting data from the database: {ex.Message}");
            }
        }


    }
}