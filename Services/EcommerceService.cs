using EcommerceApi.Models;
using EcommerceApi.DTOs;

namespace EcommerceApi.Services;

public class EcommerceService : IEcommerceService
{
    private readonly List<EcommerceItem> _items = new();
    private int _nextId = 1;

    public Task<IEnumerable<EcommerceItem>> GetAllAsync()
        => Task.FromResult(_items.AsEnumerable());

    public Task<EcommerceItem?> GetByIdAsync(int id)
        => Task.FromResult(_items.FirstOrDefault(x => x.Id == id));

    public Task<EcommerceItem> CreateAsync(CreateEcommerceDto dto)
    {
        var item = new EcommerceItem
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

    public Task<EcommerceItem?> UpdateAsync(int id, UpdateEcommerceDto dto)
    {
        var item = _items.FirstOrDefault(x => x.Id == id);
        if (item == null) return Task.FromResult<EcommerceItem?>(null);
        if (dto.Name        != null) item.Name        = dto.Name;
        if (dto.Description != null) item.Description = dto.Description;
        if (dto.Price       != null) item.Price       = dto.Price.Value;
        if (dto.Stock       != null) item.Stock       = dto.Stock.Value;
        if (dto.IsActive    != null) item.IsActive    = dto.IsActive.Value;
        item.UpdatedAt = DateTime.UtcNow;
        return Task.FromResult<EcommerceItem?>(item);
    }

    public Task<bool> DeleteAsync(int id)
    {
        var item = _items.FirstOrDefault(x => x.Id == id);
        if (item == null) return Task.FromResult(false);
        _items.Remove(item);
        return Task.FromResult(true);
    }
}
