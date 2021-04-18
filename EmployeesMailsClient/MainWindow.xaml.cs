using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        private List<Employee> employees = new List<Employee>();
        public MainWindow()
        {
            InitializeComponent();

            employees = GetEmployeeList();
            FillEmployeeCombo();
        }

        private List<Employee> GetEmployeeList()
        {
            var json = new WebClient().DownloadString("https://localhost:44345/api/Employees/");
            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<Employee>>(json);
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
            SentDataGrid.Items.Clear();

            List<Mail1> employeeSentMails = GetSentMails(employeeId);

            foreach (Mail1 mail1 in employeeSentMails)
            {
                string senderFullName = mail1.from_employee.name + " " + mail1.from_employee.surname;
                string receiverFullName = mail1.to_employee.name + " " + mail1.to_employee.surname;
                SentDataGrid.Items.Add(new { name = mail1.name, content=mail1.content, sender=senderFullName, receiver=receiverFullName,date = mail1.date}); ;
            }
        }

        private void FillGotDataGrid(int employeeId)
        {
            GotDataGrid.Items.Clear();

            List<Mail1> employeeGotMails = GetGotMails(employeeId);

            foreach (Mail1 mail1 in employeeGotMails)
            {
                string senderFullName = mail1.from_employee.name + " " + mail1.from_employee.surname;
                string receiverFullName = mail1.to_employee.name + " " + mail1.to_employee.surname;
                GotDataGrid.Items.Add(new { name = mail1.name, content = mail1.content, sender = senderFullName, receiver = receiverFullName, date = mail1.date }); ;
            }
        }

        private void FillEmployeeCombo()
        {
            ComboBoxItem defaulEmployeeItem = new ComboBoxItem();
            defaulEmployeeItem.Name = "item_" + "0";
            defaulEmployeeItem.Content = "(не выбран)";
            EmployeeCombo.Items.Add(defaulEmployeeItem);
            EmployeeCombo.SelectedItem = defaulEmployeeItem;

            foreach(Employee employee in employees)
            {
                string EmployeeFullName = employee.name + " " + employee.surname;
                ComboBoxItem newComboBoxItem = new ComboBoxItem();
                newComboBoxItem.Name = "item_" + employee.id.ToString();
                newComboBoxItem.Content = EmployeeFullName;
                EmployeeCombo.Items.Add(newComboBoxItem);
            }
        }

        private void EmployeeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EmployeeDepartmentBox.Text = "";

            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
            int selectedEmployeeId = int.Parse(selectedItem.Name.Substring(5));

            if (selectedEmployeeId > 0)
            {
                foreach (Employee employee in employees)
                {
                    if (employee.id == selectedEmployeeId)
                    {
                        EmployeeDepartmentBox.Text = employee.department;
                        break;
                    }
                }

                FillSentDataGrid(selectedEmployeeId);
                FillGotDataGrid(selectedEmployeeId);
            }
        }
    }
}