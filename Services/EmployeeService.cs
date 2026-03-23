using EmployeeApi.Models;
using EmployeeApi.DTOs;

namespace EmployeeApi.Services;

public class EmployeeService : IEmployeeService
{
    private readonly List<EmployeeItem> _items = new();
    private int _nextId = 1;

    public Task<IEnumerable<EmployeeItem>> GetAllAsync()
        => Task.FromResult(_items.AsEnumerable());

    public Task<EmployeeItem?> GetByIdAsync(int id)
        => Task.FromResult(_items.FirstOrDefault(x => x.Id == id));

    public Task<EmployeeItem> CreateAsync(CreateEmployeeDto dto)
    {
        var item = new EmployeeItem
        {
            Id          = _nextId++,
            Name        = dto.Name,
            Description = dto.Description,
            Price       = dto.Price,
            Stock       = dto.Stock,
            CreatedAt   = DateTime.UtcNow,
            UpdatedAt   = DateTime.UtcNow
        };
        _items.Add(item);
        return Task.FromResult(item);
    }

    public Task<EmployeeItem?> UpdateAsync(int id, UpdateEmployeeDto dto)
    {
        var item = _items.FirstOrDefault(x => x.Id == id);
        if (item == null) return Task.FromResult<EmployeeItem?>(null);
        if (dto.Name        != null) item.Name        = dto.Name;
        if (dto.Description != null) item.Description = dto.Description;
        if (dto.Price       != null) item.Price       = dto.Price.Value;
        if (dto.Stock       != null) item.Stock       = dto.Stock.Value;
        if (dto.IsActive    != null) item.IsActive    = dto.IsActive.Value;
        item.UpdatedAt = DateTime.UtcNow;
        return Task.FromResult<EmployeeItem?>(item);
    }

    public Task<bool> DeleteAsync(int id)
    {
        var item = _items.FirstOrDefault(x => x.Id == id);
        if (item == null) return Task.FromResult(false);
        _items.Remove(item);
        return Task.FromResult(true);
    }
}
