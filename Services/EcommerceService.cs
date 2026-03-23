using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Ecommerce.API.Services
{
    public class EcommerceService : IEcommerceService
    {
        private readonly List<Ecommerce> _ecommereces;
        private readonly ILogger<EcommerceService> _logger;

        public EcommerceService(ILogger<EcommerceService> logger)
        {
            _logger = logger;
            _ecommereces = new List<Ecommerce>();
        }

        public async Task<Ecommerce> GetByIdAsync(int id)
        {
            try
            {
                var ecommerce = _ecommereces.FirstOrDefault(e => e.Id == id);
                if (ecommerce == null)
                {
                    _logger.LogInformation($"Ecommerce with id {id} not found");
                    throw new InvalidOperationException($"Ecommerce with id {id} not found");
                }
                return ecommerce;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting ecommerce by id {id}");
                throw;
            }
        }

        public async Task<IEnumerable<Ecommerce>> GetAllAsync()
        {
            try
            {
                return _ecommereces.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all ecommerces");
                throw;
            }
        }

        public async Task<Ecommerce> CreateAsync(Ecommerce ecommerce)
        {
            try
            {
                if (ecommerce == null)
                {
                    _logger.LogInformation("Ecommerce is null");
                    throw new ArgumentNullException(nameof(ecommerce));
                }
                if (_ecommereces.Any(e => e.Id == ecommerce.Id))
                {
                    _logger.LogInformation($"Ecommerce with id {ecommerce.Id} already exists");
                    throw new InvalidOperationException($"Ecommerce with id {ecommerce.Id} already exists");
                }
                _ecommereces.Add(ecommerce);
                return ecommerce;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creating ecommerce {ecommerce.Id}");
                throw;
            }
        }

        public async Task UpdateAsync(Ecommerce ecommerce)
        {
            try
            {
                if (ecommerce == null)
                {
                    _logger.LogInformation("Ecommerce is null");
                    throw new ArgumentNullException(nameof(ecommerce));
                }
                var existingEcommerce = _ecommereces.FirstOrDefault(e => e.Id == ecommerce.Id);
                if (existingEcommerce == null)
                {
                    _logger.LogInformation($"Ecommerce with id {ecommerce.Id} not found");
                    throw new InvalidOperationException($"Ecommerce with id {ecommerce.Id} not found");
                }
                existingEcommerce.Name = ecommerce.Name;
                existingEcommerce.Description = ecommerce.Description;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating ecommerce {ecommerce.Id}");
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var ecommerce = _ecommereces.FirstOrDefault(e => e.Id == id);
                if (ecommerce == null)
                {
                    _logger.LogInformation($"Ecommerce with id {id} not found");
                    throw new InvalidOperationException($"Ecommerce with id {id} not found");
                }
                _ecommereces.Remove(ecommerce);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting ecommerce {id}");
                throw;
            }
        }
    }
}