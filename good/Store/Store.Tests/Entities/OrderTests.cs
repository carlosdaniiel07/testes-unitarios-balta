using Store.Domain.Entities;
using Store.Domain.Enums;
using System;
using Xunit;

namespace Store.Tests.Entities
{
    public class OrderTests
    {
        private readonly Customer _customer = new Customer("Customer", "customer@email.com");
        private readonly Product _product = new Product("Product", 10, true);
        private readonly Discount _discount = new Discount(5, DateTime.Now.AddDays(2));
        private readonly Discount _expiredDiscount = new Discount(10, DateTime.Now.AddDays(-2));

        [Fact]
        public void Dado_um_novo_pedido_valido_deve_ser_gerado_um_numero_com_8_caracteres()
        {
            var order = GetMockedOrder();

            Assert.Equal(8, order.Number.Length);
        }

        [Fact]
        public void Dado_um_novo_pedido_valido_seu_status_deve_ser_aguardando_pagamento()
        {
            var order = GetMockedOrder();

            Assert.Equal(OrderStatus.WaitingPayment, order.Status);
        }

        [Fact]
        public void Dado_um_pagamento_de_pedido_seu_status_deve_ser_aguardando_entrega()
        {
            var order = GetMockedOrder();

            order.AddItem(_product, 1);
            order.Pay(10);

            Assert.Equal(OrderStatus.WaitingDelivery, order.Status);
        }

        [Fact]
        public void Dado_um_pedido_cancelado_seu_status_deve_ser_cancelado()
        {
            var order = GetMockedOrder();

            order.Cancel();

            Assert.Equal(OrderStatus.Canceled, order.Status);
        }

        [Fact]
        public void Dado_um_novo_item_sem_produto_o_mesmo_nao_deve_ser_adicionado()
        {
            var order = GetMockedOrder();

            order.AddItem(null, 1);

            Assert.Equal(0, order.Items.Count);
        }

        [Fact]
        public void Dado_um_novo_item_com_quantidade_zero_ou_menor_o_mesmo_nao_deve_ser_adicionado()
        {
            var order = GetMockedOrder();

            order.AddItem(_product, 0);
            order.AddItem(_product, -1);

            Assert.Equal(0, order.Items.Count);
        }

        [Fact]
        public void Dado_um_novo_pedido_valido_seu_total_deve_ser_de_50()
        {
            var order = GetMockedOrder();

            order.AddItem(_product, 5);

            Assert.Equal(50, order.Total());
        }

        [Fact]
        public void Dado_um_desconto_expirado_o_valor_do_pedido_deve_ser_de_60()
        {
            var order = GetMockedOrder(_expiredDiscount);

            order.AddItem(_product, 6);

            Assert.Equal(60, order.Total());
        }

        [Fact]
        public void Dado_um_desconto_invalido_o_valor_do_pedido_deve_ser_de_55()
        {
            var order = GetMockedOrder();

            order.AddItem(_product, 6);

            Assert.Equal(60, order.Total());
        }

        [Fact]
        public void Dado_um_desconto_valido_de_5_o_valor_do_pedido_deve_ser_de_55()
        {
            var order = GetMockedOrder(_discount);

            order.AddItem(_product, 6);

            Assert.Equal(55, order.Total());
        }

        [Fact]
        public void Dado_um_pedido_valido_com_uma_taxa_de_entrega_de_20_seu_valor_deve_ser_70()
        {
            var order = new Order(_customer, 20, null);

            order.AddItem(_product, 5);

            Assert.Equal(70, order.Total());
        }

        [Fact]
        public void Dado_um_pedido_sem_cliente_o_mesmo_deve_ser_invalido()
        {
            var order = new Order(null, 0, null);

            Assert.True(order.Invalid);
        }

        private Order GetMockedOrder(Discount discount = null)
        {
            return new Order(_customer, 0, discount);
        }
    }
}
