using Microsoft.EntityFrameworkCore;
using SingleStage.Entities;

namespace SingleStage.DAC
{
    public class EmployeeDAC : BaseDAC<Employee>
    {
        public EmployeeDAC(SingleStageMvvmContext context) : base(context) { }

        public Task<Employee?> GetFirstOrDefaultByUsernameAsync(string username)
        {
            return _context.Employees
                .FirstOrDefaultAsync(employee => employee.Username == username);
        }
    }
}
