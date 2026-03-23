using Ecommerce.API.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ecommerce.API.Services
{
    public class EcommerceService : IEcommerceService
    {
        private readonly List<Ecommerce> _ecommmerceList;
        private readonly ILogger _logger;

        public EcommerceService(ILogger<EcommerceService> logger)
        {
            _ecommmerceList = new List<Ecommerce>();
            _logger = logger;
        }

        public Ecommerce AddEcommerce(Ecommerce ecommerce)
        {
            try
            {
                if (ecommerce == null)
                {
                    throw new ArgumentNullException(nameof(ecommerce), "Ecommerce cannot be null");
                }

                _ecommmerceList.Add(ecommerce);
                return ecommerce;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding ecommerce");
                throw;
            }
        }

        public Ecommerce GetEcommerce(int id)
        {
            try
            {
                var ecommerce = _ecommmerceList.FirstOrDefault(e => e.Id == id);
                if (ecommerce == null)
                {
                    throw new InvalidOperationException($"Ecommerce with id {id} not found");
                }

                return ecommerce;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting ecommerce");
                throw;
            }
        }

        public IEnumerable<Ecommerce> GetEcommerces()
        {
            try
            {
                return _ecommmerceList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting ecommerces");
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

                var existingEcommerce = _ecommmerceList.FirstOrDefault(e => e.Id == ecommerce.Id);
                if (existingEcommerce == null)
                {
                    throw new InvalidOperationException($"Ecommerce with id {ecommerce.Id} not found");
                }

                existingEcommerce.Name = ecommerce.Name;
                existingEcommerce.Description = ecommerce.Description;

                return existingEcommerce;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating ecommerce");
                throw;
            }
        }

        public void DeleteEcommerce(int id)
        {
            try
            {
                var ecommerce = _ecommmerceList.FirstOrDefault(e => e.Id == id);
                if (ecommerce == null)
                {
                    throw new InvalidOperationException($"Ecommerce with id {id} not found");
                }

                _ecommmerceList.Remove(ecommerce);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting ecommerce");
                throw;
            }
        }
    }

    public interface IEcommerceService
    {
        Ecommerce AddEcommerce(Ecommerce ecommerce);
        Ecommerce GetEcommerce(int id);
        IEnumerable<Ecommerce> GetEcommerces();
        Ecommerce UpdateEcommerce(Ecommerce ecommerce);
        void DeleteEcommerce(int id);
    }
}