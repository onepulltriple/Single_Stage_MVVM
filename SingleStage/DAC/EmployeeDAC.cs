using Microsoft.EntityFrameworkCore;
using SingleStage.Entities;

namespace SingleStage.DAC
{
    public class EmployeeDAC
    {
        private readonly SingleStageMvvmContext _context;

        public EmployeeDAC(SingleStageMvvmContext context)
        {
            _context = context;
        }

        // read all employees
        public Task<List<Employee>> GetAllAsync()
        {
            return _context.Employees.ToListAsync();
        }

        // read one employee
        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        // add employee
        public async Task AddAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
        }

        // update employee
        public async Task UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }

        // delete employee
        public async Task DeleteAsync(int id)
        {
            Employee? employee = await _context.Employees.FindAsync(id);

            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
        }

        public Task<Employee?> GetFirstOrDefaultByUsernameAsync(string username)
        {
            return _context.Employees
                .FirstOrDefaultAsync(employee => employee.Username == username);
        }
    }
}
