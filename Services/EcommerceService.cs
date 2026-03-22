using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

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

        public void AddEcommerce(Ecommerce ecommerce)
        {
            try
            {
                _ecommerces.Add(ecommerce);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding ecommerce");
                throw;
            }
        }

        public void DeleteEcommerce(int id)
        {
            try
            {
                var ecommerce = _ecommerces.FirstOrDefault(e => e.Id == id);
                if (ecommerce == null)
                {
                    _logger.LogWarning($"Ecommerce with id {id} not found");
                    throw new InvalidOperationException($"Ecommerce with id {id} not found");
                }
                _ecommerces.Remove(ecommerce);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting ecommerce");
                throw;
            }
        }

        public Ecommerce GetEcommerce(int id)
        {
            try
            {
                return _ecommerces.FirstOrDefault(e => e.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting ecommerce");
                throw;
            }
        }

        public List<Ecommerce> GetEcommerces()
        {
            try
            {
                return _ecommerces;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting ecommerces");
                throw;
            }
        }

        public void UpdateEcommerce(Ecommerce ecommerce)
        {
            try
            {
                var existingEcommerce = _ecommerces.FirstOrDefault(e => e.Id == ecommerce.Id);
                if (existingEcommerce == null)
                {
                    _logger.LogWarning($"Ecommerce with id {ecommerce.Id} not found");
                    throw new InvalidOperationException($"Ecommerce with id {ecommerce.Id} not found");
                }
                _ecommerces.Remove(existingEcommerce);
                _ecommerces.Add(ecommerce);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating ecommerce");
                throw;
            }
        }
    }
}