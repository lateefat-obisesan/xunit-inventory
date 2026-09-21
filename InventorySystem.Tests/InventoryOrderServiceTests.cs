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
        public void ProcessOrder_ZeroTax_ReturnsRightTotal()
        {
            //Arrange
            InventoryOrderService orderService = new InventoryOrderService();
            Product product = new Product
            {
                Id = "P100",
                Name = "Keyboard",
                UnitPrice = 50.00m,
                StockQuantity = 20
            };
            orderService.AddProduct(product);

            // Act
            OrderResult result = orderService.ProcessOrder("P100", 2, 0.00m);

            // Assert
            Assert.Equal(100.00m, result.TotalCost);
        }
        [Fact]
        public void ProcessOrder_QuantityOf10_ReturnsCorrectTotal()
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
            OrderResult result = orderService.ProcessOrder("P100", 10, 0.00m);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(900.00m, result.TotalCost);
        }
        [Fact]
        public void ProcessOrder_QuantityOf50_Appilies20PercentDiscount()
        {
            //Arrange
            InventoryOrderService orderService = new InventoryOrderService();
            Product product = new Product
            {
                Id = "P100",
                Name = "Keyboard",
                UnitPrice = 100.00m,
                StockQuantity = 60
            };

            orderService.AddProduct(product);

            // Act
            OrderResult result = orderService.ProcessOrder("P100", 50, 0.00m);

            // Assert
            Assert.Equal(4000.00m, result.TotalCost);
        }
        [Fact]
        public void ProcessOrder_ZeroQuantity_ReturnsError()
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
            OrderResult result = orderService.ProcessOrder("P100", 0, 0.00m);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Quantity must be positive.", result.Message);
        }
        [Fact]
        public void ProcessOrder_ProductDoesNotExist_ReturnsProductNotFound()
        {
            //Arrange
            InventoryOrderService orderService = new InventoryOrderService();

            //Act
            OrderResult result = orderService.ProcessOrder("P999", 2, 0.05m);

            //Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Product not found.", result.Message);
        }
        [Fact]
        public void ProcessOrder_QuantityExceedsStock_ReturnsInsufficientStock()
        {
            // Arrange
            InventoryOrderService orderService = new InventoryOrderService();
            Product product = new Product
            {
                Id = "P100",
                Name = "Keyboard",
                UnitPrice = 100.00m,
                StockQuantity = 5
            };

            orderService.AddProduct(product);

            // Act
            OrderResult result = orderService.ProcessOrder("P100", 6, 0.05m);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Insufficient stock.", result.Message);
        }
        [Fact]
        public void ProcessOrder_NegativeQuantity_ReturnsError()
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
            OrderResult result = orderService.ProcessOrder("P100", -1, 0.00m);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Quantity must be positive.", result.Message);
        }
        [Fact]
        public void AddProduct_NullProduct_ThrowsArgumentException()
        {
            //Arrange
            InventoryOrderService orderService = new InventoryOrderService();

            // Act & Assert
            ArgumentException exception = Assert.Throws<ArgumentException>(
                () => orderService.AddProduct(null)
            );

            Assert.Equal("Invalid product details.", exception.Message);
        }
        [Fact]
        public void AddProduct_EmptyProductId_ThrowsArgumentException()
        {
            // Arrange
            InventoryOrderService orderService = new InventoryOrderService();

            Product product = new Product
            {
                Id = "",
                Name = "Keyboard",
                UnitPrice = 100.00m,
                StockQuantity = 20
            };

            // Act & Assert
            ArgumentException exception = Assert.Throws<ArgumentException>(
                () => orderService.AddProduct(product)
            );

            Assert.Equal("Invalid product details.", exception.Message);
        }
    }
}

