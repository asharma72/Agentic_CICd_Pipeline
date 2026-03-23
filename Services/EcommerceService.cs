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
            _ecommmerces = new List<Ecommerce>();
            _logger = logger;
        }

        public Ecommerce AddEcommerce(Ecommerce ecommerce)
        {
            try
            {
                _ecommmerces.Add(ecommerce);
                _logger.LogInformation($"Ecommerce added successfully. Id: {ecommerce.Id}");
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
                var ecommerce = _ecommmerces.FirstOrDefault(e => e.Id == id);
                if (ecommerce == null)
                {
                    _logger.LogWarning($"Ecommerce not found. Id: {id}");
                    throw new InvalidOperationException($"Ecommerce not found. Id: {id}");
                }
                _logger.LogInformation($"Ecommerce retrieved successfully. Id: {id}");
                return ecommerce;
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
                _logger.LogInformation("Ecommerces retrieved successfully");
                return _ecommmerces.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting ecommerces");
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
                    _logger.LogWarning($"Ecommerce not found. Id: {ecommerce.Id}");
                    throw new InvalidOperationException($"Ecommerce not found. Id: {ecommerce.Id}");
                }
                existingEcommerce.Name = ecommerce.Name;
                existingEcommerce.Description = ecommerce.Description;
                _logger.LogInformation($"Ecommerce updated successfully. Id: {ecommerce.Id}");
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
                var ecommerce = _ecommmerces.FirstOrDefault(e => e.Id == id);
                if (ecommerce == null)
                {
                    _logger.LogWarning($"Ecommerce not found. Id: {id}");
                    throw new InvalidOperationException($"Ecommerce not found. Id: {id}");
                }
                _ecommmerces.Remove(ecommerce);
                _logger.LogInformation($"Ecommerce deleted successfully. Id: {id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting ecommerce");
                throw;
            }
        }
    }

    public interface IEcommerceService
    {
        Ecommerce AddEcommerce(Ecommerce ecommerce);
        Ecommerce GetEcommerce(int id);
        List<Ecommerce> GetEcommerces();
        Ecommerce UpdateEcommerce(Ecommerce ecommerce);
        void DeleteEcommerce(int id);
    }

    public class Ecommerce
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}