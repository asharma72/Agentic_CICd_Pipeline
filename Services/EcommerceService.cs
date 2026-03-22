using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Ecommerce.API.Services
{
    public class EcommerceService : IEcommerceService
    {
        private readonly List<Ecommerce> _ecommences;
        private readonly ILogger<EcommerceService> _logger;

        public EcommerceService(ILogger<EcommerceService> logger)
        {
            _logger = logger;
            _ecommences = new List<Ecommerce>();
        }

        public Ecommerce AddEcommerce(Ecommerce ecommerce)
        {
            try
            {
                _ecommences.Add(ecommerce);
                _logger.LogInformation($"Ecommerce added with id: {ecommerce.Id}");
                return ecommerce;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding ecommerce");
                throw;
            }
        }

        public List<Ecommerce> GetAllEcommerces()
        {
            try
            {
                return _ecommences.ToList();
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
                var ecommerce = _ecommences.FirstOrDefault(e => e.Id == id);
                if (ecommerce == null)
                {
                    _logger.LogInformation($"Ecommerce not found with id: {id}");
                    throw new InvalidOperationException($"Ecommerce not found with id: {id}");
                }
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
                var existingEcommerce = _ecommences.FirstOrDefault(e => e.Id == ecommerce.Id);
                if (existingEcommerce == null)
                {
                    _logger.LogInformation($"Ecommerce not found with id: {ecommerce.Id}");
                    throw new InvalidOperationException($"Ecommerce not found with id: {ecommerce.Id}");
                }
                existingEcommerce.Name = ecommerce.Name;
                existingEcommerce.Description = ecommerce.Description;
                _logger.LogInformation($"Ecommerce updated with id: {ecommerce.Id}");
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
                var ecommerce = _ecommences.FirstOrDefault(e => e.Id == id);
                if (ecommerce == null)
                {
                    _logger.LogInformation($"Ecommerce not found with id: {id}");
                    throw new InvalidOperationException($"Ecommerce not found with id: {id}");
                }
                _ecommences.Remove(ecommerce);
                _logger.LogInformation($"Ecommerce deleted with id: {id}");
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
}