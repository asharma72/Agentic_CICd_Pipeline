using Ecommerce.API.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ecommerce.API.Services
{
    public class EcommerceService : IEcommerceService
    {
        private readonly ILogger<EcommerceService> _logger;
        private List<Ecommerce> _ecommereces;

        public EcommerceService(ILogger<EcommerceService> logger)
        {
            _logger = logger;
            _ecommereces = new List<Ecommerce>();
        }

        public Ecommerce AddEcommerce(Ecommerce ecommerce)
        {
            try
            {
                if (string.IsNullOrEmpty(ecommerce.Name))
                {
                    throw new ArgumentException("Name is required", nameof(ecommerce));
                }

                if (_ecommereces.Any(e => e.Name == ecommerce.Name))
                {
                    throw new InvalidOperationException("Ecommerce with the same name already exists");
                }

                _ecommereces.Add(ecommerce);
                _logger.LogInformation($"Ecommerce {ecommerce.Name} added successfully");
                return ecommerce;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding ecommerce");
                throw;
            }
        }

        public Ecommerce GetEcommerce(int id)
        {
            try
            {
                var ecommerce = _ecommereces.FirstOrDefault(e => e.Id == id);
                if (ecommerce == null)
                {
                    throw new KeyNotFoundException($"Ecommerce with id {id} not found");
                }

                _logger.LogInformation($"Ecommerce {ecommerce.Name} retrieved successfully");
                return ecommerce;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving ecommerce");
                throw;
            }
        }

        public List<Ecommerce> GetEcommerces()
        {
            try
            {
                _logger.LogInformation("Ecommerces retrieved successfully");
                return _ecommereces.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving ecommerces");
                throw;
            }
        }

        public Ecommerce UpdateEcommerce(int id, Ecommerce ecommerce)
        {
            try
            {
                if (string.IsNullOrEmpty(ecommerce.Name))
                {
                    throw new ArgumentException("Name is required", nameof(ecommerce));
                }

                var existingEcommerce = _ecommereces.FirstOrDefault(e => e.Id == id);
                if (existingEcommerce == null)
                {
                    throw new KeyNotFoundException($"Ecommerce with id {id} not found");
                }

                existingEcommerce.Name = ecommerce.Name;
                _logger.LogInformation($"Ecommerce {existingEcommerce.Name} updated successfully");
                return existingEcommerce;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating ecommerce");
                throw;
            }
        }

        public void DeleteEcommerce(int id)
        {
            try
            {
                var ecommerce = _ecommereces.FirstOrDefault(e => e.Id == id);
                if (ecommerce == null)
                {
                    throw new KeyNotFoundException($"Ecommerce with id {id} not found");
                }

                _ecommereces.Remove(ecommerce);
                _logger.LogInformation($"Ecommerce {ecommerce.Name} deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting ecommerce");
                throw;
            }
        }
    }
}