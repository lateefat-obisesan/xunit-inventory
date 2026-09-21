using Xunit;
using InventorySystem;

namespace InventorySystem.Tests
{
    public class InventoryOrderServiceTests
    {
        [Fact]
        public void ProcessOrder_ValidOrder_ReturnsSuccess()
        {
            // Arrange
            InventoryOrderService orderService = new InventoryOrderService();

            Product product = new Product
            {
                Id = "P100",
                Name = "Keyboard",
                UnitPrice = 100.00m,
                StockQuantity = 20
            };

            orderService.AddProduct(product);

            // Act
            OrderResult result = orderService.ProcessOrder("P100", 2, 0.05m);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(210.00m, result.TotalCost);
            Assert.Equal("Order processed successfully.", result.Message);
        }

        [Fact]
        public void ProcessOrder_ValidOrder_DeductsStock()
        {
            //Arrange
            InventoryOrderService orderService = new InventoryOrderService();

            Product product = new Product
            {
                Id = "P100",
                Name = "Keyboard",
                UnitPrice = 100.00m,
                StockQuantity = 20
            };

            orderService.AddProduct(product);

            // Act
           OrderResult result = orderService.ProcessOrder("P100", 5, 0.00m);

            // Assert
            Product updatedProduct = orderService.GetProduct("P100");

            Assert.NotNull(updatedProduct);
            Assert.Equal(15, updatedProduct.StockQuantity);
        }

        [Fact]
        public void ProcessOrder_ZeroTax_ReturnsCorrectTotal()
        {

        }
    }
}

