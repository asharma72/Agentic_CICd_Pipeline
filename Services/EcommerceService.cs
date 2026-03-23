using Ecommerce.API.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ecommerce.API.Services
{
    public class EcommerceService : IEcommerceService
    {
        private readonly List<Ecommerce> _ecommereceList;
        private readonly ILogger _logger;

        public EcommerceService(ILogger<EcommerceService> logger)
        {
            _logger = logger;
            _ecommereceList = new List<Ecommerce>();
        }

        public Ecommerce AddEcommerce(Ecommerce ecommerce)
        {
            try
            {
                if (ecommerce == null)
                {
                    throw new ArgumentNullException(nameof(ecommerce), "Ecommerce cannot be null");
                }

                if (_ecommereceList.Any(e => e.Id == ecommerce.Id))
                {
                    throw new InvalidOperationException("Ecommerce with the same id already exists");
                }

                _ecommereceList.Add(ecommerce);
                return ecommerce;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding ecommerce");
                throw;
            }
        }

        public Ecommerce GetEcommerceById(int id)
        {
            try
            {
                return _ecommereceList.FirstOrDefault(e => e.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting ecommerce by id");
                throw;
            }
        }

        public List<Ecommerce> GetAllEcommerces()
        {
            try
            {
                return _ecommereceList.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all ecommerces");
                throw;
            }
        }

        public Ecommerce UpdateEcommerce(Ecommerce ecommerce)
        {
            try
            {
                if (ecommerce == null)
                {
                    throw new ArgumentNullException(nameof(ecommerce), "Ecommerce cannot be null");
                }

                var existingEcommerce = _ecommereceList.FirstOrDefault(e => e.Id == ecommerce.Id);
                if (existingEcommerce == null)
                {
                    throw new InvalidOperationException("Ecommerce not found");
                }

                existingEcommerce.Name = ecommerce.Name;
                existingEcommerce.Description = ecommerce.Description;
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
                var ecommerce = _ecommereceList.FirstOrDefault(e => e.Id == id);
                if (ecommerce == null)
                {
                    throw new InvalidOperationException("Ecommerce not found");
                }

                _ecommereceList.Remove(ecommerce);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting ecommerce");
                throw;
            }
        }
    }
}