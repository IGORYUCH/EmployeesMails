using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesMailsClient
{
    public class Employee3
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string department { get; set; }

        public List<Mail1> mailsSent { get; set; }
        public List<Mail1> mailsGot { get; set; }
    }
}
