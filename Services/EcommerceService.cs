using Ecommerce.API.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ecommerce.API.Services
{
    public class EcommerceService : IEcommerceService
    {
        private readonly List<Ecommerce> _ecommerces;
        private readonly ILogger _logger;

        public EcommerceService(ILogger<EcommerceService> logger)
        {
            _logger = logger;
            _ecommerces = new List<Ecommerce>();
        }

        public Ecommerce AddEcommerce(Ecommerce ecommerce)
        {
            try
            {
                ecommerce.Id = Guid.NewGuid();
                _ecommerces.Add(ecommerce);
                _logger.LogInformation($"Ecommerce added successfully. Id: {ecommerce.Id}");
                return ecommerce;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding ecommerce");
                throw new InvalidOperationException("Failed to add ecommerce", ex);
            }
        }

        public Ecommerce GetEcommerce(Guid id)
        {
            try
            {
                var ecommerce = _ecommerces.FirstOrDefault(e => e.Id == id);
                if (ecommerce == null)
                {
                    _logger.LogWarning($"Ecommerce not found. Id: {id}");
                    throw new InvalidOperationException("Ecommerce not found");
                }
                _logger.LogInformation($"Ecommerce retrieved successfully. Id: {id}");
                return ecommerce;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving ecommerce");
                throw new InvalidOperationException("Failed to retrieve ecommerce", ex);
            }
        }

        public List<Ecommerce> GetEcommerces()
        {
            try
            {
                _logger.LogInformation("Ecommerces retrieved successfully");
                return _ecommerces.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving ecommerces");
                throw new InvalidOperationException("Failed to retrieve ecommerces", ex);
            }
        }

        public Ecommerce UpdateEcommerce(Ecommerce ecommerce)
        {
            try
            {
                var existingEcommerce = _ecommerces.FirstOrDefault(e => e.Id == ecommerce.Id);
                if (existingEcommerce == null)
                {
                    _logger.LogWarning($"Ecommerce not found. Id: {ecommerce.Id}");
                    throw new InvalidOperationException("Ecommerce not found");
                }
                existingEcommerce.Name = ecommerce.Name;
                existingEcommerce.Description = ecommerce.Description;
                _logger.LogInformation($"Ecommerce updated successfully. Id: {ecommerce.Id}");
                return existingEcommerce;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating ecommerce");
                throw new InvalidOperationException("Failed to update ecommerce", ex);
            }
        }

        public void DeleteEcommerce(Guid id)
        {
            try
            {
                var ecommerce = _ecommerces.FirstOrDefault(e => e.Id == id);
                if (ecommerce == null)
                {
                    _logger.LogWarning($"Ecommerce not found. Id: {id}");
                    throw new InvalidOperationException("Ecommerce not found");
                }
                _ecommerces.Remove(ecommerce);
                _logger.LogInformation($"Ecommerce deleted successfully. Id: {id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting ecommerce");
                throw new InvalidOperationException("Failed to delete ecommerce", ex);
            }
        }
    }
}