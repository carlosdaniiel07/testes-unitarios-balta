using Store.Domain.Entities;
using Store.Domain.Repositories;
using System;
using System.Collections.Generic;

namespace Store.Tests.Repositories
{
    public class FakeProductRepository : IProductRepository
    {
        public IEnumerable<Product> Get(IEnumerable<Guid> ids)
        {
            return new List<Product>()
            {
                new Product("Produto 01", 5, true),
                new Product("Produto 02", 10, false),
                new Product("Produto 03", 15, true),
                new Product("Produto 04", 20, false),
                new Product("Produto 05", 25, true),
            };
        }
    }
}
