using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeesMails_.Models
{
    public class Mail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("From_employeeId")]
        public Employee From_employee { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("To_employeeId")]
        public Employee To_employee { get; set; }
        public DateTime Date { get; set; }
        public Mail()
        {

        }
    }
}
