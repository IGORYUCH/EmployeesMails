using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesMailsClient
{
    public class Mail
    {
        public int id { get; set; }
        public string name { get; set; }
        public string content { get; set; }
        public MailEmployee from_employee { get; set; }
        public MailEmployee to_employee { get; set; }
        public string date { get; set; }
    }
}
