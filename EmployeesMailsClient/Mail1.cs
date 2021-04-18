using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesMailsClient
{
    public class Mail1
    {
        public int id { get; set; }
        public string name { get; set; }
        public string content { get; set; }
        public Employee1 from_employee { get; set; }
        public Employee1 to_employee { get; set; }
        public string date { get; set; }
    }

}
