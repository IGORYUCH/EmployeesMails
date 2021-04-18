using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeesMails_.Data;
using EmployeesMails_.Models;

namespace EmployeesMails_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Mails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Mail>>> GetMail()
        {
            return await _context.Mail.Include(f => f.From_employee).Include(a => a.To_employee).ToListAsync();
        }

        // GET: api/Mails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Mail>> GetMail(int id)
        {
            Mail mail = await _context.Mail.Include(f => f.To_employee).Include(a => a.From_employee).FirstOrDefaultAsync(x => x.Id == id);

            if (mail == null)
            {
                return NotFound(String.Format("The mail with id {0} not found", id));
            }

            return mail;
        }

        // POST: api/Mails
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754

        [HttpPost]
        public async Task<ActionResult<Mail>> PostMail(PostMail postMail)
        {
            Employee fromEmployee = await _context.Employee.FindAsync(postMail.From_employeeId);
            Employee toEmployee = await _context.Employee.FindAsync(postMail.To_employeeId);
            
            if (fromEmployee == null)
            {
                return NotFound(String.Format("Employee sender with id {0} not found", postMail.From_employeeId));
            }
            
            if (toEmployee == null)
            {
                return NotFound(String.Format("Employee receiver with id {0} not found", postMail.To_employeeId));
            }
            Mail mail = new Mail {Name=postMail.Name, Content=postMail.Content, From_employee=fromEmployee, To_employee=toEmployee, Date=postMail.Date};
            
            _context.Mail.Add(mail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMail", new { id = mail.Id }, mail);
        }

        // DELETE: api/Mails/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Mail>> DeleteMail(int id)
        {
            var mail = await _context.Mail.FindAsync(id);
            if (mail == null)
            {
                return NotFound(String.Format("The mail with id {0} not found", id));
            }

            _context.Mail.Remove(mail);
            await _context.SaveChangesAsync();

            return mail;
        }
    }
}
