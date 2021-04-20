using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesMailsClient
{
    public class Employee
    {
        public int id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string department { get; set; }
        public string fullName 
        { 
            get { return name + " " + surname; } 
        }
    }
}
