using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeesMails_
{
    public class PostMail
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public int From_employeeId { get; set; }
        public int To_employeeId { get; set; }
        public DateTime Date { get; set; }
    }
}
