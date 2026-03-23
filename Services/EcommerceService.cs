using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Ecommerce.API.Services
{
    public class EcommerceService : IEcommerceService
    {
        private readonly List<Ecommerce> _ecommmerces;
        private readonly ILogger<EcommerceService> _logger;

        public EcommerceService(ILogger<EcommerceService> logger)
        {
            _logger = logger;
            _ecommmerces = new List<Ecommerce>();
        }

        public Ecommerce AddEcommerce(Ecommerce ecommerce)
        {
            try
            {
                _ecommmerces.Add(ecommerce);
                _logger.LogInformation($"Ecommerce added successfully: {ecommerce.Id}");
                return ecommerce;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding ecommerce");
                throw;
            }
        }

        public IEnumerable<Ecommerce> GetAllEcommerces()
        {
            try
            {
                return _ecommmerces.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all ecommerces");
                throw;
            }
        }

        public Ecommerce GetEcommerceById(int id)
        {
            try
            {
                var ecommerce = _ecommmerces.FirstOrDefault(e => e.Id == id);
                if (ecommerce == null)
                {
                    _logger.LogWarning($"Ecommerce not found: {id}");
                    throw new InvalidOperationException($"Ecommerce not found: {id}");
                }
                _logger.LogInformation($"Ecommerce found: {id}");
                return ecommerce;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting ecommerce by id");
                throw;
            }
        }

        public Ecommerce UpdateEcommerce(Ecommerce ecommerce)
        {
            try
            {
                var existingEcommerce = _ecommmerces.FirstOrDefault(e => e.Id == ecommerce.Id);
                if (existingEcommerce == null)
                {
                    _logger.LogWarning($"Ecommerce not found: {ecommerce.Id}");
                    throw new InvalidOperationException($"Ecommerce not found: {ecommerce.Id}");
                }
                _ecommmerces.Remove(existingEcommerce);
                _ecommmerces.Add(ecommerce);
                _logger.LogInformation($"Ecommerce updated successfully: {ecommerce.Id}");
                return ecommerce;
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
                var ecommerce = _ecommmerces.FirstOrDefault(e => e.Id == id);
                if (ecommerce == null)
                {
                    _logger.LogWarning($"Ecommerce not found: {id}");
                    throw new InvalidOperationException($"Ecommerce not found: {id}");
                }
                _ecommmerces.Remove(ecommerce);
                _logger.LogInformation($"Ecommerce deleted successfully: {id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting ecommerce");
                throw;
            }
        }
    }
}