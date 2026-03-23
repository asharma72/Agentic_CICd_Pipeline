using EmployeeApi.Models;
using EmployeeApi.DTOs;

namespace EmployeeApi.Services;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeItem>> GetAllAsync();
    Task<EmployeeItem?> GetByIdAsync(int id);
    Task<EmployeeItem> CreateAsync(CreateEmployeeDto dto);
    Task<EmployeeItem?> UpdateAsync(int id, UpdateEmployeeDto dto);
    Task<bool> DeleteAsync(int id);
}
