using Store.Domain.Commands;
using System;
using Xunit;

namespace Store.Tests.Commands
{
    public class CreateOrderCommandTests
    {
        [Fact]
        public void Dado_um_comando_invalido_o_pedido_nao_deve_ser_gerado()
        {
            var command = new CreateOrderCommand
            {
                Customer = string.Empty,
                ZipCode = string.Empty,
                PromoCode = null
            };
            command.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));

            command.Validate();

            Assert.True(command.Invalid);
        }
    }
}
