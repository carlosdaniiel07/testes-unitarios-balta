using Store.Domain.Repositories;

namespace Store.Tests.Repositories
{
    public class FakeDeliveryFeeRepository : IDeliveryFeeRepository
    {
        public decimal Get(string zipCode)
        {
            if (string.IsNullOrEmpty(zipCode))
            {
                return 0;
            }

            return 10;
        }
    }
}
