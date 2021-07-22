using Store.Domain.Entities;
using Store.Domain.Repositories;
using System;

namespace Store.Tests.Repositories
{
    public class FakeDiscountRepository : IDiscountRepository
    {
        public Discount Get(string code)
        {
            return code switch
            {
                "123" => new Discount(10, DateTime.Now.AddDays(2)),
                "1234" => new Discount(10, DateTime.Now.AddDays(-2)),
                _ => null,
            };
        }
    }
}
