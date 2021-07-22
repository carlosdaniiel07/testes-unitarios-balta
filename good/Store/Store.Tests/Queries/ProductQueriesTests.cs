using Store.Domain.Entities;
using System;
using System.Linq;
using Xunit;
using Store.Domain.Queries;
using System.Collections.Generic;

namespace Store.Tests.Queries
{
    public class ProductQueriesTests
    {
        private readonly IList<Product> _products = new List<Product>()
        {
            new Product("Produto 01", 5, true),
            new Product("Produto 02", 10, false),
            new Product("Produto 03", 15, true),
            new Product("Produto 04", 20, false),
            new Product("Produto 05", 25, true),
        };

        [Fact]
        public void Dado_a_consulta_de_produtos_ativos_deve_retornar_3()
        {
            Assert.Equal(3, _products.Count(ProductQueries.GetActiveProducts()));
        }

        [Fact]
        public void Dado_a_consulta_de_produtos_inativos_deve_retornar_2()
        {
            Assert.Equal(2, _products.Count(ProductQueries.GetInactiveProducts()));
        }
    }
}
