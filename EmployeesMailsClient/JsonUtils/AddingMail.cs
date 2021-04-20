using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesMailsClient
{
    public class AddingMail
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public int From_employeeId { get; set; }
        public int To_employeeId { get; set; }
        public DateTime Date { get; set; }
        public AddingMail()
        {

        }
    }
}
