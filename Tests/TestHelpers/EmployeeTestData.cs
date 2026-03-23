using Employee.API.Models;
using Employee.API.DTOs;

namespace Employee.API.Tests.TestHelpers;

public static class EmployeeTestData
{
    public static Employee GetSampleEmployee()
    {
        return new Employee
        {
            Id   = 1,
            Name = "Test Employee",
        };
    }

    public static List<Employee> GetSampleEmployeeList(int count = 3)
    {
        return Enumerable.Range(1, count)
            .Select(i => new Employee { Id = i, Name = $"Test Employee {i}" })
            .ToList();
    }
}
