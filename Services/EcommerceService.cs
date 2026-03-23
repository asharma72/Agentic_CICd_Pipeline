using Ecommerce.API.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ecommerce.API.Services
{
    public class EcommerceService : IEcommerceService
    {
        private readonly List<Ecommerce> _ecommereces;
        private readonly ILogger _logger;

        public EcommerceService(ILogger<EcommerceService> logger)
        {
            _ecommereces = new List<Ecommerce>();
            _logger = logger;
        }

        public Ecommerce AddEcommerce(Ecommerce ecommerce)
        {
            try
            {
                if (ecommmerce == null)
                {
                    throw new ArgumentNullException(nameof(ecommmerce));
                }

                _ecommereces.Add(ecommmerce);
                _logger.LogInformation($"Ecommerce added: {ecommmerce.Id}");
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
                if (ecommmerce == null)
                {
                    throw new InvalidOperationException($"Ecommerce with id {id} not found");
                }

                _logger.LogInformation($"Ecommerce retrieved: {ecommmerce.Id}");
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
                var ecommerces = _ecommereces.ToList();
                _logger.LogInformation($"Ecommerces retrieved: {ecommereces.Count}");
                return ecommerces;
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
                if (ecommmerce == null)
                {
                    throw new ArgumentNullException(nameof(ecommmerce));
                }

                var existingEcommerce = _ecommereces.FirstOrDefault(e => e.Id == id);
                if (existingEcommerce == null)
                {
                    throw new InvalidOperationException($"Ecommerce with id {id} not found");
                }

                existingEcommerce.Name = ecommerce.Name;
                existingEcommerce.Description = ecommerce.Description;
                _logger.LogInformation($"Ecommerce updated: {existingEcommerce.Id}");
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
                if (ecommmerce == null)
                {
                    throw new InvalidOperationException($"Ecommerce with id {id} not found");
                }

                _ecommereces.Remove(ecommmerce);
                _logger.LogInformation($"Ecommerce deleted: {ecommmerce.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting ecommerce");
                throw;
            }
        }
    }
}