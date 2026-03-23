using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

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
                _ecommereces.Add(ecommerce);
                _logger.LogInformation($"Ecommerce with id {ecommerce.Id} added successfully.");
                return ecommerce;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding ecommerce.");
                throw new ServiceException("Error adding ecommerce.", ex);
            }
        }

        public List<Ecommerce> GetAllEcommerces()
        {
            try
            {
                return _ecommereces.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all ecommerces.");
                throw new ServiceException("Error getting all ecommerces.", ex);
            }
        }

        public Ecommerce GetEcommerceById(int id)
        {
            try
            {
                var ecommerce = _ecommereces.FirstOrDefault(e => e.Id == id);
                if (ecommerce == null)
                {
                    _logger.LogInformation($"Ecommerce with id {id} not found.");
                    throw new ServiceException($"Ecommerce with id {id} not found.");
                }
                return ecommerce;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting ecommerce by id.");
                throw new ServiceException("Error getting ecommerce by id.", ex);
            }
        }

        public Ecommerce UpdateEcommerce(Ecommerce ecommerce)
        {
            try
            {
                var existingEcommerce = _ecommereces.FirstOrDefault(e => e.Id == ecommerce.Id);
                if (existingEcommerce == null)
                {
                    _logger.LogInformation($"Ecommerce with id {ecommerce.Id} not found.");
                    throw new ServiceException($"Ecommerce with id {ecommerce.Id} not found.");
                }
                existingEcommerce.Name = ecommerce.Name;
                existingEcommerce.Description = ecommerce.Description;
                _logger.LogInformation($"Ecommerce with id {ecommerce.Id} updated successfully.");
                return existingEcommerce;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating ecommerce.");
                throw new ServiceException("Error updating ecommerce.", ex);
            }
        }

        public void DeleteEcommerce(int id)
        {
            try
            {
                var ecommerce = _ecommereces.FirstOrDefault(e => e.Id == id);
                if (ecommerce == null)
                {
                    _logger.LogInformation($"Ecommerce with id {id} not found.");
                    throw new ServiceException($"Ecommerce with id {id} not found.");
                }
                _ecommereces.Remove(ecommerce);
                _logger.LogInformation($"Ecommerce with id {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting ecommerce.");
                throw new ServiceException("Error deleting ecommerce.", ex);
            }
        }
    }

    public interface IEcommerceService
    {
        Ecommerce AddEcommerce(Ecommerce ecommerce);
        List<Ecommerce> GetAllEcommerces();
        Ecommerce GetEcommerceById(int id);
        Ecommerce UpdateEcommerce(Ecommerce ecommerce);
        void DeleteEcommerce(int id);
    }

    public class Ecommerce
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class ServiceException : Exception
    {
        public ServiceException(string message) : base(message) { }
        public ServiceException(string message, Exception innerException) : base(message, innerException) { }
    }
}