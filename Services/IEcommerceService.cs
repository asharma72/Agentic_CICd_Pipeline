using EcommerceApi.Models;
using EcommerceApi.DTOs;

namespace EcommerceApi.Services;

public interface IEcommerceService
{
    Task<IEnumerable<EcommerceItem>> GetAllAsync();
    Task<EcommerceItem?> GetByIdAsync(int id);
    Task<EcommerceItem> CreateAsync(CreateEcommerceDto dto);
    Task<EcommerceItem?> UpdateAsync(int id, UpdateEcommerceDto dto);
    Task<bool> DeleteAsync(int id);
}
