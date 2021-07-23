using Flunt.Notifications;
using Store.Domain.Commands;
using Store.Domain.Commands.Interfaces;
using Store.Domain.Entities;
using Store.Domain.Handlers.Interfaces;
using Store.Domain.Repositories;
using System.Linq;

namespace Store.Domain.Handlers
{
    public class OrderHandler : Notifiable, IHandler<CreateOrderCommand>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IDeliveryFeeRepository _deliveryFeeRepository;
        private readonly IDiscountRepository _discountRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;

        public OrderHandler(
            ICustomerRepository customerRepository,
            IDeliveryFeeRepository deliveryFeeRepository,
            IDiscountRepository discountRepository,
            IProductRepository productRepository,
            IOrderRepository orderRepository)
        {
            _customerRepository = customerRepository;
            _deliveryFeeRepository = deliveryFeeRepository;
            _discountRepository = discountRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }

        public ICommandResult Handle(CreateOrderCommand command)
        {
            command.Validate();

            if (command.Invalid)
            {
                return new GenericCommandResult(false, "Pedido com dados inválidos", command.Notifications);
            }

            // 1. Recupera o cliente
            var customer = _customerRepository.Get(command.Customer);
            
            // 2. Calcula a taxa de entrega
            var deliveryFee = _deliveryFeeRepository.Get(command.ZipCode);

            // 3. Recupera o desconto
            var discount = _discountRepository.Get(command.PromoCode);

            // 4. Recupera os produtos
            var products = _productRepository.Get(command.Items.Select(item => item.ProductId));

            // 5. Criação do pedido
            var order = new Order(customer, deliveryFee, discount);

            command.Items.ToList().ForEach(item =>
            {
                var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                order.AddItem(product, item.Quantity);
            });

            AddNotifications(order.Notifications);

            // 6. Persiste pedido no banco de dados
            _orderRepository.Save(order);

            if (Invalid)
            {
                return new GenericCommandResult(false, "Falha ao gerar pedido", Notifications);
            }

            return new GenericCommandResult(true, $"Pedido ${order.Number} gerado com sucesso", order);
        }
    }
}
