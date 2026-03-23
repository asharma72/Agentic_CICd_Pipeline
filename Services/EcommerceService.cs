using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Ecommerce.API.Services
{
    public class EcommerceService : IEcommerceService
    {
        private readonly List<Ecommerce> _ecommereces;
        private readonly ILogger<EcommerceService> _logger;

        public EcommerceService(ILogger<EcommerceService> logger)
        {
            _ecommereces = new List<Ecommerce>();
            _logger = logger;
        }

        public IEnumerable<Ecommerce> GetAll()
        {
            try
            {
                return _ecommereces.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all ecommerces");
                throw;
            }
        }

        public Ecommerce GetById(int id)
        {
            try
            {
                return _ecommereces.FirstOrDefault(e => e.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting ecommerce by id");
                throw;
            }
        }

        public Ecommerce Create(Ecommerce ecommerce)
        {
            try
            {
                if (string.IsNullOrEmpty(ecommerce.Name))
                {
                    throw new ArgumentException("Ecommerce name cannot be null or empty");
                }

                if (_ecommereces.Any(e => e.Name == ecommerce.Name))
                {
                    throw new InvalidOperationException("Ecommerce with the same name already exists");
                }

                _ecommereces.Add(ecommerce);
                return ecommerce;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating ecommerce");
                throw;
            }
        }

        public void Update(Ecommerce ecommerce)
        {
            try
            {
                var existingEcommerce = _ecommereces.FirstOrDefault(e => e.Id == ecommerce.Id);

                if (existingEcommerce == null)
                {
                    throw new ArgumentException("Ecommerce not found");
                }

                existingEcommerce.Name = ecommerce.Name;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating ecommerce");
                throw;
            }
        }

        public void Delete(int id)
        {
            try
            {
                var ecommerce = _ecommereces.FirstOrDefault(e => e.Id == id);

                if (ecommerce == null)
                {
                    throw new ArgumentException("Ecommerce not found");
                }

                _ecommereces.Remove(ecommerce);
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
        IEnumerable<Ecommerce> GetAll();
        Ecommerce GetById(int id);
        Ecommerce Create(Ecommerce ecommerce);
        void Update(Ecommerce ecommerce);
        void Delete(int id);
    }

    public class Ecommerce
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}