using Store.Domain.Entities;
using System;

namespace Store.Domain.Queries
{
    public static class ProductQueries
    {
        public static Func<Product, bool> GetActiveProducts()
        {
            return product => product.Active;
        }

        public static Func<Product, bool> GetInactiveProducts()
        {
            return product => !product.Active;
        }
    }
}
