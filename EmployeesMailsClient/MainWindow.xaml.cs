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


namespace EmployeesMailsClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Employee> employees = new ObservableCollection<Employee>();
        private ObservableCollection<Mail> mails = new ObservableCollection<Mail>();

        private string host = "localhost";
        private string port = "44345";
        private string mailsUrl = "https://{0}:{1}/api/Mails/";
        private string mailUrl = "https://{0}:{1}/api/Mails/{2}";
        private string employeesUrl = "https://{0}:{1}/api/Employees/";
        private string employeeUrl = "https://{0}:{1}/api/Employees/{2}";

        public MainWindow()
        {
            InitializeComponent();

            employees = GetEmployeeList();
            mails = GetMailsList();

            EmployeeCombo.ItemsSource = employees;
            AddMailSenderCombo.ItemsSource = employees;
            AddMailReceiverCombo.ItemsSource = employees;
            DeleteMailCombo.ItemsSource = mails;



            var uri = new Uri("dark.xaml", UriKind.Relative);
            // загружаем словарь ресурсов
            ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
            // очищаем коллекцию ресурсов приложения
            Application.Current.Resources.Clear();
            // добавляем загруженный словарь ресурсов
            Application.Current.Resources.MergedDictionaries.Add(resourceDict);
        }

        private ObservableCollection<Mail> GetMailsList()
        {
            string url = string.Format(mailsUrl, host, port);
            var json = new WebClient().DownloadString(url);
            return JsonConvert.DeserializeObject<ObservableCollection<Mail>>(json);
        }

        private ObservableCollection<Employee> GetEmployeeList()
        {
            string url = string.Format(employeesUrl, host, port);
            var json = new WebClient().DownloadString(url);
            return JsonConvert.DeserializeObject<ObservableCollection<Employee>>(json);
        }

        private List<Mail> GetSentMails(int employeeId)
        {
            string url = string.Format(employeeUrl, host, port, employeeId);
            var json = new WebClient().DownloadString(url);
            EmployeeWithMails employee = JsonConvert.DeserializeObject<EmployeeWithMails>(json);
            return employee.mailsSent;
        }

        private List<Mail> GetGotMails(int employeeId)
        {
            string url = string.Format(employeeUrl, host, port, employeeId);
            var json = new WebClient().DownloadString(url);
            EmployeeWithMails employee = JsonConvert.DeserializeObject<EmployeeWithMails>(json);
            return employee.mailsGot;
        }

        private void FillSentDataGrid(int employeeId)
        {
            List<Mail> employeeSentMails = GetSentMails(employeeId);
            SentDataGrid.ItemsSource = employeeSentMails;
        }

        private void FillGotDataGrid(int employeeId)
        {
            List<Mail> employeeGotMails = GetGotMails(employeeId);
            GotDataGrid.ItemsSource = employeeGotMails;
        }

        private void EmployeeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EmployeeDepartmentBox.Text = "";

            Employee selectedEmployee = (Employee)EmployeeCombo.SelectedItem;
            if (selectedEmployee != null)
            {
                EmployeeDepartmentBox.Text = selectedEmployee.department;
                FillSentDataGrid(selectedEmployee.id);
                FillGotDataGrid(selectedEmployee.id);
            } 
        }

        private void MailsCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Mail selectedMail = (Mail)DeleteMailCombo.SelectedItem;
            
            if (selectedMail != null)
            {
                DeleteMailSenderBox.Text = selectedMail.from_employee.fullName;
                DeleteMailReceiverBox.Text = selectedMail.to_employee.fullName;
                DeleteMailNameBox.Text = selectedMail.name;
                DeleteMailContentBox.Text = selectedMail.content;
                DeleteMailDateBox.Text = selectedMail.date;
            }
        }

        private void DeleteMailButton_Click(object sender, RoutedEventArgs e)
        {
            Mail selectedMail = (Mail)DeleteMailCombo.SelectedItem;
            if (selectedMail != null)
            {
                string url = string.Format(mailUrl, host, port, selectedMail.id);

                WebRequest request = WebRequest.Create(url);
                request.Method = "DELETE";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    DeleteMailCombo.SelectedItem = null;

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

            object mailToAdd = new {
                Name = mailName,
                Content = mailContent,
                From_employeeId = senderEmployee.id,
                To_employeeId = receiverEmployee.id,
                Date = DateTime.Now 
            };

            string json = JsonConvert.SerializeObject(mailToAdd);
            WebClient webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.ContentType] = "application/json";

            string url = string.Format(mailsUrl, host, port);

            var res = webClient.UploadString(url, json);
            Mail newMail = JsonConvert.DeserializeObject<Mail>(res);
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