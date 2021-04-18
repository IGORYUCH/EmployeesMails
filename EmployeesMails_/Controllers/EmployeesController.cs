using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeesMails_.Data;
using EmployeesMails_.Models;
using Microsoft.Extensions.Logging;


namespace EmployeesMails_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetEmployee()
        {
            var employees = await _context.Employee.ToListAsync();
            var extendedEmployees = new List<object>();
            foreach (Employee employee in employees)
            {
                int id = employee.Id;
                int MailsSent = await _context.Mail.Where(f => f.From_employee.Id == employee.Id).CountAsync();
                int MailsGot = await _context.Mail.Where(f => f.To_employee.Id == employee.Id).CountAsync();
                var extendedEmployee = new { Id = id, Name = employee.Name, Surname = employee.Surname, Department = employee.Department, MailsSent = MailsSent, MailsGot = MailsGot };
                extendedEmployees.Add(extendedEmployee);
            }
            return extendedEmployees;
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetEmployee(int id)
        {

            var employee = await _context.Employee.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            var mailsSent = await _context.Mail.Where(f => f.From_employee.Id == id).Include(a=> a.To_employee).ToListAsync();
            var mailsGot = await _context.Mail.Where(f => f.To_employee.Id == id).Include(a=> a.From_employee).ToListAsync();
            var extendedEmployee = new {Id=employee.Id, Name = employee.Name, Surname = employee.Surname, Department=employee.Department, MailsGot= mailsGot, MailsSent= mailsSent };

            return extendedEmployee;
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.Id == id);
        }
    }
}
