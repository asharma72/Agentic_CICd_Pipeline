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
                if (ecommerce == null)
                {
                    throw new ArgumentNullException(nameof(ecommerce));
                }

                if (string.IsNullOrEmpty(ecommerce.Name))
                {
                    throw new ArgumentException("Name is required", nameof(ecommerce.Name));
                }

                _ecommerces.Add(ecommerce);
                _logger.LogInformation($"Ecommerce added: {ecommerce.Name}");
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
                var ecommerce = _ecommerces.FirstOrDefault(e => e.Id == id);
                if (ecommerce == null)
                {
                    throw new ArgumentException("Ecommerce not found", nameof(id));
                }

                _logger.LogInformation($"Ecommerce retrieved: {ecommerce.Name}");
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
                var ecommerces = _ecommerces.ToList();
                _logger.LogInformation($"Ecommerces retrieved: {ecommmerces.Count}");
                return ecommerces;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving ecommerces");
                throw;
            }
        }

        public Ecommerce UpdateEcommerce(Ecommerce ecommerce)
        {
            try
            {
                if (ecommerce == null)
                {
                    throw new ArgumentNullException(nameof(ecommerce));
                }

                var existingEcommerce = _ecommerces.FirstOrDefault(e => e.Id == ecommerce.Id);
                if (existingEcommerce == null)
                {
                    throw new ArgumentException("Ecommerce not found", nameof(ecommerce.Id));
                }

                existingEcommerce.Name = ecommerce.Name;
                _logger.LogInformation($"Ecommerce updated: {existingEcommerce.Name}");
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
                var ecommerce = _ecommerces.FirstOrDefault(e => e.Id == id);
                if (ecommerce == null)
                {
                    throw new ArgumentException("Ecommerce not found", nameof(id));
                }

                _ecommerces.Remove(ecommerce);
                _logger.LogInformation($"Ecommerce deleted: {id}");
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
}