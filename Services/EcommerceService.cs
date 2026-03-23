using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

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
                _ecommmerceList.Add(ecommerce);
                _logger.LogInformation("Ecommerce added successfully");
                return ecommerce;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding ecommerce");
                throw;
            }
        }

        public Ecommerce UpdateEcommerce(Ecommerce ecommerce)
        {
            try
            {
                var existingEcommerce = _ecommmerceList.FirstOrDefault(e => e.Id == ecommerce.Id);
                if (existingEcommerce != null)
                {
                    existingEcommerce.Name = ecommerce.Name;
                    existingEcommerce.Description = ecommerce.Description;
                    _logger.LogInformation("Ecommerce updated successfully");
                    return existingEcommerce;
                }
                else
                {
                    _logger.LogWarning("Ecommerce not found");
                    throw new InvalidOperationException("Ecommerce not found");
                }
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
                var ecommerce = _ecommmerceList.FirstOrDefault(e => e.Id == id);
                if (ecommerce != null)
                {
                    _ecommmerceList.Remove(ecommerce);
                    _logger.LogInformation("Ecommerce deleted successfully");
                }
                else
                {
                    _logger.LogWarning("Ecommerce not found");
                    throw new InvalidOperationException("Ecommerce not found");
                }
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
                var ecommerce = _ecommmerceList.FirstOrDefault(e => e.Id == id);
                if (ecommerce != null)
                {
                    _logger.LogInformation("Ecommerce retrieved successfully");
                    return ecommerce;
                }
                else
                {
                    _logger.LogWarning("Ecommerce not found");
                    throw new InvalidOperationException("Ecommerce not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving ecommerce");
                throw;
            }
        }

        public List<Ecommerce> GetAllEcommerce()
        {
            try
            {
                _logger.LogInformation("Ecommerce list retrieved successfully");
                return _ecommmerceList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving ecommerce list");
                throw;
            }
        }
    }
}