using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
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

namespace EmployeesMailsClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Employee> employees = new ObservableCollection<Employee>();
        private ObservableCollection<Mail1> mails = new ObservableCollection<Mail1>();
        public MainWindow()
        {
            InitializeComponent();

            employees = GetEmployeeList();
            mails = GetMailsList();

            EmployeeCombo.ItemsSource = employees;
            AddMailSenderCombo.ItemsSource = employees;
            AddMailReceiverCombo.ItemsSource = employees;
            MailsCombo.ItemsSource = mails;
        }

        private ObservableCollection<Mail1> GetMailsList()
        {
            string url = "https://localhost:44345/api/Mails";
            var json = new WebClient().DownloadString(url);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Mail1>>(json);
        }

        private ObservableCollection<Employee> GetEmployeeList()
        {
            string url = "https://localhost:44345/api/Employees";
            var json = new WebClient().DownloadString(url);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Employee>>(json);
        }

        private List<Mail1> GetSentMails(int employeeId)
        {
            string url = String.Format("https://localhost:44345/api/Employees/{0}", employeeId);
            var json = new WebClient().DownloadString(url);
            Employee3 e = Newtonsoft.Json.JsonConvert.DeserializeObject<Employee3>(json);
            return e.mailsSent;
        }

        private List<Mail1> GetGotMails(int employeeId)
        {
            string url = String.Format("https://localhost:44345/api/Employees/{0}", employeeId);
            var json = new WebClient().DownloadString(url);
            Employee3 e = Newtonsoft.Json.JsonConvert.DeserializeObject<Employee3>(json);
            return e.mailsGot;
        }

        private void FillSentDataGrid(int employeeId)
        {
            List<Mail1> employeeSentMails = GetSentMails(employeeId);
            SentDataGrid.ItemsSource = employeeSentMails;
        }

        private void FillGotDataGrid(int employeeId)
        {
            List<Mail1> employeeGotMails = GetGotMails(employeeId);
            GotDataGrid.ItemsSource = employeeGotMails;
        }

        private void EmployeeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EmployeeDepartmentBox.Text = "";

            ComboBox comboBox = (ComboBox)EmployeeCombo;
            Employee selectedEmployee = (Employee)comboBox.SelectedItem;
            if (selectedEmployee != null)
            {
                EmployeeDepartmentBox.Text = selectedEmployee.department;
                FillSentDataGrid(selectedEmployee.id);
                FillGotDataGrid(selectedEmployee.id);
            } 
        }

        private void MailsCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            Mail1 selectedMail = (Mail1)comboBox.SelectedItem;
            
            if (selectedMail != null)
            {
                string senderFullName = selectedMail.from_employee.name + " " + selectedMail.from_employee.surname;
                string receiverFullName = selectedMail.to_employee.name + " " + selectedMail.to_employee.surname;

                DeleteMailSenderBox.Text = senderFullName;
                DeleteMailReceiverBox.Text = receiverFullName;
                DeleteMailNameBox.Text = selectedMail.name;
                DeleteMailContentBox.Text = selectedMail.content;
                DeleteMailDateBox.Text = selectedMail.date;
            }
        }

        private void DeleteMailButton_Click(object sender, RoutedEventArgs e)
        {
            Mail1 selectedMail = (Mail1)MailsCombo.SelectedItem;
            if (selectedMail != null)
            {
                string url = String.Format("https://localhost:44345/api/Mails/{0}", selectedMail.id);

                WebRequest request = WebRequest.Create(url);
                request.Method = "DELETE";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    MailsCombo.SelectedItem = null;

                    DeleteMailSenderBox.Text = "";
                    DeleteMailReceiverBox.Text = "";
                    DeleteMailNameBox.Text = "";
                    DeleteMailContentBox.Text = "";
                    DeleteMailDateBox.Text = "";

                    mails.Remove(selectedMail);

                    Employee employee = (Employee)EmployeeCombo.SelectedItem;
                    if (employee != null)
                    {
                        if (employee.id == selectedMail.from_employee.id || employee.id == selectedMail.to_employee.id)
                        {
                            EmployeeCombo_SelectionChanged(EmployeeCombo, null);
                        }
                    }

                    MessageBox.Show("Письмо удалено", "Удаление письма");
                } else
                {
                    MessageBox.Show("При удалении письма произошла ошибка", "Удаление письма");
                }
            }
        }

        private void AddMailSenderCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AddMailReceiverCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AddMailButton_Click(object sender, RoutedEventArgs e)
        {

            Employee senderEmployee = (Employee)AddMailSenderCombo.SelectedItem;
            Employee receiverEmployee = (Employee)AddMailReceiverCombo.SelectedItem;

            string mailName = AddMailNameBox.Text;
            string mailContent = AddMailContentBox.Text;

            if (senderEmployee == null)
            {
                MessageBox.Show("Отправитель не выбран", "Ошибка валидации");
                return;
            }
            if (receiverEmployee == null)
            {
                MessageBox.Show("Получатель не выбран", "Ошибка валидации");
                return;
            }

            if (mailName == string.Empty)
            {
                MessageBox.Show("Заголовок письма отсутствует", "Ошибка валидации");
                return;
            }
            if (mailContent == string.Empty)
            {
                MessageBox.Show("Содержание письма отсутствует", "Ошибка валидации");
                return;
            }

            AddingMail addingMail = new AddingMail { Name = mailName, Content = mailContent, From_employeeId = senderEmployee.id, To_employeeId = receiverEmployee.id, Date = DateTime.Now };
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(addingMail);
            WebClient webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.ContentType] = "application/json";

            var res = webClient.UploadString("https://localhost:44345/api/Mails", json);
            Mail1 newMail = Newtonsoft.Json.JsonConvert.DeserializeObject<Mail1>(res);
            mails.Add(newMail);


            Employee selectedEmployee = (Employee)EmployeeCombo.SelectedItem;
            if (selectedEmployee != null)
            {
                if (selectedEmployee.id == newMail.from_employee.id || selectedEmployee.id == newMail.to_employee.id)
                {
                    EmployeeCombo_SelectionChanged(EmployeeCombo, null);
                }
            }

            AddMailReceiverCombo.SelectedItem = null;
            AddMailSenderCombo.SelectedItem = null;
            AddMailNameBox.Text = "";
            AddMailContentBox.Text = "";

            MessageBox.Show("Письмо добавлено", "Добавление письма");
        }
    }
}