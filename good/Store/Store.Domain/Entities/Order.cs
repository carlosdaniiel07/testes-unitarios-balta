using Store.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using Flunt.Validations;

namespace Store.Domain.Entities
{
    public class Order : Entity
    {
        public string Number { get; private set; }
        public Customer Customer { get; private set; }
        public DateTime Date { get; private set; }
        public decimal DeliveryFee { get; private set; }
        public Discount Discount { get; private set; }
        public OrderStatus Status { get; private set; }
        public IList<OrderItem> Items { get; private set; }

        public Order(Customer customer, decimal deliveryFee, Discount discount)
        {
            AddNotifications(
                new Contract()
                    .Requires()
                    .IsNotNull(customer, "Cliente", "Cliente inválido")
            );

            Customer = customer;
            Date = DateTime.Now;
            Number = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            Status = OrderStatus.WaitingPayment;
            DeliveryFee = deliveryFee;
            Discount = discount;
            Items = new List<OrderItem>();
        }

        public void AddItem(Product product, int quantity)
        {
            var item = new OrderItem(product, quantity);
            
            if (item.Valid)
            {
                Items.Add(item);
            }
        }

        public decimal Total()
        {
            var totalOrder = Items.Select(orderItem => orderItem.Total()).Sum();
            var discount = Discount?.Value() ?? 0;

            return totalOrder + DeliveryFee - discount;
        }

        public void Pay(decimal value)
        {
            if (value == Total())
            {
                Status = OrderStatus.WaitingDelivery;
            }
        }

        public void Cancel()
        {
            Status = OrderStatus.Canceled;
        }
    }
}
