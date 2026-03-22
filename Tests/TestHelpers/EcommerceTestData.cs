using Ecommerce.API.Models;
using Ecommerce.API.DTOs;

namespace Ecommerce.API.Tests.TestHelpers;

public static class EcommerceTestData
{
    public static Ecommerce GetSampleEcommerce()
    {
        return new Ecommerce
        {
            Id   = 1,
            Name = "Test Ecommerce",
        };
    }

    public static List<Ecommerce> GetSampleEcommerceList(int count = 3)
    {
        return Enumerable.Range(1, count)
            .Select(i => new Ecommerce { Id = i, Name = $"Test Ecommerce {i}" })
            .ToList();
    }
}
