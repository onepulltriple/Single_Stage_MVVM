using SingleStage.DAC;
using SingleStage.Entities;
using System.Collections.ObjectModel;

namespace SingleStage.ViewModels
{
    public class EmployeeViewModel : ViewModelBase
    {
        private readonly EmployeeDAC _employeeDAC;

        public ObservableCollection<Employee> Employees { get; } = new();

        public EmployeeViewModel(EmployeeDAC employeeDAC)
        {
            _employeeDAC = employeeDAC;
        }

        public async Task LoadEmployeesAsync()
        {
            Employees.Clear();

            var employees = await _employeeDAC.GetAllAsync();

            foreach (var employee in employees)
            {
                Employees.Add(employee);
            }
        }
    }
}
