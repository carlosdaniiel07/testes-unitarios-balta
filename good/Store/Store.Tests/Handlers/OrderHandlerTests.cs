using Store.Domain.Commands;
using Store.Domain.Handlers;
using Store.Domain.Repositories;
using Store.Tests.Repositories;
using System;
using Xunit;

namespace Store.Tests.Handlers
{
    public class OrderHandlerTests
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IDeliveryFeeRepository _deliveryFeeRepository;
        private readonly IDiscountRepository _discountRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;

        public OrderHandlerTests()
        {
            _customerRepository = new FakeCustomerRepository();
            _deliveryFeeRepository = new FakeDeliveryFeeRepository();
            _discountRepository = new FakeDiscountRepository();
            _productRepository = new FakeProductRepository();
            _orderRepository = new FakeOrderRepository(); ;
        }

        /*[Fact]
        public void Dado_um_cliente_inexistente_o_pedido_nao_deve_ser_gerado()
        {

        }

        [Fact]
        public void Dado_um_cep_invalido_o_pedido_nao_deve_ser_gerado()
        {

        }

        [Fact]
        public void Dado_um_promocode_inexistente_o_pedido_deve_ser_gerado()
        {

        }

        [Fact]
        public void Dado_um_pedido_sem_itens_o_mesmo_nao_deve_ser_gerado()
        {

        }

        [Fact]
        public void Dado_um_comando_invalido_o_pedido_nao_deve_ser_gerado()
        {

        }*/

        [Fact]
        public void Dado_um_comando_valido_o_pedido_deve_ser_gerado()
        {
            var command = new CreateOrderCommand
            {
                Customer = "12345678910",
                ZipCode = "12345678",
                PromoCode = "123"
            };
            command.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));

            var orderHandler = new OrderHandler(_customerRepository, _deliveryFeeRepository, _discountRepository, _productRepository, _orderRepository);

            orderHandler.Handle(command);

            Assert.True(orderHandler.Valid);
        }
    }
}
